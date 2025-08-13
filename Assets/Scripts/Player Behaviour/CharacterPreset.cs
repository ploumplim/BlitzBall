using UnityEngine;

[CreateAssetMenu(menuName = "Blitz Balls/Character Preset", fileName = "New Character Preset")]
public class CharacterPreset : ScriptableObject
{
    [Header("Movement Settings")] 
    public float baseSpeed;
    public float acceleration;
    public float baseRotationSpeed;
    
    [Header("Sprint Settings")]
    public float sprintMaxBoostSpeed = 10f;
    public float sprintSpeed = 20f;
    public float sprintBoostDecayTime = 0.5f;
    public AnimationCurve sprintCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float sprintBoostRecoveryRate = 0.5f;

    [Header("Hit Settings")]
    public float hitForce = 10f;
    public float hitDuration = 0.2f;
    public float hitCooldown = 0.5f;
    public float hitRadius = 1.0f;
    public float hitAngle = 360f;
    public float hitForwardOffset = 0.5f;
    
    [Header("Knockback Settings")]
    public float knockbackMassMult = 1.0f;
    public float knockbackLinearDampingMult = 0.5f;
    public float fullKnockBackForce = 20f;
    public float fullKnockBackDuration = 1.5f;
    public AnimationCurve knockbackDurationCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public AnimationCurve knockbackForceCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    
    [Header("Input Buffering Settings")]
    public float inputBufferTime = 0.2f;
}
