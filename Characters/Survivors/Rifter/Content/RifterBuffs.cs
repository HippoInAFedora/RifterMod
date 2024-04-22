using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterBuffs
    {
        // armor buff gained during roll
        public static BuffDef unstableDebuff;

        public static BuffDef teleporting;

        public static void Init(AssetBundle assetBundle)
        {

            unstableDebuff = Modules.Content.CreateAndAddBuff("Unstable",
                null,
                Color.white,
                true,
                true);
            unstableDebuff.isHidden = true;
        }
    }
}
