using System.Collections;
using UnityEngine;

public class Point : Drop
{
    [SerializeField]
    private int pointValue;

    protected override float GetPickUpRange()
    {
        Debug.Log(base.GetPickUpRange() * Player.GetMod(ModType.PICK_UP_RANGE));
        return base.GetPickUpRange() * Player.GetMod(ModType.PICK_UP_RANGE);
    }

    protected override void OnCollect()
    {
        Player.Instance.AddPoints(pointValue);
    }
}
