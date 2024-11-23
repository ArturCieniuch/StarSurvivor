using System;
using UnityEngine;
using UnityEngine.Events;
using static Delegates;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private int maxHp;
    private int hp;
    private int points;

    public int pointRange;

    public OnMeterChange onHpChange;
    public OnMeterChange onExChange;
    public OnValueChange onLevelChange;

    private int pointToLevel = 5;
    private int level = 1;
    public CameraShake cameraShake;
    public ParticleController particlesOnPlayerDeath;
    public TurretSlotSelection turretSlotSelection;

    public static PlayerMods playerMods;

    void Awake()
    {
        playerMods = new PlayerMods();
        Instance = this;
        hp = maxHp;
        points = 0;
    }

    public void AddPoints(int pointValue)
    {
        points += pointValue;

        if (points >= pointToLevel)
        {
            points -= pointToLevel;
            level++;
            onLevelChange?.Invoke(level);
            pointToLevel += 10;

            if (level <= 7)
            {
                GameController.Instance.OnLevelUp();
            }
        }

        onExChange?.Invoke(points, pointToLevel);
    }

    public void DealDamage(int damage)
    {
        hp -= damage;
        onHpChange?.Invoke(hp, maxHp);

        cameraShake.ShakeCamera();

        if (hp <= 0)
        {
            ParticleController particle = PoolController.Instance.GetObject<ParticleController>(particlesOnPlayerDeath);
            particle.transform.position = transform.position;
            particle.Play(true);
            GameController.Instance.OnPlayerDeath();
            gameObject.SetActive(false);
        }
    }
}

[Serializable]
public class PlayerMods
{
    public float maxVelocityMod = 1;
    public float accelerationMod = 1;
    public float sideAccelerationMod = 1;
    public float maxSideAccelerationMod = 1;
    public float hpMod = 1;
    public float enemySpeedMod = 1;
    public float turretsFireRateMod = 1;
    public float turretsDamageMod = 1;
}
