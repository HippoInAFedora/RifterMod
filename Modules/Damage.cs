using R2API;

public static class Damage
{
    internal static DamageAPI.ModdedDamageType riftDamage;

    internal static DamageAPI.ModdedDamageType riftAssistDamage;

    internal static void SetupModdedDamage()
    {
        riftDamage = DamageAPI.ReserveDamageType();

        riftAssistDamage = DamageAPI.ReserveDamageType();
    }
}