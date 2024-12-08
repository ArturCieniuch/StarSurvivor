using System;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public static class Extensions
{
    public static int GetRarityValue(this Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return 10;
            case Rarity.UNCOMMON:
                return 6;
            case Rarity.RARE:
                return 3;
            case Rarity.LEGENDARY:
                return 1;
            default:
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
        }

    }

    public static Color GetRarityColor(this Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return Color.gray;
            case Rarity.UNCOMMON:
                return Color.green;
            case Rarity.RARE:
                return Color.blue;
            case Rarity.LEGENDARY:
                return Color.yellow;
            default:
                throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
        }
    }
}
