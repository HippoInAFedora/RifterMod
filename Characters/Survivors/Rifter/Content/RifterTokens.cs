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

            string desc = "Rifter utilizes strategic positioning to create devestating rifts in reality.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "<!> Your Ranged Gauntlet encourages strong positioning to hit in the rift." + Environment.NewLine + Environment.NewLine
             + "<!> Use your secondary to fill in your midrange." + Environment.NewLine + Environment.NewLine
             + "<!> Slipstream acts as a repositioning tool and a way to move enemies into the sweet spot, while Rift Rider lets you reposition yourself AND the enemy." + Environment.NewLine + Environment.NewLine
             + "<!> Recursion is a good get-off-me tool, dealing close range damage and teleporting enemies away." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left a fractured world behind.";
            string outroFailure = "..and so he vanished from all realms.";

            Language.Add(prefix + "NAME", "Rifter");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Fractured Scientist");
            Language.Add(prefix + "LORE", "ye ye ye");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Survivalist");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE", "Rift/Fracture");
            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE_DESCRIPTION", "Rifts weaken if closer to you. Ranged rifts are connected by weaker Fracture Lines that deal <style=cIsDamage>100% damage</style> that are  <style=cIsHealth>unable to activate item effects</style>.");

            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED", "Rift Gauntlet Ranged");
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION", $"Shoot a far-ranged rift for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient}% damage</style>. Can be <style=cIsUtility>overcharged</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GAUNTLET", "Rift Gauntlet");
            Language.Add(prefix + "SECONDARY_GAUNTLET_DESCRIPTION", $"Shoot a midrange rift <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>. Incurs no cooldown. Can be <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "SECONDARY_RAPIDFIRE", "Rapidfire Gauntlet");
            Language.Add(prefix + "SECONDARY_RAPIDFIRE_DESCRIPTION", $"Shoot a midrange rift for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>. Hold up to 3. Last shot is <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "SECONDARY_REFRACTION", "Refraction");
            Language.Add(prefix + "SECONDARY_REFRACTION_DESCRIPTION", $"Shoot a spread of 3 midrange rifts for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient - 50f}% damage</style> each. Always <style=cIsUtility>overcharged</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_SLIPSTREAM", "Slipstream");
            Language.Add(prefix + "UTILITY_SLIPSTREAM_DESCRIPTION", "Teleport a short distance, <style=cIsUtility>briefly turning invulnerable</style>. Your next shot is <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "UTILITY_RIFT_RIDER", "Rift Rider");
            Language.Add(prefix + "UTILITY_RIFT_RIDER_DESCRIPTION", "Travel through a long-range Fracture Line, dealing <style=cIsDamage>100% damage</style>. All hit enemies will be <style=cIsUtility>teleported to your previous location</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_RECURSION", "Recursion");
            Language.Add(prefix + "SPECIAL_RECURSION_DESCRIPTION", $"Charge a up to 5 rifts centered on you for <style=cIsDamage>{100f * RifterStaticValues.recursionCoefficient}% damage</style>, with each <style=cIsDamage>20% larger and stronger damage</style> than the last. The last rift is <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS", "Chained Worlds");
            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS_DESCRIPTION", $"Charge a up to 5 rifts stepped out for <style=cIsDamage>{100f * RifterStaticValues.chainedWorldsCoefficient}% damage</style>, with each <style=cIsDamage>20% larger and stronger damage</style> than the last. Each rift <style=cIsUtility>teleports enemies to the next rift</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier), "Rifter: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(RifterMasteryAchievement.identifier), "As Rifter, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
