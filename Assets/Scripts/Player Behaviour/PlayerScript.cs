using System;
using UnityEngine;
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
    public float hitMinAngle = 0f; // Minimum angle for hit detection
    public float hitMaxAngle = 360f; // Maximum angle for hit detection
    
    
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
    }

    private void Update()
    {
        moveVec2 = moveInput.ReadValue<Vector2>();
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
        if (context.started)
        {
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
    
    
    
    
    // This method checks for a ball within a cone defined by minAngle and maxAngle, and returns the first ball found within that cone.
    public GameObject BallInConeHitBox(float minAngle, float maxAngle, float detectionRadius)
    {
        if (minAngle < 0 || maxAngle > 360 || minAngle >= maxAngle)
        {
            Debug.LogWarning("Min angle is out of range.");
            return null;
        }
        inConeColliders = Physics.OverlapSphere(transform.position, detectionRadius);
        
        foreach (Collider hitCollider in inConeColliders)
        {
            if (hitCollider.gameObject.CompareTag("Ball"))
            {
                return hitCollider.gameObject;
            }
        }
        return null;
    }
    
    //Draw gizmo of the forward direction
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
