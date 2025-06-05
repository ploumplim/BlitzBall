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
        aimVec2 = aimInput.ReadValue<Vector2>();
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
                break;
            case AimMode.RightStickDirection:
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

        Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, inConeColliders);
        foreach (var hitCollider in inConeColliders)
        {
            if (!hitCollider.CompareTag("Ball"))
            {
                continue;
            }
            else
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
    }
}
