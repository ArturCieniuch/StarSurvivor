using System.Collections;
using System.Threading;
using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;
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
    public UnityEvent<int, int> onExChange;
    public UnityEvent<int> onLevelChange;

    private int pointToLevel = 5;
    private int level = 1;
    public CameraShake cameraShake;
    public ParticleController particlesOnPlayerDeath;


    void Awake()
    {
        onHpChange = new UnityEvent<int, int>();
        onExChange = new UnityEvent<int, int>();
        onLevelChange = new UnityEvent<int>();
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
            onLevelChange.Invoke(level);
            pointToLevel += 10;
        }

        onExChange.Invoke(points, pointToLevel);
    }

    public void DealDamage(int damage)
    {
        hp -= damage;
        onHpChange.Invoke(hp, maxHp);

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
