using RifterMod.Modules;
using RifterMod.Survivors.Rifter.Achievements;
using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static UnlockableDef riftFocusUnlockableDef = null;
        public static UnlockableDef buckshotUnlockableDef = null;

        //public static UnlockableDef scattershotUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                RifterMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier),
                RifterSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));

           buckshotUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(BuckshotAchievement.unlockableIdentifier, BuckshotAchievement.identifier, null);
        }
    }
}
