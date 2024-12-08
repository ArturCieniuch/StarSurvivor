using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Turret : ShipSystem
{
    [Header("Data")]
    [SerializeField] private List<TurretData> levels = new List<TurretData>(1);

    [Serializable]
    public class TurretData
    {
        public float rotationSpeed = 360;
        public float fireRate;
        public float damagePerSecond;
        public float range;
        public bool shotOnTargetOnly;
    }

    [Header("Components")]
    [SerializeField] private ShotController shotController;
    [SerializeField] private SphereCollider trigger;
    [SerializeField] private EnemyTracker enemyTracker;
    [SerializeField] private Transform angleTracker;


    private List<Enemy> enemies;
    private TurretSlot.TurretSlotData turretSlotData;

    private float timer;
    public int currentLevel = 0;

    public int MaxLevel => levels.Count;

    private void Start()
    {
        EnemyManager.Instance.enemyRemoved += OnEnemyRemoved;
        enemies = new List<Enemy>(EnemyManager.Instance.maxEnemies);
        shotController.SetUp(this);
        trigger.radius = levels[currentLevel].range;
        angleTracker.SetParent(Player.Instance.transform);
    }

    public void LevelUp()
    {
        currentLevel++;

        trigger.radius = levels[currentLevel].range;
    }

    public float GetDamage()
    {
        return (levels[currentLevel].damagePerSecond / levels[currentLevel].fireRate) * Player.GetMod(ModType.TURRETS_DAMAGE);
    }

    public float GetDamage(int level)
    {
        return (levels[level].damagePerSecond / levels[level].fireRate) * Player.GetMod(ModType.TURRETS_DAMAGE);
    }

    private void OnEnemyRemoved(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    public void SetTurretSlotData(TurretSlot.TurretSlotData data)
    {
        this.turretSlotData = data;
    }

    public override string GetDescription()
    {
        string description = $"Rotation speed: <color=\"green\"><b>{levels[0].rotationSpeed}\u00b0</b></color> per second\n" +
                      $"Fire Rate: <color=\"green\"><b>{levels[0].fireRate}</b></color> per second\n" +
                      $"Damage: <color=\"green\"><b>{GetDamage(0)}</b></color>\n" +
                      $"Range: <color=\"green\"><b>{levels[0].range}</b></color>";
        return description;
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
        if (Vector3.Distance(enemyTransform.position, transform.position) > levels[currentLevel].range)
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(GetTrackerDirection()), levels[currentLevel].rotationSpeed * Time.deltaTime);
            return;
        }

        Vector3 directionToEnemy = (closestEnemy.transform.position - transform.position).normalized;
        directionToEnemy.y = 0;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(directionToEnemy, Vector3.up), levels[currentLevel].rotationSpeed * Time.deltaTime);

        float realFireRate = 1f / (levels[currentLevel].fireRate * Player.GetMod(ModType.TURRETS_FIRE_RATE));

        if (timer >= realFireRate)
        {
            if (!levels[currentLevel].shotOnTargetOnly)
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
