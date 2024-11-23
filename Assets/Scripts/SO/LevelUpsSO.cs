using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUpsSO", menuName = "Scriptable Objects/LevelUpsSO")]
public class LevelUpsSO : ScriptableObject
{
    public List<TurretDataSO> levelUps = new List<TurretDataSO>();

    private int levelUpsPointsSum;

    public void Init()
    {
        levelUpsPointsSum = 0;

        foreach (var levelUp in levelUps)
        {
            levelUpsPointsSum += levelUp.chanceValue;
        }
    }

    public List<TurretDataSO> GetLevelUps(int count)
    {
        List<TurretDataSO> levelUpsToReturn = new List<TurretDataSO>(count);

        if (levelUps.Count <= count)
        {
            return levelUps;
        }

        while (levelUpsToReturn.Count != count)
        {
            int chance = Random.Range(0, levelUpsPointsSum);

            int chanceCount = 0;

            foreach (var levelUp in levelUps)
            {
                if (chance >= chanceCount && chance < chanceCount + levelUp.chanceValue)
                {
                    if (levelUpsToReturn.Contains(levelUp))
                    {
                        break;
                    }
                    levelUpsToReturn.Add(levelUp);
                    break;
                }

                chanceCount += levelUp.chanceValue;
            }
        }

        return levelUpsToReturn;
    }
}
