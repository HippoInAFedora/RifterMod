using BepInEx;
using RifterMod.Survivors.Rifter;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using System.Runtime.CompilerServices;
using R2API;
using UnityEngine;

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

            Damage.SetupModdedDamage();

            // character initialization
            new RifterSurvivor().Initialize();


            // make a content pack and add it. this has to be last
            new Modules.ContentPacks().Initialize();

            Hook();

        }

        private static void Hook()
        {
            //RoR2.GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
        }

        private static void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, RoR2.GlobalEventManager self, RoR2.DamageInfo damageInfo, GameObject victim)
        {
            //IL_0013: Unknown result type (might be due to invalid IL or missing references)
            orig(self, damageInfo, victim);
            CharacterModel model = victim.GetComponent<CharacterModel>();
            if (DamageAPI.HasModdedDamageType(damageInfo, Damage.riftDamage) || DamageAPI.HasModdedDamageType(damageInfo, Damage.riftAssistDamage))
            {

            }
        }

        public static void AddBodyToBlacklist(string bodyName)
        {
            BodyIndex bodyIndex = RoR2.BodyCatalog.FindBodyIndex(bodyName);
            TeamComponent teamComponent = BodyCatalog.FindBodyPrefab(bodyName).GetComponent<CharacterBody>().teamComponent;

            if (bodyIndex != BodyIndex.None)
            {
                if (blacklistBodyNames.Contains(bodyName) || teamComponent.teamIndex == TeamIndex.Lunar)
                {
                    blacklist.Add(bodyIndex);
                }

            }
        }
    }
}
