using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private LevelUpCard card;
    [SerializeField] private List<ShipSystem> weaponLevelUps;
    [SerializeField] private List<ShipSystem> systemLevelUps;
    [SerializeField] private List<UpgradeDataSO> upgradeLevelUps;
    [SerializeField] private Transform cardContainer;

    private List<LevelUpCard> cards = new List<LevelUpCard>(3);

    private void OnEnable()
    {
        List<ShipSystem> shipSystemCards = new List<ShipSystem>();
        List<UpgradeDataSO> updageCards = new List<UpgradeDataSO>();

        shipSystemCards.AddRange(GetLevelUps(weaponLevelUps, 1));
        shipSystemCards.AddRange(GetLevelUps(systemLevelUps, 1));
        updageCards.AddRange(GetLevelUps(upgradeLevelUps, 2));

        foreach (var levelUp in shipSystemCards)
        {
            LevelUpCard newCard = PoolController.Instance.GetObject<LevelUpCard>(card);
            newCard.transform.SetParent(cardContainer);
            newCard.transform.localScale = Vector3.one;
            newCard.SetUp(levelUp);
            newCard.OnShipSystemSelected.AddListener(OnLevelUpSelected);
            cards.Add(newCard);
        }

        foreach (var levelUp in updageCards)
        {
            LevelUpCard newCard = PoolController.Instance.GetObject<LevelUpCard>(card);
            newCard.transform.SetParent(cardContainer);
            newCard.transform.localScale = Vector3.one;
            newCard.SetUp(levelUp);
            newCard.OnUpgradeSelected.AddListener(OnUpgradeSelected);
            cards.Add(newCard);
        }
    }

    private void OnLevelUpSelected(ShipSystem data)
    {
        gameObject.SetActive(false);

        switch (data)
        {
            case Turret turretData:
                Player.Instance.turretSlotSelection.SetUp(turretData);
                break;
            case AuxiliarySystem upgradeData:
                break;
        }
    }

    private void OnUpgradeSelected(UpgradeDataSO upgrade)
    {
        gameObject.SetActive(false);

        Player.Instance.AddMod(upgrade.modType, upgrade.GetModValue());
        GameController.Instance.SetPause(false);
    }

    private List<ShipSystem> GetLevelUps(List<ShipSystem> levelUpList, int count)
    {
        int levelUpsPointsSum = 0;

        foreach (var levelUp in levelUpList)
        {
            levelUpsPointsSum += levelUp.rarity.GetRarityValue();
        }

        List<ShipSystem> levelUpsToReturn = new List<ShipSystem>(count);

        if (levelUpList.Count <= count)
        {
            return levelUpList;
        }

        while (levelUpsToReturn.Count != count)
        {
            int chance = GameController.Rand.Next(0, levelUpsPointsSum);

            int chanceCount = 0;

            foreach (var levelUp in levelUpList)
            {
                if (chance >= chanceCount && chance < chanceCount + levelUp.rarity.GetRarityValue())
                {
                    if (levelUpsToReturn.Contains(levelUp))
                    {
                        break;
                    }
                    levelUpsToReturn.Add(levelUp);
                    break;
                }

                chanceCount += levelUp.rarity.GetRarityValue();
            }
        }

        return levelUpsToReturn;
    }

    private List<UpgradeDataSO> GetLevelUps(List<UpgradeDataSO> levelUpList, int count)
    {
        int levelUpsPointsSum = 0;

        foreach (var levelUp in levelUpList)
        {
            levelUpsPointsSum += levelUp.rarity.GetRarityValue();
        }

        List<UpgradeDataSO> levelUpsToReturn = new List<UpgradeDataSO>(count);

        if (levelUpList.Count <= count)
        {
            return levelUpList;
        }

        while (levelUpsToReturn.Count != count)
        {
            int chance = GameController.Rand.Next(0, levelUpsPointsSum);

            int chanceCount = 0;

            foreach (var levelUp in levelUpList)
            {
                if (chance >= chanceCount && chance < chanceCount + levelUp.rarity.GetRarityValue())
                {
                    if (levelUpsToReturn.Contains(levelUp))
                    {
                        break;
                    }
                    levelUpsToReturn.Add(levelUp);
                    break;
                }

                chanceCount += levelUp.rarity.GetRarityValue();
            }
        }

        return levelUpsToReturn;
    }

    private void OnDisable()
    {
        foreach (var cardToRemove in cards)
        {
            PoolController.Instance.ReturnObject(cardToRemove);
        }

        cards.Clear();
    }
}
