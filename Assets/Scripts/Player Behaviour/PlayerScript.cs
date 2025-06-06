using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Aim Modes
    public enum AimMode
    {
        ForwardDirection, // Aim in the forward direction of the player
        RightStickDirection // Aim in the direction of the right stick
    }
    
    
    //Editor variables
    [Header("Movement Settings")] 
    public float baseSpeed;
    public float acceleration;
    public float baseRotationSpeed;
    
    [Header("Aim Settings")]
    public AimMode aimMode = AimMode.ForwardDirection; // Default to forward direction aiming

    [Header("Hit Settings")]
    public float hitForce = 10f; // Force applied to the ball when hit
    public float hitDuration = 0.2f;
    public float hitCooldown = 0.5f; // Cooldown time for hit action
    public float hitRadius = 1.0f; // Radius for hit detection
    public float hitAngle = 360f; // Angle for hit detection cone
    
    
    //Hidden public variables
    [HideInInspector] public Vector2 moveVec2;
    [HideInInspector] public Vector2 aimVec2;
    
    // References
    private PlayerSM playerSM;
    private PlayerInput playerInput;
    private Rigidbody rb;
    
    // Inputs
    private InputAction aimInput;
    private InputAction moveInput;
    private InputAction hitInput;
    private InputAction specialInput;
    private InputAction sprintInput;
    
    private InputAction bufferedInput;
    
    // Method Variables
    private Collider[] inConeColliders;
    private bool usingConeHit; // Flag to check if the cone hit detection is being used
    private float currentAngle; // Current angle for the cone hit detection
    private float currentDetectionRadius; // Current detection radius for the cone hit detection
    private float currentHitCooldownTimer; // Current cooldown for the hit action
    
    // Events
    public UnityEvent onHitPressed;

    private void Start()
    {
        playerSM = GetComponent<PlayerSM>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        
        // Initialize Inputs
        aimInput = playerInput.actions["Aim"];
        moveInput = playerInput.actions["Move"];
        hitInput = playerInput.actions["Hit"];
        specialInput = playerInput.actions["Special"];
        sprintInput = playerInput.actions["Sprint"];
        
        // Timers
        currentHitCooldownTimer = 0f;
    }

    private void Update()
    {
        moveVec2 = moveInput.ReadValue<Vector2>();
        
        // Hit timer
        if (currentHitCooldownTimer < hitCooldown)
        {
            currentHitCooldownTimer += Time.deltaTime;
        }
    }

    public void Move(float speed)
    {
        Vector3 moveDirection = new Vector3(moveVec2.x, 0, moveVec2.y).normalized;
        rb.AddForce(moveDirection * Mathf.Lerp(0, speed, acceleration), ForceMode.VelocityChange);
    }

    public void Aim(float rotationSpeed)
    {
        switch (aimMode)
        {
            case AimMode.ForwardDirection:
                // Turn the player to face towards their front.
                aimVec2 = moveVec2;
                break;
            case AimMode.RightStickDirection:
                // Use the right stick direction for aiming
                aimVec2 = aimInput.ReadValue<Vector2>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // Make the game object's forward position be equal to the aim vector
        if (aimVec2.sqrMagnitude > 0.01f)
        {
            Vector3 aimDirection = new Vector3(aimVec2.x, 0, aimVec2.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, baseRotationSpeed * Time.deltaTime);
        }
    }

    // Input Methods

    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.started && currentHitCooldownTimer >= hitCooldown)
        {
            currentHitCooldownTimer = 0f; // Reset the hit cooldown timer
            
            switch (playerSM.currentState)
            {
                case NeutralPState: // Neutral State
                    playerSM.ChangeState(playerSM.states[1]); // Change to Hit State
                    break;
                default:
                    // buffer the input
                    bufferedInput = hitInput;
                    break;
            }
            
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        // Reset the current scene
        if (context.started)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
    
    
    // This method checks for a ball within a cone defined by minAngle and maxAngle, and returns the first ball found within that cone.
    public GameObject BallInConeHitBox( float detectionRadius, bool isFixed = true, float minAngle = 0f, float maxAngle = 360f, float fixedAngle = 360f, AnimationCurve transitionCurve = null,
        float currentTime = 0f, float maxTime = 1f)
    {
        usingConeHit = true;
        if (!isFixed) // if the angle is not fixed, we need to check the min and max angles and set the animation curve accordingly
        {
            if (minAngle < 0 || maxAngle > 360 || minAngle >= maxAngle)
            {
                Debug.LogWarning("Angle limits are out of range: (min) " + minAngle + " -> (max) " + maxAngle);
                return null;
            }

            if (transitionCurve == null)
            {
                Debug.Log("No transition curve provided, using default linear transition.");
                transitionCurve = AnimationCurve.Linear(0, 0, 1, 1);
            }
            
            if (currentTime < 0 || currentTime > maxTime || maxTime <= 0)
            {
                Debug.LogWarning("Current time is out of range: " + currentTime + ". It should be between 0 and " + maxTime);
                return null;
            }
            
            float currentTimeNormalized = Mathf.Clamp01(currentTime / maxTime);
            float curveValue = transitionCurve.Evaluate(currentTimeNormalized);
            fixedAngle = Mathf.Lerp(minAngle, maxAngle, curveValue);
        }
        
        currentAngle = fixedAngle; // Update the current angle to the fixed angle for the cone hit detection
        currentDetectionRadius = detectionRadius; // Update the current detection radius
        
        inConeColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider hitCollider in inConeColliders)
        {
            if (!hitCollider.gameObject.CompareTag("Ball"))
            {
                continue;
            }
            Vector3 directionToObject = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToObject);
            if (angle <= fixedAngle / 2f)
            {
                currentAngle = 0f;
                currentDetectionRadius = 0f;
                usingConeHit = false;
                // If the ball is within the cone, return it
                return hitCollider.gameObject;
            }
        }
        
        //Reset all cone hit detection variables
        currentAngle = 0f;
        currentDetectionRadius = 0f;
        usingConeHit = false;
        return null;
    }
    
    //Draw gizmo of the forward direction
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
        
        // Draw the angle whenever the hitcone is being used
        if (usingConeHit)
        {
            Handles.color = new Color(1,0,1,0.5f);
            Vector3 forward = transform.forward;
            Handles.DrawSolidArc(transform.position,
                Vector3.up,
                Quaternion.Euler(0, -currentAngle/2f, 0)*forward, currentAngle,
                currentDetectionRadius);
        }
    }
}
