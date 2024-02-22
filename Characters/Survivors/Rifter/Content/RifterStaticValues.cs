using System;

namespace RifterMod.Survivors.Rifter
{
    public static class RifterStaticValues
    {
        public const float swordDamageCoefficient = 2.8f;

        public const float gunDamageCoefficient = 4.0f;

        public const float bombDamageCoefficient = 16f;

        public const float riftPrimaryDistance = 50f;

        public const float riftSecondaryDistance = 25f;

        public enum AccesibleState
        {
            None = 0,
            Special
        }
    }
}