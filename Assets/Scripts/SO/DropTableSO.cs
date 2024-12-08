using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(fileName = "DropTableSO", menuName = "Scriptable Objects/DropTableSO")]
public class DropTableSO : ScriptableObject
{
    public List<DropData> dropList;

    public DropData GetDrop()
    {
        int probabilitySum = 0;

        foreach (var drop in dropList)
        {
            probabilitySum += drop.probability;
        }

        int chance = GameController.Rand.Next(0, probabilitySum);

        int chanceCount = 0;

        foreach (var drop in dropList)
        {
            if (chance >= chanceCount && chance < chanceCount + drop.probability)
            {
                return drop;
            }

            chanceCount += drop.probability;
        }

        return null;
    }
}

public enum DropType
{
    EMPTY,
    POINT,
    BIG_POINT,
    HYPER_POINT,
    HEAL,
    WIPE,
    SPEED_BOOST,
    WEAPON_BOOST,
    SUPER_SHIELD,
    MAGNET,
}

[Serializable]
public class DropData
{
    public int probability;
    public DropType dropType = DropType.EMPTY;
}
