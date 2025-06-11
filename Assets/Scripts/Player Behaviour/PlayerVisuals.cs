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
        var hitShape = hitParticleSystem.shape;
        var hitMain = hitParticleSystem.main;
        float hitStartSize = hitMain.startSize.constant;
        float currentShapeRadius = hitShape.radius;
        hitShape.radius = playerScript.hitRadius - Mathf.Clamp(hitStartSize / 2, 0, currentShapeRadius / 2); // Adjust radius to account for the particle's start size
        
        
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
        ParticleSystem instanceParticleSystem = instance.GetComponent<ParticleSystem>();
        // Destroy the instance after the particle system has finished playing
        Destroy(instance, instanceParticleSystem.main.duration * 2);
        instance.transform.SetParent(transform);
        instance.transform.rotation = Quaternion.identity;
        instanceParticleSystem.Play();
        
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
