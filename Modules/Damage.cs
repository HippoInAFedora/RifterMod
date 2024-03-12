using R2API;

public static class Damage
{
    internal static DamageAPI.ModdedDamageType overchargedDamageType;

    internal static void SetupModdedDamage()
    {
        overchargedDamageType = DamageAPI.ReserveDamageType();
    }
}