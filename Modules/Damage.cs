using R2API;

public static class Damage
{
    internal static DamageAPI.ModdedDamageType riftDamage;

    internal static void SetupModdedDamage()
    {
        riftDamage = DamageAPI.ReserveDamageType();
    }
}