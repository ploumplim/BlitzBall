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
        // Hit particle init
        // Set the particle's shape emission's radius to be the same as the hit radius
        var shape = hitParticleSystem.shape;
        shape.radius = playerScript.hitRadius;
        
        // Set the particle's duration to be the same as the hit duration
        var main = hitParticleSystem.main;
        main.duration = playerScript.hitDuration;
        
        
    }

    private void Update()
    {
        // Hit Particle position
        hitParticleSystem.transform.position = transform.position;
    }

    public void OnHitPerformed()
    {
        // Create an instance of the hit particle system at the player's position
        
        GameObject instance = Instantiate(hitParticleSystem.gameObject, transform.position, Quaternion.identity);
        instance.transform.SetParent(transform);
        ParticleSystem instanceParticleSystem = instance.GetComponent<ParticleSystem>();
        instanceParticleSystem.Play();
        // Destroy the instance after the particle system has finished playing
        Destroy(instance, instanceParticleSystem.main.duration);

    }
    
}
