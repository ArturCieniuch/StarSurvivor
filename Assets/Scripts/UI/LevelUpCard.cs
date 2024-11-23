using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpCard : PoolObject
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Button button;

    public UnityEvent<TurretDataSO> OnCardSelected = new UnityEvent<TurretDataSO>();
    private TurretDataSO turretData;

    private void Awake()
    {
        button.onClick.AddListener(OnButton);
    }

    public void SetUp(TurretDataSO turretData)
    {
        this.turretData = turretData;
        icon.sprite = turretData.turretIcon;
        name.text = turretData.name;
    }

    private void OnButton()
    {
        OnCardSelected.Invoke(turretData);
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
    }

    public override void OnReturnToPool()
    {
        OnCardSelected.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
