using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterBuffs
    {
        // armor buff gained during roll
        public static BuffDef shatterDebuff;
        public static BuffDef superShatterDebuff;

        public static BuffDef postTeleport;

        public static AssetBundle _assetBundle;

        public static Sprite shatterIcon = RifterAssets.shatterIcon;

        public static void Init(AssetBundle assetBundle)
        {

            shatterDebuff = Modules.Content.CreateAndAddBuff("Shatter",
                shatterIcon,
                Color.white,
                true,
                true);
            shatterDebuff.isHidden = false;

            superShatterDebuff = Modules.Content.CreateAndAddBuff("SuperShatter",
                shatterIcon,
                Color.white,
                false,
                false);
            superShatterDebuff.isHidden = true;

            postTeleport = Modules.Content.CreateAndAddBuff("PostTeleport",
               null,
               Color.white,
               true,
               false);
            postTeleport.isHidden = true;
        }
    }
}
