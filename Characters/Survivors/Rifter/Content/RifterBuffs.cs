using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterBuffs
    {
        // armor buff gained during roll
        public static BuffDef shatterDebuff;

        public static AssetBundle _assetBundle;

        public static Sprite shatterIcon = RifterAssets.shatterIcon;

        public static void Init(AssetBundle assetBundle)
        {

            shatterDebuff = Modules.Content.CreateAndAddBuff("Shatter",
                shatterIcon,
                Color.white,
                true,
                false);
            shatterDebuff.isHidden = true;
        }
    }
}
