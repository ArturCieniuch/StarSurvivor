using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TurretSlot connectedTurretSlot;

    [SerializeField] private Image turretIcon;
    [SerializeField] private Button button;
    [SerializeField] private Image rightArcImage;
    [SerializeField] private Image leftArcImage;
    [SerializeField] private TextMeshProUGUI levelText;

    public UnityEvent<TurretSlot> OnSlotSelected = new UnityEvent<TurretSlot>();

    private void Start()
    {
        rightArcImage.fillAmount = (connectedTurretSlot.GetSlotData.maxRightAngle - connectedTurretSlot.GetSlotData.minRightAngle) / 360f;
        leftArcImage.fillAmount = (connectedTurretSlot.GetSlotData.maxLeftAngle - connectedTurretSlot.GetSlotData.minLeftAngle) / 360f;

        rightArcImage.rectTransform.rotation = Quaternion.Euler(0, 0, -connectedTurretSlot.GetSlotData.minRightAngle);
        leftArcImage.rectTransform.rotation = Quaternion.Euler(0, 0, connectedTurretSlot.GetSlotData.minLeftAngle);

        button.onClick.AddListener(OnButton);
    }

    private void OnButton()
    {
        OnSlotSelected.Invoke(connectedTurretSlot);
        rightArcImage.enabled = false;
        leftArcImage.enabled = false;
    }

    public void Init(Turret currentTurret)
    {
        button.targetGraphic.color = Color.white;
        button.interactable = true;

        if (connectedTurretSlot.GetTurret == null)
        {
            levelText.text = "";
            turretIcon.sprite = null;
            return;
        }

        levelText.text = $"{connectedTurretSlot.GetTurret.currentLevel + 1}/{connectedTurretSlot.GetTurret.MaxLevel}";

        if (connectedTurretSlot.GetTurret.name == currentTurret.name && connectedTurretSlot.GetTurret.currentLevel < connectedTurretSlot.GetTurret.MaxLevel-1)
        {
            button.targetGraphic.color = Color.green;
            turretIcon.enabled = true;
            turretIcon.sprite = connectedTurretSlot.GetTurret.icon;
            return;
        }

        button.interactable = false;
        turretIcon.enabled = true;
        turretIcon.sprite = connectedTurretSlot.GetTurret.icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        rightArcImage.enabled = true;
        leftArcImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rightArcImage.enabled = false;
        leftArcImage.enabled = false;
    }
}
