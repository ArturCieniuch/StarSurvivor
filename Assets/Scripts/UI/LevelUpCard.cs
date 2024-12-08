using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpCard : PoolObject
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button button;

    public UnityEvent<ShipSystem> OnShipSystemSelected = new UnityEvent<ShipSystem>();
    public UnityEvent<UpgradeDataSO> OnUpgradeSelected = new UnityEvent<UpgradeDataSO>();
    private ShipSystem shipSystem;
    private UpgradeDataSO upgradeData;

    private void Awake()
    {
        button.onClick.AddListener(OnButton);
    }

    public void SetUp(ShipSystem shipSystem)
    {
        this.shipSystem = shipSystem;
        icon.sprite = shipSystem.icon;
        name.text = shipSystem.name;
        name.color = shipSystem.rarity.GetRarityColor();
        description.text = (shipSystem as ILevelUpDescription).GetDescription();
    }

    public void SetUp(UpgradeDataSO upgradeData)
    {
        this.upgradeData = upgradeData;
        icon.sprite = upgradeData.icon;
        icon.color = upgradeData.rarity.GetRarityColor();

        name.color = upgradeData.rarity.GetRarityColor();
        name.text = upgradeData.name;
        description.text = (upgradeData as ILevelUpDescription).GetDescription();
    }

    private void OnButton()
    {
        if (shipSystem != null)
        {
            OnShipSystemSelected.Invoke(shipSystem);
            return;
        }

        if (upgradeData != null)
        {
            OnUpgradeSelected.Invoke(upgradeData);
            return;
        }
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
    }

    public override void OnReturnToPool()
    {
        OnShipSystemSelected.RemoveAllListeners();
        OnUpgradeSelected.RemoveAllListeners();
        shipSystem = null;
        upgradeData = null;
        gameObject.SetActive(false);
    }
}
