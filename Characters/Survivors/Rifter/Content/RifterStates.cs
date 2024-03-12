using RifterMod.Characters.Survivors.Rifter.SkillStates;
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
        }
    }
}
