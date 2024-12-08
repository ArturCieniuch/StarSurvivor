using UnityEngine;

public class ShipSystem : MonoBehaviour, ILevelUpDescription
{
    [Header("Level up data")]
    public Sprite icon;
    public Rarity rarity = Rarity.COMMON;
    public virtual string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
