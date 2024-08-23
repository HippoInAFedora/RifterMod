using BepInEx.Configuration;
using RifterMod.Modules;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterConfig
    {

        public static ConfigEntry<bool> distanceAssist;

        public static ConfigEntry<bool> teleportYourFriends;

        public static ConfigEntry<bool> cursed;

        public static ConfigEntry<bool> HUD;


        public static void Init()
        {
            string section = "Rifter";

            distanceAssist = Config.BindAndOptions(section, "Distance Assist", defaultValue: true, "Creates a constant beam to show your primary rift distance.");

            teleportYourFriends = Config.BindAndOptions(section, "Teleport Your Friends", defaultValue: false, "Teleport your firends!");

            //cursed = Config.BindAndOptions(section, "Cursed", defaultValue: false, "Adds sillies, such as blind pests losing the ability to fly once teleported.");

            HUD = Config.BindAndOptions(section, "Enable HUD", defaultValue: true, "Adds HUD for overcharge count.");
        }
    }
}
