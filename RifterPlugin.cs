using BepInEx;
using RifterMod.Survivors.Rifter;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using R2API;
using UnityEngine;
using RifterMod.Modules;
using RoR2.DispatachableEffects;


[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace RifterMod
{
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
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

        public static List<BodyIndex> blacklist = new List<BodyIndex>();

        public static List<string> blacklistBodyNames = new List<string> { "MinorConstructBody(Clone)", "VoidBarnacleBody(Clone)" };

        void Awake()
        {
            instance = this;
            //easy to use logger
            Log.Init(Logger);

            // used when you want to properly set up language folders
            Modules.Language.Init();

            

            // character initialization
            new RifterSurvivor().Initialize();


            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();

            Hook();

        }

        private static void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.OnTakeDamageServer += CharacterBody_OnTakeDamageServer;
            On.RoR2.CharacterModel.UpdateOverlays += CharacterModel_UpdateOverlays;
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

        private static void CharacterBody_OnTakeDamageServer(On.RoR2.CharacterBody.orig_OnTakeDamageServer orig, CharacterBody self, DamageReport damageReport)
        {
            orig(self, damageReport);
            if (!self)
            {
                return;
            }
            if (DamageAPI.HasModdedDamageType(damageReport.damageInfo, RifterDamage.riftDamage))
            {
                Debug.Log("character can proc riftDamage");
                if (self.GetBuffCount(RifterBuffs.shatterDebuff) < 20)
                {
                    self.AddBuff(RifterBuffs.shatterDebuff);
                    if (!self.gameObject.GetComponent<ShatterOverlay>())
                    {
                        self.gameObject.AddComponent<ShatterOverlay>();
                    }
                    
                }
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
