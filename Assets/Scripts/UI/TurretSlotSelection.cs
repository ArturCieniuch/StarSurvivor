using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretSlotSelection : MonoBehaviour
{
    [SerializeField] List<TurretSlotUI> turretSlots = new List<TurretSlotUI>();
    [SerializeField] private LevelUpMenu menu;

    private Turret turret;

    private void Start()
    {
        foreach (var slot in turretSlots)
        {
            slot.OnSlotSelected.AddListener(OnSlotSelected);
        }
    }

    private void OnEnable()
    {
        SetUp(null);
    }

    public void Skip()
    {
        GameController.Instance.SetPause(false);
        gameObject.SetActive(false);
    }

    private void OnSlotSelected(TurretSlot slot)
    {
        if (!turret)
        {
            return;
        }
        slot.SetUpTurret(turret);
        GameController.Instance.SetPause(false);
        gameObject.SetActive(false);
    }

    public void SetUp(Turret turret)
    {
        this.turret = turret;

        foreach (var slot in turretSlots)
        {
            slot.Init(turret);
        }
    }
}
