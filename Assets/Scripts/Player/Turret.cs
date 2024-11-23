using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private List<Enemy> enemies;

    [SerializeField] private TurretDataSO turretData;

    private TurretSlot.TurretSlotData turretSlotData;

    [Header("Components")]
    [SerializeField] private ShotController shotController;
    [SerializeField] private SphereCollider trigger;
    [SerializeField] private EnemyTracker enemyTracker;
    [SerializeField] private Transform angleTracker;

    private float timer;

    public TurretDataSO GetTurretData => turretData;

    private void Start()
    {
        EnemyManager.Instance.enemyRemoved += OnEnemyRemoved;
        enemies = new List<Enemy>(EnemyManager.Instance.maxEnemies);
        shotController.SetUp(this);
        trigger.radius = turretData.range;
        angleTracker.SetParent(Player.Instance.transform);
    }

    public float GetDamage()
    {
        return (turretData.damagePerSecond / turretData.fireRate) * Player.playerMods.turretsDamageMod;
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void SetTurretSlotData(TurretSlot.TurretSlotData data)
    {
        this.turretSlotData = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.attachedRigidbody.CompareTag("Enemy"))
        {
            return;
        }

        Enemy enemy = other.attachedRigidbody.GetComponent<Enemy>();
        enemies.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.attachedRigidbody.CompareTag("Enemy"))
        {
            return;
        }

        Enemy enemy = other.attachedRigidbody.GetComponent<Enemy>();
        enemies.Remove(enemy);
    }

    private Enemy FindClosestEnemy()
    {
        if (enemies.Count == 0)
        {
            return null;
        }

        Enemy closestEnemy = null;

        foreach (Enemy enemy in enemies)
        {
            if (!CheckAngleAndDistance(enemy.transform))
            {
                continue;
            }

            if (closestEnemy == null)
            {
                closestEnemy = enemy;
                continue;
            }

            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);

            if (distanceToEnemy < Vector3.Distance(closestEnemy.transform.position, transform.position))
            {
                closestEnemy = enemy;
            }
        }

        if (closestEnemy == null || !CheckAngleAndDistance(closestEnemy.transform))
        {
            return null;
        }

        return closestEnemy;
    }

    private bool CheckAngleAndDistance(Transform enemyTransform)
    {
        if (Vector3.Distance(enemyTransform.position, transform.position) > turretData.range)
        {
            return false;
        }

        Vector3 directionToEnemy = (enemyTransform.position - transform.position).normalized;
        directionToEnemy.y = 0;

        Vector3 directionToTracker = (angleTracker.position - transform.position).normalized;

        float angle = Vector3.SignedAngle(directionToTracker, directionToEnemy, Vector3.up);

        if (angle > 0)
        {
            return angle >= turretSlotData.minRightAngle && angle <= turretSlotData.maxRightAngle;
        } 
        else
        {
            return angle <= -turretSlotData.minLeftAngle && angle >= -turretSlotData.maxLeftAngle;

        }
    }

    private Vector3 GetTrackerDirection()
    {
        return (angleTracker.position - transform.position).normalized;
    }

    void Update()
    {
        Enemy closestEnemy = FindClosestEnemy();

        timer += Time.deltaTime;

        enemyTracker.SetTarget(closestEnemy);

        if (closestEnemy == null)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetTrackerDirection()), turretData.rotationSpeed * Time.deltaTime);
            return;
        }

        Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
        directionToEnemy.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up), turretData.rotationSpeed * Time.deltaTime);

        float realFireRate = 1f / (turretData.fireRate * Player.playerMods.turretsFireRateMod);

        if (timer >= realFireRate)
        {
            if (!turretData.shotOnTargetOnly)
            {
                timer = 0;
                shotController.Shot();
            }
            else if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up)) == 0)
            {
                timer = 0;
                shotController.Shot();
            }
        }
    }
}
