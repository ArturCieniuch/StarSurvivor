using UnityEngine;

public class SpeedPowerUp : Drop
{
    protected override void OnCollect()
    {
        Player.Instance.SetTempMod(ModType.MAX_VELOCITY, 2, 5);
        Player.Instance.SetTempMod(ModType.ACCELERATION, 2, 5);
        Player.Instance.SetTempMod(ModType.ROTATION, 2, 5);
    }
}
