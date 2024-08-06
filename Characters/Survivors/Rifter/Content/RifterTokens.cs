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

            string desc = "The Rifter utilizes strategic positioning to create devestating rifts in reality.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "<!> Rifts apply shatter and are significantly stronger than fracture lines - positioning is key!" + Environment.NewLine + Environment.NewLine
             + "<!> Rift Gauntlet and Rift Gauntlet Ranged guide your positioning, allowing for mid-range and long-range combat." + Environment.NewLine + Environment.NewLine
             + "<!> Fracture lines are weak and unable to crit or activate items, but their ability to use overcharge allows you to keep enemies at bay." + Environment.NewLine + Environment.NewLine
             + "<!> Shatter helps you to remain a threat when playing the long game, weakening enemies the more stacks they have." + Environment.NewLine + Environment.NewLine;

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
            Language.Add(prefix + "PASSIVE_RIFT_BOOST", "Shatter");
            Language.Add(prefix + "PASSIVE_RIFT_BOOST_DESCRIPTION", "Rifts <style=cIsHealth>shatter</style> a percent of enemy health and armor.");


            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE", "Rift/Fracture");
            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE_DESCRIPTION", "Rifts weaken the closer they are to rifter. Ranged rifts are connected by weaker Fracture Lines that deal <style=cIsDamage>100% damage</style> that are  <style=cIsHealth>unable to activate item effects</style>.");

            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED", "Rift Gauntlet Ranged");
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION", $"Shoot a far-ranged rift for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient}% damage</style>. Can be <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "PRIMARY_BUCKSHOT", "Rift Buckshot");
            Language.Add(prefix + "PRIMARY_BUCKSHOT_DESCRIPTION", $"Shoot a far-ranged rift for <style=cIsDamage>{100f * RifterStaticValues.buckshotRiftCoefficient}% damage</style>, along with  <style=cIsDamage>5 x {100f * RifterStaticValues.buckshotWeakRiftCoefficient}% damage</style> inaccurate small rifts. <style=cIsUtility>Overcharge</style> adds <style=cIsDamage>3 x {100f * RifterStaticValues.buckshotWeakRiftCoefficient}% damage</style> rifts.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GAUNTLET", "Rift Gauntlet");
            Language.Add(prefix + "SECONDARY_GAUNTLET_DESCRIPTION", $"Shoot a midrange rift for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style>. Incurs no cooldown. Can be <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "SECONDARY_RAPIDFIRE", "Rapidfire Gauntlet");
            Language.Add(prefix + "SECONDARY_RAPIDFIRE_DESCRIPTION", $"Shoot a midrange rift for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficientAlt1}% damage</style>. Hold up to 3. Last shot is <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "SECONDARY_REFRACTION", "Refraction");
            Language.Add(prefix + "SECONDARY_REFRACTION_DESCRIPTION", $"Shoot a spread of 3 midrange rifts for <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficientAlt2}% damage</style> each. Always <style=cIsUtility>overcharged</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_SLIPSTREAM", "Slipstream");
            Language.Add(prefix + "UTILITY_SLIPSTREAM_DESCRIPTION", "Teleport a short distance, <style=cIsUtility>briefly turning invulnerable</style>. The next shot is <style=cIsUtility>overcharged</style>.");

            Language.Add(prefix + "UTILITY_RIFT_RIDER", "Rift Rider");
            Language.Add(prefix + "UTILITY_RIFT_RIDER_DESCRIPTION", "Travel through a long-range Fracture Line, dealing <style=cIsDamage>100% damage</style>. Each hit enemy will be <style=cIsUtility>teleported to your previous location</style> and add 1 <style=cIsUtility>overcharge</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_RECURSION", "Recursion");
            Language.Add(prefix + "SPECIAL_RECURSION_DESCRIPTION", $"Position 5 stacked rifts, dealing <style=cIsDamage>{100f * 26f}% damage</style>. Each rift is <style=cIsDamage>20% larger</style> than the last.");

            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS", "Chained Worlds");
            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS_DESCRIPTION", $"Charge and shoot 5 rifts in a line, dealing <style=cIsDamage>{100f * RifterStaticValues.chainedWorldsCoefficient}% damage</style> per rift. Rifts <style=cIsUtility>teleport enemies to the next</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier), "Rifter: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(RifterMasteryAchievement.identifier), "As Rifter, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
