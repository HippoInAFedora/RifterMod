using System;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter.Achievements;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterTokens
    {
        public static void Init()
        {
            AddRifterTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            Language.PrintOutput("Rifter.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddRifterTokens()
        {
            string prefix = RifterSurvivor.Rifter_PREFIX;

            string desc = "Rifter utilizes position to create devestating rifts in reality.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Rift Gauntlet encourages strong positioning to hit in the sweet spot." + Environment.NewLine + Environment.NewLine
             + "< ! > Refracted Realities ." + Environment.NewLine + Environment.NewLine
             + "< ! > Slipstream acts as a repositioning tool and a way to move enemies into the sweet spot." + Environment.NewLine + Environment.NewLine
             + "< ! > Rift Collapse can shatter and destroy enemies with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left a fractured world behind.";
            string outroFailure = "..and so he vanished from reality.";

            Language.Add(prefix + "NAME", "Rifter");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Divided");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Rifter passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * RifterStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier), "Rifter: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(RifterMasteryAchievement.identifier), "As Rifter, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
