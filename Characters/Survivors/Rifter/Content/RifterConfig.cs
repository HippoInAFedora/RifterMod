using BepInEx.Configuration;
using RifterMod.Modules;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterConfig
    {
        public static ConfigEntry<bool> teleportYourFriends;

        public static ConfigEntry<bool> cursed;


        public static void Init()
        {
            string section = "Rifter";

            teleportYourFriends = Config.BindAndOptions(section, "Teleport Your Friends", defaultValue: false, "Teleport your firends!");

            cursed = Config.BindAndOptions(section, "Cursed", defaultValue: false, "Adds sillies, such as blind pests losing the ability to fly once teleported. [Restart Required]");
        }
    }
}

