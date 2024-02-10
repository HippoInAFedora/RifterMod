using RifterMod.Survivors.Rifter.SkillStates;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(RefractedReality));

            Modules.Content.AddEntityState(typeof(RiftGauntlet));

            Modules.Content.AddEntityState(typeof(Slipstream));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
