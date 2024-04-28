using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterBuffs
    {
        // armor buff gained during roll
        public static BuffDef shatterDebuff;

        public static void Init(AssetBundle assetBundle)
        {

            shatterDebuff = Modules.Content.CreateAndAddBuff("Shatter",
                null,
                Color.white,
                true,
                false);
            shatterDebuff.isHidden = false;
        }
    }
}
