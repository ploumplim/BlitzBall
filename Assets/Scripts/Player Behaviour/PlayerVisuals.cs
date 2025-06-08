using System;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem hitParticleSystem;
    [SerializeField] private ParticleSystem sprintStartParticleSystem;
    private float sprintStartParticleSize;
    
    [Header("Trail Settings")]
    [SerializeField] private TrailRenderer sprintTrailRenderer;
    
    // References
    private PlayerScript playerScript;
    private PlayerSM playerSM;

    private void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        playerSM = GetComponent<PlayerSM>();
        // Hit particle init
        // Set the particle's shape emission's radius to be the same as the hit radius
        var shape = hitParticleSystem.shape;
        shape.radius = playerScript.hitRadius;
        
        // Set the particle's duration to be the same as the hit duration
        var main = hitParticleSystem.main;
        main.duration = playerScript.hitDuration;
        
        // Store the current start size of the sprint start particle system
        var sprintStartMain = sprintStartParticleSystem.main;
        sprintStartParticleSize = sprintStartMain.startSize.constant;
        
        
    }

    private void Update()
    {
        // State checker
        switch (playerSM.currentState)
        {
            case NeutralPState:

            case HitPState:

            case KnockPState:
                if (sprintTrailRenderer.emitting)
                {
                    sprintTrailRenderer.emitting = false;
                }
                break;
            case SprintPState:
                // Enable the trail renderer when sprinting
                if (!sprintTrailRenderer.emitting)
                {
                    sprintTrailRenderer.emitting = true;
                }
                break;
        }
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

    public void OnSprintStarted()
    {
        if (!sprintStartParticleSystem.isPlaying)
        {
            SprintBoostUpdater();
            var main = sprintStartParticleSystem.main;
            float r = playerScript.currentSprintBoost / playerScript.sprintMaxBoostSpeed;
            main.startSize= sprintStartParticleSize * r;
            sprintStartParticleSystem.Play();
        }
    }

    public void SprintBoostUpdater()
    {
        // TODO : update the sprint boost's color.
    }
}
