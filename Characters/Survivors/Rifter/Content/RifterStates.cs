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

            Modules.Content.AddEntityState(typeof(RiftFocus));

            Modules.Content.AddEntityState(typeof(RiftGauntletShort));

            Modules.Content.AddEntityState(typeof(RiftBuckshot));

            Modules.Content.AddEntityState(typeof(SlipstreamIn));
            Modules.Content.AddEntityState(typeof(Slipstream));

            Modules.Content.AddEntityState(typeof(PortalBaseState));

            Modules.Content.AddEntityState(typeof(PortalMainDrop));

            Modules.Content.AddEntityState(typeof(PortalAuxLocate));

            Modules.Content.AddEntityState(typeof(PortalAuxDrop));

            Modules.Content.AddEntityState(typeof(ModifiedTeleport));

            Modules.Content.AddEntityState(typeof(FractureShot));

            Modules.Content.AddEntityState(typeof(FaultLine));

            Modules.Content.AddEntityState(typeof(ChainedWorlds));

            Modules.Content.AddEntityState(typeof(ChainedWorldsStartup));

            Modules.Content.AddEntityState(typeof(TimelockLocate));

            Modules.Content.AddEntityState(typeof(TimelockDrop));

            Modules.Content.AddEntityState(typeof(RifterMain));
        }
    }
}
