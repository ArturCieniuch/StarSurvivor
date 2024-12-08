using UnityEngine;

public class HealPowerUp : Drop
{
    [SerializeField]
    private int hpToHeal;

    protected override bool CanBePickUp()
    {
        return !Player.Instance.HasFullHp();
    }

    protected override void OnCollect()
    {
        base.OnCollect();
        Player.Instance.Heal(hpToHeal);
    }
}
