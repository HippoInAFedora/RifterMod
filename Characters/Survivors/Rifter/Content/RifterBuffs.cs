using RoR2;
using UnityEngine;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterBuffs
    {
        // armor buff gained during roll
        public static BuffDef riftTeleportableBuff;
        public static BuffDef riftSpecial;
        public static BuffDef fractureBuff;
        public static BuffDef shatterBuff;

        public static void Init(AssetBundle assetBundle)
        {
            riftTeleportableBuff = Modules.Content.CreateAndAddBuff("RiftTeleportableBuff",
                null,
                Color.white,
                true,
                false);
            riftTeleportableBuff.isHidden = true;

            riftSpecial = Modules.Content.CreateAndAddBuff("RiftSpecial",
                null,
                Color.white,
                false,
                false);
            riftTeleportableBuff.isHidden = true;

            fractureBuff = Modules.Content.CreateAndAddBuff("Fracture",
                null,
                Color.white,
                true,
                true);


            shatterBuff = Modules.Content.CreateAndAddBuff("Fracture",
                null,
                Color.white,
                false,
                true);


        }
    }
}
