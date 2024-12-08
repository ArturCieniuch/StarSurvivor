using UnityEngine;

public class WeaponPowerUp : Drop
{
    protected override void OnCollect()
    {
        Player.Instance.SetTempMod(ModType.TURRETS_FIRE_RATE, 2, 5);
        Player.Instance.SetTempMod(ModType.TURRETS_DAMAGE, 2, 5);
    }
}
