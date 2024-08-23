using RifterMod.Characters.Survivors.Rifter.SkillStates;
using RifterMod.Characters.Survivors.Rifter.SkillStates.UnusedStates;
using RifterMod.Survivors.Rifter.SkillStates;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(RiftBase));

            Modules.Content.AddEntityState(typeof(Slipstream));

            Modules.Content.AddEntityState(typeof(ModifiedTeleport));

            Modules.Content.AddEntityState(typeof(RiftRiderLocate));

            Modules.Content.AddEntityState(typeof(AirBourneOut));

            Modules.Content.AddEntityState(typeof(RiftGauntletBase));

            Modules.Content.AddEntityState(typeof(RiftGauntletShort));

            Modules.Content.AddEntityState(typeof(RiftRider));

            Modules.Content.AddEntityState(typeof(RiftRiderOut));

            Modules.Content.AddEntityState(typeof(Refraction));

            Modules.Content.AddEntityState(typeof(RapidfireGauntlet));

            Modules.Content.AddEntityState(typeof(RecursionChargeup));

            Modules.Content.AddEntityState(typeof(Recursion));

            Modules.Content.AddEntityState(typeof(ChainedWorldsChargeup));

            Modules.Content.AddEntityState(typeof(ChainedWorlds));

            Modules.Content.AddEntityState(typeof(RifterMain));

            Modules.Content.AddEntityState(typeof(RecursionLocateScepter));

            Modules.Content.AddEntityState(typeof(ChainedWorldsChargeupScepter));
        }
    }
}
