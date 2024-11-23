using System.Collections.Generic;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    [SerializeField] private LevelUpCard card;
    [SerializeField] private LevelUpsSO levelUps;
    [SerializeField] private Transform cardContainer;

    private List<LevelUpCard> cards = new List<LevelUpCard>(2);

    private void OnEnable()
    {
        List<TurretDataSO> levelUpsCard = levelUps.GetLevelUps(2);

        foreach (var levelUp in levelUpsCard)
        {
            LevelUpCard newCard = PoolController.Instance.GetObject<LevelUpCard>(card);
            newCard.transform.SetParent(cardContainer);
            newCard.transform.localScale = Vector3.one;
            newCard.SetUp(levelUp);
            newCard.OnCardSelected.AddListener(OnLevelUpSelected);
            cards.Add(newCard);
        }
    }

    private void OnLevelUpSelected(TurretDataSO data)
    {
        gameObject.SetActive(false);
        Player.Instance.turretSlotSelection.SetUp(data);
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
