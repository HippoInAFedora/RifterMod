using System;
using RifterMod.Modules;
using RifterMod.Survivors.Rifter.Achievements;
using RifterMod.Survivors.Rifter.SkillStates;

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
             + "<!> Rifts are significantly stronger than fracture lines - positioning is key!" + Environment.NewLine + Environment.NewLine
             + "<!> Utilizing your primary's long-range and mid-range capabilities allow you to mix up your positions." + Environment.NewLine + Environment.NewLine
             + "<!> Fracture lines are weak and unable to crit or activate items, but their ability to use overcharge allows you to keep enemies at bay." + Environment.NewLine + Environment.NewLine
             + "<!> Timelock can be a powerful tool for gathering enemies and blasting them all at once!" + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left a fractured world behind.";
            string outroFailure = "..and so he vanished from all realms.";

            string lore = "\"...Funny...\"\r\n\r\n\"Yeah. Real funny.\"\r\n\r\n\"It just... appeared?\"\r\n\r\n\"Well that's what they're sticking to, at least.\"\r\n\r\n\"Huh. Never imagined <i>we'd</i> be the ones they'd keep secrets from.\"\r\n\r\n\"I guess they can't have their Science Division knowing everything that comes their way.\"\r\n\r\nHannegan peered through the strange, glassy substance. He wanted to describe it as glass, but his need for concrete evidence got the better of him. He also wanted to tap it.\r\n\r\nHe tapped it.\r\n\r\nIt resonated in a very peculiar frequency, almost... like it was clipping in and out of time? He kept the thought in mind, but hesitated to draw conclusions from it.\r\n\r\n\"Hannegan, you know how much I hate it when you touch samples without permission.\"\r\n\r\n\"Permission to touch it?\"\r\n\r\n\"...Yes... It's not reactive to touch.\"\r\n\r\n\"Did they specify the material of this... glassy stuff?\"\r\n\r\n\"No. And it's not our point of interest.\"\r\n\r\nThe lead scientist, D.O. Harold Schmeuller, gestured toward the glassy substance, but not the substance itself. He pointed at what was <i>inside</i>.\r\n\r\n\"They say they've got readings of a crude, yet incomprehensibly advanced object which has the ability to teleport objects.\"\r\n\r\n\"You make that sound special. We've already worked out a way to teleport things. What's the difference?\"\r\n\r\n\"The intel I have been given suggests it appeared -- not just through space -- but through <i>time</i>.\"\r\n\r\n\"Time? That's impossi-\"\r\n\r\n\"Impossible, yes. Unless it isn't. Our task here is to find out how this thing teleports through time and space, and to replicate it. Project Rifter, is what they're calling it. Also, remember, Hannegan, we test weapons, <i>not</i> convenience items.";

            Language.Add(prefix + "NAME", "Rifter");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Un-Whole");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Survivalist");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_RIFT_BOOST", "Imperfection");
            Language.Add(prefix + "PASSIVE_RIFT_BOOST_DESCRIPTION", "Rifts have the chance to <style=cIsUtility>Cripple</style> enemies. The chance increases each time an enemy is hit.");


            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE", "Rift/Fracture");
            Language.Add(prefix + "PASSIVE_RIFT_FRACTURE_DESCRIPTION", "Rifts weaken the closer they are to rifter. Ranged rifts are connected by weaker Fracture Lines that deal <style=cIsDamage>80% damage</style> that are  <style=cIsHealth>unable to activate item effects</style>.");

            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED", "Focused Rift");
            Language.Add(prefix + "PRIMARY_GAUNTLET_RANGED_DESCRIPTION", $"<style=cIsUtility>Fracture</style>. Shoot a far-ranged rift for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient}% damage</style>. If secondary skill is held, shoot a mid-range rift for <style=cIsDamage>{100f * RifterStaticValues.primaryRiftCoefficient * .8}%</style>.");

            Language.Add(prefix + "PRIMARY_BUCKSHOT", "Scattered Rifts");
            Language.Add(prefix + "PRIMARY_BUCKSHOT_DESCRIPTION", $"<style=cIsUtility>Fracture</style>. Shoot a rift for <style=cIsDamage>280% damage</style> that ripples for <style=cIsDamage>5x120% damage</style>. If secondary skill is held, shoot at mid-range.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_CHAINED_WORLDS", "Chained Worlds");
            Language.Add(prefix + "SECONDARY_CHAINED_WORLDS_DESCRIPTION", $"Shoot up to 5 rifts in a line, dealing <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style> per rift. Rifts <style=cIsUtility>teleport enemies to the next</style>.");

            Language.Add(prefix + "SECONDARY_FRACTURE", "Fracture Shot");
            Language.Add(prefix + "SECONDARY_FRACTURE_DESCRIPTION", $"<style=cIsUtility>Overcharged</style>. Shoot <style=cIsDamage>3x{100f * RifterStaticValues.fractureCoefficient}% damage</style> non-piercing fracture shots.");

            Language.Add(prefix + "SECONDARY_FAULT_LINE", "Fault Line");
            Language.Add(prefix + "SECONDARY_FAULT_LINE_DESCRIPTION", $"<style=cIsUtility>Overcharged</style>. Shoot a piercing fracture shot for <style=cIsDamage>{100f * RifterStaticValues.fractureCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_SLIPSTREAM", "Slipstream");
            Language.Add(prefix + "UTILITY_SLIPSTREAM_DESCRIPTION", "<style=cIsUtility>Teleport</style> a short distance, <style=cIsUtility>cleansing all debuffs</style>. Replenishes secondary skill stock.");

            Language.Add(prefix + "UTILITY_QUANTUM_RIFT", "Quantum Portals - HOST ONLY");
            Language.Add(prefix + "UTILITY_QUANTUM_RIFT_DESCRIPTION", "<style=cIsUtility>Fracture</style>. Dash back and create two portals, each blasting for <style=cIsDamage>150% damage</style> per second. Portals <style=cIsUtility>teleport</style> to each other.");

            Language.Add(prefix + "UTILITY_RIFT_RIDER", "Rift Rider");
            Language.Add(prefix + "UTILITY_RIFT_RIDER_DESCRIPTION", "Travel through a long-range Fracture Line, cleansing all debuffs. Each hit enemy is <style=cIsUtility>teleported to your previous location</style> and adds <style=cIsUtility>overcharge</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_TIMELOCK", "Timelock");
            Language.Add(prefix + "SPECIAL_TIMELOCK_DESCRIPTION", $"<style=cIsUtility>Crushing</style>. Create a field that <style=cIsVoid>roots</style> and directs rift teleportation to itself. <style=cIsUtility>Hitting it with a rift</style> collapses the timelock for <style=cIsDamage>500% damage</style>.");

           
            #endregion

            #region Scepter
            Language.Add(prefix + "SPECIAL_RECURSION_SCEPTER", "To Singularity");
            Language.Add(prefix + "SPECIAL_RECURSION_DESCRIPTION_SCEPTER", $"Position 10 stacked rifts, dealing <style=cIsDamage>{100f * 3.5f}%  damage</style> <style=cStack>(* 120% per rift)</style>. Each rift is <style=cIsDamage>5% smaller</style> than the last.");

            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS_SCEPTER", "Colliding Worlds");
            Language.Add(prefix + "SPECIAL_CHAINED_WORLDS_DESCRIPTION_SCEPTER", $"Charge and shoot 7 rifts in a line, dealing <style=cIsDamage>{100f * RifterStaticValues.secondaryRiftCoefficient}% damage</style> <cStack/style> (* 120% per rift) <style=cStack> per rift. Rifts <style=cIsUtility>teleport enemies to the next</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(RifterMasteryAchievement.identifier), "Rifter: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(RifterMasteryAchievement.identifier), "As rifter, beat the game or obliterate on monsoon.");

            Language.Add(Tokens.GetAchievementNameToken(BuckshotAchievement.identifier), "Rifter: tangling reality");
            Language.Add(Tokens.GetAchievementDescriptionToken(BuckshotAchievement.identifier), "Teleport 10 or more enemies in a single move.");
            #endregion
        }
    }
}
