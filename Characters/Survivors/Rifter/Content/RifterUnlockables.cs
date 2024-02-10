using RifterMod.Survivors.Rifter.Achievements;
using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                RifterMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier),
                RifterSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
