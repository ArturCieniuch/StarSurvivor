using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : PoolObject
{
    [SerializeField] private int damage;
    [SerializeField] private int maxHp;
    [Range(0,1)]
    [SerializeField] private float pointDropChance;
    private float hp;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;
    [SerializeField] private float refreshTargetTime;
    [SerializeField]
    [Range(0, 180)]
    private int rotationDegrees;
    [SerializeField]
    [Range(0f, 5f)]
    private float trackingSpeed;

    [Header("Componets")]
    [SerializeField] private Rigidbody enemyRigidbody;
    [SerializeField] private ParticleController particleOnDeath;

    private Coroutine movementCoroutine;

    public void StartMovement()
    {
        movementCoroutine = StartCoroutine(Movement());
    }

    private Vector3 GetPlayerPosition()
    {
        return Player.Instance.transform.position;
    }

    private Vector3 GetPlayerDirection()
    {
        return (GetPlayerPosition() - transform.position).normalized;
    }

    private IEnumerator Movement()
    {
        float timer = refreshTargetTime;

        float waitTimer = waitTime;

        Vector3 oldDirection = Vector3.zero;
        Vector3 moveDirection = Vector3.zero;
        float trackingProgress = 0;

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= refreshTargetTime)
            {
                if (waitTimer < waitTime)
                {
                    enemyRigidbody.linearVelocity = Vector2.zero;

                    waitTimer += Time.fixedDeltaTime;

                    yield return wait;
                    continue;
                }

                oldDirection = enemyRigidbody.linearVelocity.normalized * speed;
                moveDirection = Quaternion.AngleAxis(Random.Range(-rotationDegrees, rotationDegrees), Vector3.forward) * GetPlayerDirection() * speed;
                timer = 0;
                waitTimer = 0;
                trackingProgress = 0;
            }

            if (trackingSpeed > 0)
            {
                trackingProgress += trackingSpeed * Time.fixedDeltaTime;

                trackingProgress = Mathf.Clamp01(trackingProgress);

                enemyRigidbody.linearVelocity = (Vector3.Lerp(oldDirection, moveDirection, trackingProgress));
            }
            else
            {
                enemyRigidbody.linearVelocity = moveDirection;
            }

            transform.rotation = Quaternion.LookRotation(enemyRigidbody.linearVelocity.normalized, Vector3.up);

            yield return wait;
        }
    }

    public void DealDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            ParticleController particleController = PoolController.Instance.GetObject<ParticleController>(particleOnDeath);
            particleController.transform.position = transform.position;
            particleController.Play(true);

            if (Random.Range(0f, 1f) < pointDropChance)
            {
                EnemyManager.Instance.SpawnPoint(transform.position);
            }
            EnemyManager.Instance.RemoveEnemy(this);
        }
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
        hp = maxHp;
    }

    public override void OnReturnToPool()
    {
        enemyRigidbody.linearVelocity = Vector3.zero;
        enemyRigidbody.angularVelocity = Vector3.zero;
        StopCoroutine(movementCoroutine);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ParticleController particleController = PoolController.Instance.GetObject<ParticleController>(particleOnDeath);
            particleController.transform.position = transform.position;
            particleController.Play(true);

            Player.Instance.DealDamage(damage);
            EnemyManager.Instance.RemoveEnemy(this);
        }
    }

    public void Reset()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }
}
