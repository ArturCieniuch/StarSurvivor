using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static Player Instance;

    [SerializeField] private int maxHp;
    private int hp;
    private int points;

    public int pointRange;

    public UnityEvent<int, int> onHpChange;
    public UnityEvent<int> onPointsChange;

    void Awake()
    {
        onHpChange = new UnityEvent<int, int>();
        onPointsChange = new UnityEvent<int>();

        Instance = this;
        hp = maxHp;
        points = 0;
    }

    public void AddPoints(int pointValue)
    {
        points += pointValue;
        onPointsChange.Invoke(points);
    }

    public void DealDamage(int damage)
    {
        hp -= damage;
        onHpChange.Invoke(hp, maxHp);
    }
}
