using UnityEngine;

[CreateAssetMenu(fileName = "TurretDataSO", menuName = "Scriptable Objects/TurretDataSO")]
public class TurretDataSO : ScriptableObject
{
    public Turret prefab;
    public int chanceValue = 10;

    [Header("Display")]
    public Sprite turretIcon;

    [Header("Shooting")]
    public float rotationSpeed = 5;
    public float fireRate;
    public float damagePerSecond;
    public float range;
    public bool shotOnTargetOnly;
}
