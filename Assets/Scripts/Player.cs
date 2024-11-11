using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public int hp;
    public int maxHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        hp = maxHp;
    }

    public void DealDamage(int damage)
    {
        hp -= damage;
    }
}
