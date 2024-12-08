using System.Collections.Generic;
using UnityEngine;

public class TurretSlotSelection : MonoBehaviour
{
    [SerializeField]List<TurretSlotUI> turretSlots = new List<TurretSlotUI>();

    private Turret turret;

    private void Start()
    {
        foreach (var slot in turretSlots)
        {
            slot.OnSlotSelected.AddListener(OnSlotSelected);
        }
    }

    public void Skip()
    {
        GameController.Instance.SetPause(false);
        gameObject.SetActive(false);
    }

    private void OnSlotSelected(TurretSlot slot)
    {
        slot.SetUpTurret(turret);
        GameController.Instance.SetPause(false);
        gameObject.SetActive(false);
    }

    public void SetUp(Turret turret)
    {
        gameObject.SetActive(true);
        this.turret = turret;

        foreach (var slot in turretSlots)
        {
            slot.Init(turret);
        }
    }
}
