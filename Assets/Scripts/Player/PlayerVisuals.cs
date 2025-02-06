using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hitPs;
    public void PlayHitParticles()
    {
        _hitPs.Play();
    }
}
