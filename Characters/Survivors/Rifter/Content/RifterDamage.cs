using R2API;

public static class RifterDamage
{
    internal static DamageAPI.ModdedDamageType riftDamage;
    internal static DamageAPI.ModdedDamageType riftSuperDamage;

    internal static void SetupModdedDamage()
    {
        riftDamage = DamageAPI.ReserveDamageType();
        riftSuperDamage = DamageAPI.ReserveDamageType();
    }
}