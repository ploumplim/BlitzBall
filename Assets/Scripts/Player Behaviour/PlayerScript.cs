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
    
    [Header("Sprint Settings")]
    public float sprintMaxBoostSpeed = 10f; // Maximum speed when sprinting
    public float sprintSpeed = 20f; // Maximum speed when sprinting after initial speed
    public float sprintBoostDecayTime = 0.5f; // Time it takes for the sprint boost to decay
    public AnimationCurve sprintCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Curve for sprint speed transition
    public float sprintBoostRecoveryRate = 0.5f; // Rate at which the sprint boost recovers
    
    [Header("Aim Settings")]
    public AimMode aimMode = AimMode.ForwardDirection; // Default to forward direction aiming

    [Header("Hit Settings")]
    public float hitForce = 10f; // Force applied to the ball when hit
    public float hitDuration = 0.2f;
    public float hitCooldown = 0.5f; // Cooldown time for hit action
    public float hitRadius = 1.0f; // Radius for hit detection
    public float hitAngle = 360f; // Angle for hit detection cone
    
    [Header("Knockback Settings")]
    public float knockbackForce = 5f; // Force applied to the player when hit by a ball
    public float knockbackDuration = 0.5f; // Duration of the knock-back effect
    public float knockbackMassMult = 1.0f; // Multiplier for the player's mass during knock-back
    public float knockbackLinearDampingMult = 0.5f; // Linear damping applied to the player during knock-back
    
    [Header("Input Buffering Settings")]
    public float inputBufferTime = 0.2f; // Time to buffer inputs
    
    //Hidden public variables
    [HideInInspector] public Vector2 moveVec2;
    [HideInInspector] public Vector2 aimVec2;
    
    // private References
    private PlayerSM playerSM;
    private PlayerInput playerInput;
    private InputActionAsset inputActionAsset;
    [HideInInspector]public Rigidbody rb;
    
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
    private float inputBufferTimer; // Timer for input buffering
    private bool isBuffered; // Flag to check if the input is buffered
    [HideInInspector] public float currentSprintBoost; // Current sprint boost value
    private float currentPlayerMass; // Current mass of the player, used for hit calculations
    private float currentPlayerLinearDamping; // Current linear damping of the player, used for hit calculations
    [HideInInspector] public GameObject lastCollidedBall; // Last ball collided with, used for hit calculations
    
    
    // Events
    public UnityEvent onHitPressed;
    public UnityEvent onSprintStarted;
    public UnityEvent onSprintEnded;


    
    private void Start()
    {
        playerSM = GetComponent<PlayerSM>();
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        inputActionAsset = playerInput.actions; // Get the InputActionAsset from PlayerInput
        inputActionAsset.FindActionMap("BasicMap").Enable(); // Enable the BasicMap action map
        
        
        // Initialize Inputs
        aimInput = InputSystem.actions.FindAction("Aim");
        moveInput = InputSystem.actions.FindAction("Move");
        hitInput = InputSystem.actions.FindAction("Hit");
        specialInput = InputSystem.actions.FindAction("Special");
        sprintInput = InputSystem.actions.FindAction("Sprint");
        
        // Timers
        currentHitCooldownTimer = 0f;
        
        // Assign Physics properties
        currentPlayerMass = rb.mass;
        currentPlayerLinearDamping = rb.linearDamping;
    }

    private void OnDisable()
    {
        inputActionAsset.FindActionMap("BasicMap").Disable(); // Enable the BasicMap action map

    }

    private void Update()
    {
        // Send a debug log when I press the hit button
        
        moveVec2 = moveInput.ReadValue<Vector2>();

        // Hit timer
        if (currentHitCooldownTimer < hitCooldown)
        {
            currentHitCooldownTimer += Time.deltaTime;
        }
        
        // Sprintboost recovery
        if (currentSprintBoost < sprintMaxBoostSpeed)
        {
            currentSprintBoost += sprintBoostRecoveryRate * Time.deltaTime;
        }

        // Input Buffering
        if (bufferedInput != null)
        {
            inputBufferTimer += Time.deltaTime;

            // Execute buffered input if in Neutral State
            if (playerSM.currentState == playerSM.states[0]) 
            {
                ExecuteBufferedInput();
            }

            // Clear buffer if timer exceeds limit
            if (inputBufferTimer >= inputBufferTime)
            {
                ClearBuffer();
            }
        }
        
        // If the input is released while sprinting, change to Neutral State
        if (sprintInput.WasReleasedThisFrame() && playerSM.currentState == playerSM.states[4])
        {
            playerSM.ChangeState(playerSM.states[0]); // Change to Neutral State
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Ball":
                // Handle collision with ball
                lastCollidedBall = other.gameObject;
                if (playerSM.currentState == playerSM.states[0]) // If in neutral state, set to knockback state.
                {
                    playerSM.ChangeState(playerSM.states[2]); // Change to KnockPState
                }
                break;
        }
    }


    // Input buffering methods
    
    void ExecuteBufferedInput()
    {
        isBuffered = true;
        switch (bufferedInput.name)
        {
            case "Hit":
                OnHit();
                break;
            case "Special":
                // Handle special input if needed
                break;
            case "Sprint":
                OnSprint();
                break;
            default:
                Debug.LogWarning($"Buffered input not recognized: {bufferedInput.name}");
                break;
        }
        ClearBuffer();
    }
    
    void ClearBuffer()
    {
        bufferedInput = null;
        inputBufferTimer = 0f;
    }
    
    // Input methods

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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Input Methods

    public void OnHit()
    {
        if (isBuffered || currentHitCooldownTimer >= hitCooldown)
        {
            currentHitCooldownTimer = 0f; // Reset the hit cooldown timer
            switch (playerSM.currentState)
            {
                case NeutralPState: // Neutral State
                case SprintPState: // Sprint State
                    isBuffered = false;
                    playerSM.ChangeState(playerSM.states[1]); // Change to Hit State
                    break;
                default:
                    // buffer the input
                    bufferedInput = hitInput;
                    break;
            }
        }
    }
    public void OnSprint()
    {
        if (isBuffered || moveVec2 != Vector2.zero)
        {
            switch (playerSM.currentState)
            {
                case NeutralPState: // Neutral State
                    isBuffered = false;
                    playerSM.ChangeState(playerSM.states[4]); // Change to Sprint State
                    break;
                default:
                    bufferedInput = sprintInput; // Buffer the sprint input if in Hit State
                    break;
            }
        }
        

    }

    public void OnDebugReset()
    {
        // Reset the current scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

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
