using Redcode.Moroutines;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Delegates;
using static UnityEngine.Rendering.DebugUI;

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
    public static PlayerMods playerTempMods;
    private Dictionary<ModType, Coroutine> tempModsCoroutines = new Dictionary<ModType, Coroutine>();

    void Awake()
    {
        playerMods = new PlayerMods(1);
        playerTempMods = new PlayerMods(0);
        Instance = this;
        hp = maxHp;
        points = 0;
    }

    public bool HasFullHp() => hp == maxHp;

    public static float GetMod(ModType modType)
    {
        switch (modType)
        {
            case ModType.MAX_VELOCITY:
                return playerMods.maxVelocityMod + playerTempMods.maxVelocityMod;
            case ModType.ACCELERATION:
                return playerMods.accelerationMod + playerTempMods.accelerationMod;
            case ModType.ENEMY_SPEED:
                return playerMods.enemySpeedMod + playerTempMods.enemySpeedMod;
            case ModType.TURRETS_FIRE_RATE:
                return playerMods.turretsFireRateMod + playerTempMods.turretsFireRateMod;
            case ModType.TURRETS_DAMAGE:
                return playerMods.turretsDamageMod + playerTempMods.turretsDamageMod;
            case ModType.PICK_UP_RANGE:
                return playerMods.pickUpRange + playerTempMods.pickUpRange;
            case ModType.ROTATION:
                return playerMods.rotationMod + playerTempMods.rotationMod;
            default:
                throw new ArgumentOutOfRangeException(nameof(modType), modType, null);
        }
    }

    public void SetTempMod(ModType modType, float value, float time)
    {
        switch (modType)
        {
            case ModType.MAX_VELOCITY:
                playerTempMods.maxVelocityMod = value;
                break;
            case ModType.ACCELERATION:
                playerTempMods.accelerationMod = value;
                break;
            case ModType.ENEMY_SPEED:
                playerTempMods.enemySpeedMod = value;
                break;
            case ModType.TURRETS_FIRE_RATE:
                playerTempMods.turretsFireRateMod = value;
                break;
            case ModType.TURRETS_DAMAGE:
                playerTempMods.turretsDamageMod = value;
                break;
            case ModType.ROTATION:
                playerTempMods.rotationMod = value;
                break;
            case ModType.PICK_UP_RANGE:
                playerTempMods.pickUpRange = value;
                break;
        }

        if (value == 0)
        {
            return;
        }

        if (tempModsCoroutines.ContainsKey(modType))
        {
            StopCoroutine(tempModsCoroutines[modType]);
            tempModsCoroutines.Remove(modType);
        }

        tempModsCoroutines.Add(modType, StartCoroutine(TempModCoroutine(modType, time)));
    }

    private IEnumerator TempModCoroutine(ModType modType, float time)
    {
        yield return new WaitForSeconds(time);

        SetTempMod(modType, 0, 0);

        tempModsCoroutines.Remove(modType);
    }

    public void AddMod(ModType modType, float value)
    {
        switch (modType)
        {
            case ModType.MAX_VELOCITY:
                playerMods.maxVelocityMod += value;
                break;
            case ModType.ACCELERATION:
                playerMods.accelerationMod += value;
                break;
            case ModType.ENEMY_SPEED:
                playerMods.enemySpeedMod += value;
                break;
            case ModType.TURRETS_FIRE_RATE:
                playerMods.turretsFireRateMod += value;
                break;
            case ModType.TURRETS_DAMAGE:
                playerMods.turretsDamageMod += value;
                break;
            case ModType.ROTATION:
                playerMods.rotationMod += value;
                break;
            case ModType.PICK_UP_RANGE:
                playerMods.pickUpRange += value;
                break;
        }
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

            GameController.Instance.OnLevelUp();
        }

        onExChange?.Invoke(points, pointToLevel);
    }

    public void Heal(int healAmount)
    {
        hp += healAmount;
        if (hp > maxHp)
        {
            hp = maxHp;
        }

        onHpChange?.Invoke(hp, maxHp);
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
    public float maxVelocityMod;
    public float accelerationMod;
    public float rotationMod;
    public float enemySpeedMod;
    public float turretsFireRateMod;
    public float turretsDamageMod;
    public float pickUpRange;

    public PlayerMods(float startValue)
    {
        maxVelocityMod = startValue;
        accelerationMod = startValue;
        enemySpeedMod = startValue;
        turretsFireRateMod = startValue;
        turretsDamageMod = startValue;
        pickUpRange = startValue;
        rotationMod = startValue;
    }
}

public enum ModType
{
    MAX_VELOCITY,
    ACCELERATION,
    ENEMY_SPEED,
    TURRETS_FIRE_RATE,
    TURRETS_DAMAGE,
    PICK_UP_RANGE,
    ROTATION
}
