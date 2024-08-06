using R2API;

public static class RifterDamage
{
    internal static DamageAPI.ModdedDamageType riftDamage;

    internal static void SetupModdedDamage()
    {
        riftDamage = DamageAPI.ReserveDamageType();
    }
}