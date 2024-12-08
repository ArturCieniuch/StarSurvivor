using UnityEngine;

public class MagnetPowerUp : Drop
{
    protected override void OnCollect()
    {
        Player.Instance.SetTempMod(ModType.PICK_UP_RANGE, 10000, 0.25f);
    }
}
