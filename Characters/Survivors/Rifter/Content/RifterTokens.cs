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
             + "Your Ranged Gauntlet encourages strong positioning to hit in the rift." + Environment.NewLine + Environment.NewLine
             + "Your secondaries fill in your midrange. Rift Gauntlet is solid and stable, Rapidfire Gauntlet shoots quickly and teleports on its last shot, and Refraction shoots 3 rifts at once." + Environment.NewLine + Environment.NewLine
             + "Slipstream acts as a repositioning tool and a way to move enemies into the sweet spot, while Rift Rider lets you reposition yourself AND the enemy." + Environment.NewLine + Environment.NewLine
             + "Recursion is a good get-off-me tool, dealing close range damage and teleporting enemies away." + Environment.NewLine + Environment.NewLine;

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
            Language.Add(prefix + "PASSIVE_NAME", "Rift/Fracture");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Each shot is comprised of a strong, large Rift at the end of the shot, and a thin, weak Fracture Line that travels from you to the Rift. Rifts do less damage on contact with the ground.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED", "Rift Gauntlet Ranged");
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION", $"Shoot a far ranged rift for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GAUNTLET", "Rift Gauntlet");
            Language.Add(prefix + "SECONDARY_GAUNTLET_DESCRIPTION", $"Shoot a midrange rift <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>. There is no cooldown.");

            Language.Add(prefix + "SECONDARY_RAPIDFIRE", "Rapidfire Gauntlet");
            Language.Add(prefix + "SECONDARY_RAPIDFIRE_DESCRIPTION", $"Shoot a midrange rift at twice the speed for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>. Last stock  <style=cIsUtility>Teleports</style> enemies.");

            Language.Add(prefix + "SECONDARY_REFRACTION", "Refraction");
            Language.Add(prefix + "SECONDARY_REFRACTION_DESCRIPTION", $"Shoot a spread of 3 midrange rifts for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style> each. On a cooldown.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_SLIPSTREAM", "Slipstream");
            Language.Add(prefix + "UTILITY_SLIPSTREAM_DESCRIPTION", "Teleport a short distance. Your next shot will <style=cIsUtility>teleport</style> enemies to primary distance. You are Invulnerable during your teleport.");

            Language.Add(prefix + "UTILITY_RIFT_RIDER", "Rift Rider");
            Language.Add(prefix + "UTILITY_RIFT_RIDER_DESCRIPTION", "Traverse through a Fracture Line to a primary rift. <style=cIsUtility>Teleport</style> enemies to your previous location. You are Invulnerable during your teleport.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_RECURSION", "Recursion");
            Language.Add(prefix + "SPECIAL_RECURSION_DESCRIPTION", $"Charge a up to 5 rifts stacked on your location. Initial rift is <style=cIsDamage>{100f * RifterStaticValues.recursionCoefficient}% damage</style>, subsequent rifts are 20% larger and stronger than the last.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier), "Rifter: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(RifterMasteryAchievement.identifier), "As Rifter, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
