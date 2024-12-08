using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugMods : MonoBehaviour
{
    public Slider maxVelocityMod;
    public Slider accelerationMod;
    public Slider enemySpeedMod;
    public Slider turretsFireRateMod;
    public Slider turretsDamageMod;

    private void Start()
    {
        maxVelocityMod.onValueChanged.AddListener((value)=> Player.playerMods.maxVelocityMod = value);
        accelerationMod.onValueChanged.AddListener((value) => Player.playerMods.accelerationMod = value);
        enemySpeedMod.onValueChanged.AddListener((value) => Player.playerMods.enemySpeedMod = value);
        turretsFireRateMod.onValueChanged.AddListener((value) => Player.playerMods.turretsFireRateMod = value);
        turretsDamageMod.onValueChanged.AddListener((value) => Player.playerMods.turretsDamageMod = value);
    }
}
