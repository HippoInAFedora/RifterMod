using BepInEx;
using RifterMod.Survivors.Rifter;
using R2API.Utils;
using RoR2;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using R2API;
using UnityEngine;
using RifterMod.Modules;
using RiskOfOptions.Options;
using RifterMod.Characters.Survivors.Rifter.Components;
using BepInEx.Bootstrap;
using System.Runtime.CompilerServices;
using RiskOfOptions;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]


//rename this namespace
namespace RifterMod
{
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [BepInDependency("com.weliveinasociety.CustomEmotesAPI", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.DestroyedClone.AncientScepter", BepInDependency.DependencyFlags.SoftDependency)]
    public class RifterPlugin : BaseUnityPlugin
    {
        // if you do not change this, you are giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.blake.RifterMod";
        public const string MODNAME = "RifterMod";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "BLAKE";

        public static RifterPlugin instance;

        public static BodyIndex rifterIndex;

        public static GameObject hudInstance;

        public static List<BodyIndex> blacklist = new List<BodyIndex>();

        public static List<string> blacklistBodyNames = new List<string> { "MinorConstructBody(Clone)", "VoidBarnacleBody(Clone)" };

        public static bool ScepterInstalled;

        public static bool riskOfOptionsLoaded;

        void Awake()
        {
            instance = this;
            //easy to use logger
            Log.Init(Logger);

            // used when you want to properly set up language folders
            Modules.Language.Init();

            ScepterInstalled = Chainloader.PluginInfos.ContainsKey("com.DestroyedClone.AncientScepter");
            riskOfOptionsLoaded = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");


            // character initialization
            new RifterSurvivor().Initialize();

            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();

            Hook();

        }

        public void Start()
        {
            SetupRifterPlugin();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void SetupRifterPlugin()
        {       
            if (riskOfOptionsLoaded)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(RifterConfig.distanceAssist));
                ModSettingsManager.AddOption(new CheckBoxOption(RifterConfig.HUD));
                ModSettingsManager.AddOption(new CheckBoxOption(RifterConfig.teleportYourFriends));
                //ModSettingsManager.AddOption(new CheckBoxOption(RifterConfig.cursed));
            }
        }

        private static void Hook()
        {
            On.RoR2.BodyCatalog.Init += BodyCatalog_Init;
            //On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            //On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
            On.RoR2.UI.HUD.Awake += HUD_Awake;
            On.RoR2.UI.HUD.Update += HUD_Update;
        }

        private static void BodyCatalog_Init(On.RoR2.BodyCatalog.orig_Init orig)
        {
            orig();
            rifterIndex = BodyCatalog.FindBodyIndex("RifterBody(Clone)");
        }

        private static void HUD_Update(On.RoR2.UI.HUD.orig_Update orig, RoR2.UI.HUD self)
        {
            orig(self);
            CharacterBody body = self.targetBodyObject.GetComponent<CharacterBody>();
            if (RifterConfig.HUD.Value == true && body && body.bodyIndex == rifterIndex)
            {
                hudInstance.SetActive(true);
            }
            else
            {
                hudInstance.SetActive(false);
            }
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig(self, damageInfo, victim);
            if (!self)
            {
                return;
            }
            CharacterBody body = damageInfo.attacker ? damageInfo.attacker.GetComponent<CharacterBody>() : null;
            CharacterBody victimBody = victim.GetComponent<CharacterBody>();
            if (body && body.bodyIndex == rifterIndex)
            {
                if (victimBody != null && DamageAPI.HasModdedDamageType(damageInfo, RifterDamage.riftDamage))
                {
                    victimBody.AddBuff(RifterBuffs.shatterDebuff);

                    bool crippling = Util.CheckRoll(20f + 2.5f * victimBody.GetBuffCount(RifterBuffs.shatterDebuff));
                    if (crippling)
                    {
                        victimBody.AddBuff(RoR2Content.Buffs.Cripple);
                        int buffCount = victimBody.GetBuffCount(RifterBuffs.shatterDebuff);
                        for (int i = 0; i < buffCount; i++)
                        {
                            victimBody.RemoveBuff(RifterBuffs.shatterDebuff);
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.CharacterModel.UpdateOverlays -= CharacterModel_UpdateOverlays;
            On.RoR2.UI.HUD.Update -= HUD_Update;
            On.RoR2.UI.HUD.Awake -= HUD_Awake;
        }

        



        private static void HUD_Awake(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            orig(self);
            hudInstance = Object.Instantiate(RifterAssets.overchargeHUD);
            hudInstance.transform.SetParent(self.mainContainer.transform);
            RectTransform component = hudInstance.GetComponent<RectTransform>();
            //component.anchorMin = new Vector2(.5f, .5f);
            //component.anchorMax = new Vector2(.5f, .5f);
            //component.sizeDelta = Vector2.zero;
            //component.localScale = Vector2.zero;
            component.anchoredPosition = new Vector2(50, 0);
            OverchargeMeter.fill = hudInstance.GetComponent<Image>();
            OverchargeMeter.counter = hudInstance.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();         
        }

        private static void CharacterModel_UpdateOverlays(On.RoR2.CharacterModel.orig_UpdateOverlays orig, CharacterModel self)
        {
            orig(self);
            if (self && self.body)
            {
                CharacterBody body = self.body;
                ShatterOverlay component = body.gameObject.GetComponent<ShatterOverlay>();               
            }
            
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (!self)
            {
                return;
            }
            if (self.HasBuff(RifterBuffs.shatterDebuff))
            {
                int shatterStacks = self.GetBuffCount(RifterBuffs.shatterDebuff);
                self.cursePenalty += shatterStacks * 5f / 100f;
                if (shatterStacks < 5)
                {
                    self.armor -= shatterStacks * 1f;
                }
                if (5 <= shatterStacks && shatterStacks < 10)
                {
                    self.armor -= shatterStacks * 2f;
                }
                if (shatterStacks >= 10)
                {
                    self.armor -= shatterStacks * 4f;
                }
            }
        }

        public static void AddBodyToBlacklist(string bodyName)
        {
            BodyIndex bodyIndex = BodyCatalog.FindBodyIndex(bodyName);
            TeamComponent teamComponent = BodyCatalog.FindBodyPrefab(bodyName).GetComponent<CharacterBody>().teamComponent;

            if (bodyIndex != BodyIndex.None)
            {
                if (blacklistBodyNames.Contains(bodyName))
                {
                    blacklist.Add(bodyIndex);
                }

            }
        }
    }
}
