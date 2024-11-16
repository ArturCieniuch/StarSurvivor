using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : PoolObject
{
    [SerializeField] private int damage;
    [SerializeField] private int maxHp;
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
    [SerializeField] private TrailRenderer trail;

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
        float time = refreshTargetTime;
        float timer = time;

        float waitTimer = waitTime;

        Vector3 oldDirection = Vector3.zero;
        Vector3 moveDirection = Vector3.zero;
        float trackingProgress = 0;

        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (true)
        {
            timer += Time.fixedDeltaTime;

            if (timer >= time)
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

            yield return wait;
        }
    }

    public void DealDamage(float damage)
    {
        hp -= damage;

        Debug.Log(hp);

        if (hp <= 0)
        {
            EnemyManager.Instance.SpawnPoint(transform.position);
            EnemyManager.Instance.RemoveEnemy(this);
        }
    }

    public void ResetTrail()
    {
        trail.Clear();
    }

    public override void OnTakenFromPool()
    {
        gameObject.SetActive(true);
        hp = maxHp;
        trail.transform.SetParent(transform);
        StartMovement();
    }

    public override void OnReturnToPool()
    {
        trail.transform.SetParent(null);
        enemyRigidbody.linearVelocity = Vector3.zero;
        enemyRigidbody.angularVelocity = Vector3.zero;
        StopCoroutine(movementCoroutine);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.DealDamage(damage);
            EnemyManager.Instance.RemoveEnemy(this);
        }
    }

    public void Reset()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
    }
}
