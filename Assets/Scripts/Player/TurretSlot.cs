using System;
using UnityEngine;

public class TurretSlot : MonoBehaviour
{
    [Serializable]
    public struct TurretSlotData
    {
        public int range;
        public int minRightAngle;
        public int maxRightAngle;
        public int minLeftAngle;
        public int maxLeftAngle;
    }

    [SerializeField] private Turret turret;
    [SerializeField] private TurretSlotData turretSlotData;
    [SerializeField] private Vector3 turretOffset;
    [SerializeField] private bool showDebug;

    public TurretSlotData GetSlotData => turretSlotData;
    public Turret GetTurret => turret;

    public void SetUpTurret(Turret turret)
    {
        if (this.turret != null)
        {
            this.turret.LevelUp();
            return;
        }

        this.turret = Instantiate(turret, transform);
        this.turret.name = turret.name;
        this.turret.transform.localPosition = turretOffset;
        this.turret.SetTurretSlotData(turretSlotData);
    }

    void Start()
    {
        if (turret)
        {
            turret.SetTurretSlotData(turretSlotData);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (showDebug)
        {
            if (turretSlotData is { maxRightAngle: 0, minRightAngle: 0 } == false)
            {
                Debug.DrawArc(-turretSlotData.maxRightAngle, -turretSlotData.minRightAngle, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), turretSlotData.range, Color.blue, false, true);
            }

            if (turretSlotData is { maxLeftAngle: 0, minLeftAngle: 0 } == false)
            {
                Debug.DrawArc(turretSlotData.minLeftAngle, turretSlotData.maxLeftAngle, transform.position, Quaternion.LookRotation(transform.forward, Vector3.up), turretSlotData.range, Color.blue, false, true);
            }
        }
    }
}
