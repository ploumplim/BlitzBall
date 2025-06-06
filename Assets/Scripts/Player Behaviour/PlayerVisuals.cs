using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem hitParticleSystem;
    
    // References
    private PlayerScript playerScript;

    private void Start()
    {
        playerScript = GetComponent<PlayerScript>();
    }

    public void OnHitPerformed()
    {
        // Set the particle's shape emission's radius to be the same as the hit radius
        var shape = hitParticleSystem.shape;
        shape.radius = playerScript.hitRadius;
        // Set the particle's position to the player's position
        if (!hitParticleSystem.isPlaying)
        {
            hitParticleSystem.Play();
        }
    }
    
}
