using RoR2;
using RifterMod.Modules.Achievements;

namespace RifterMod.Survivors.Rifter.Achievements
{
    //Automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10)]
    public class RifterMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = RifterSurvivor.Rifter_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = RifterSurvivor.Rifter_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => RifterSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}