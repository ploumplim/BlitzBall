using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    //Editor variables
    [Header("Movement Settings")] 
    public float baseSpeed;
    public float acceleration;
    public float baseRotationSpeed;
    
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
        // Make the game object's forward position be equal to the aim vector
        if (aimVec2.sqrMagnitude > 0.01f)
        {
            Vector3 aimDirection = new Vector3(aimVec2.x, 0, aimVec2.y).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, baseRotationSpeed * Time.deltaTime);
        }
    }

    public void ConeHitBox(float minAngle, float maxAngle, float detectionRadius)
    {
        if (minAngle < 0 || maxAngle > 360 || minAngle >= maxAngle)
        {
            Debug.LogWarning("Min angle is out of range.");
            return;
        }

        Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, inConeColliders);
        foreach (var hitCollider in inConeColliders)
        {
            if (!hitCollider.CompareTag("Ball"))
            {
                continue;
            }
        }

    }
}
