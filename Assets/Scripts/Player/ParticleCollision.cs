using UnityEngine;
using UnityEngine.Events;
using static Delegates;

public class ParticleCollision : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    public OnGameObject OnHit;

    public void Shot()
    {
        particleSystem.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        OnHit?.Invoke(other);
    }
}
