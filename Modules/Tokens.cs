﻿namespace RifterMod.Modules
{
    internal static class Tokens
    {
        public const string overchargedPrefix = "<style=cIsUtility>Overcharge</style>";

        public const string overchargedChainedPrefix = "<style=cIsUtility>Overcharge</style>";

        //public const string stuntingPrefix = "<style=cIsUtility>Inverse Falloff</style>";

        public const string fracturePrefix = "<style=cIsUtility>Fracture</style>";

        public static string overchargedKeyword = KeywordText("Overcharged", "Enemies hit are teleported to Rifter's primary-rift distance.");

        public static string overchargedChainedKeyword = KeywordText("Overcharged", "Enemies hit are teleported to the next Rift.");

        //public static string stuntingKeyword = KeywordText("Inverse Falloff", "Rift is weaker the closer it is to Rifter.");

        public static string fractureKeyword = KeywordText("Fracture", "Rift weakens the closer it is to Rifter. A fracture line connects between Rifter and the rift, dealing <style=cIsDamage>80% damage</style>. Fracture lines cannot crit or activate item effects.");

        public static string DamageText(string text)
        {
            return $"<style=cIsDamage>{text}</style>";
        }
        public static string DamageValueText(float value)
        {
            return $"<style=cIsDamage>{value * 100}% damage</style>";
        }
        public static string UtilityText(string text)
        {
            return $"<style=cIsUtility>{text}</style>";
        }
        public static string RedText(string text) => HealthText(text);
        public static string HealthText(string text)
        {
            return $"<style=cIsHealth>{text}</style>";
        }
        public static string KeywordText(string keyword, string sub)
        {
            return $"<style=cKeywordName>{keyword}</style><style=cSub>{sub}</style>";
        }
        public static string ScepterDescription(string desc)
        {
            return $"\n<color=#d299ff>SCEPTER: {desc}</color>";
        }

        public static string GetAchievementNameToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_NAME";
        }
        public static string GetAchievementDescriptionToken(string identifier)
        {
            return $"ACHIEVEMENT_{identifier.ToUpperInvariant()}_DESCRIPTION";
        }
    }
}