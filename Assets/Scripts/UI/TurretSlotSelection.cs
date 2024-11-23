using System.Collections.Generic;
using UnityEngine;

public class TurretSlotSelection : MonoBehaviour
{
    [SerializeField]List<TurretSlotUI> turretSlots = new List<TurretSlotUI>();

    private TurretDataSO turretData;

    private void Start()
    {
        foreach (var slot in turretSlots)
        {
            slot.OnSlotSelected.AddListener(OnSlotSelected);
        }
    }

    private void OnSlotSelected(TurretSlot slot)
    {
        slot.SetUpTurret(turretData);
        GameController.Instance.SetPause(false);
        gameObject.SetActive(false);
    }

    public void SetUp(TurretDataSO turretData)
    {
        gameObject.SetActive(true);
        this.turretData = turretData;
    }
}
