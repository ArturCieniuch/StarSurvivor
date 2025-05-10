using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeDataSO", menuName = "Scriptable Objects/UpgradeDataSO")]
public class UpgradeDataSO : ScriptableObject, ILevelUpDescription
{
    [Header("Level up data")]
    public Sprite icon;
    public Rarity rarity = Rarity.COMMON;
    public ModType modType;

    public float GetModValue()
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return 0.05f;
            case Rarity.UNCOMMON:
                return 0.1f;
            case Rarity.RARE:
                return 0.15f;
            case Rarity.LEGENDARY:
                return 0.25f;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string GetDescription()
    {
        return $"{modType.ToString()}: <color=\"green\"><b>+{GetModValue() * 100}%</b></color>";
    }
}

