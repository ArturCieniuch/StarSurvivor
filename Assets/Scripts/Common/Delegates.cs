using UnityEngine;

public static class Delegates
{
    public delegate void OnValueChange(int x);
    public delegate void OnMeterChange(int current, int maxValue);
    public delegate void OnEnemy(Enemy enemy);
    public delegate void OnGameObject(GameObject gameObject);
}
