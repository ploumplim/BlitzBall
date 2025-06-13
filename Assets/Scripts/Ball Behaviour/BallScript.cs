using System;
using UnityEngine;
using UnityEngine.Events;

public class BallScript : MonoBehaviour
{
    // Editor Variables
    [Header("Ball Settings")]
    public float maximumLinearVelocity = 100f;
    public float softMaximumLinearVelocity = 50f; // Soft cap for the ball's speed
    public float speedDropDuration = 1f; // Duration over which the speed drops to the softMaximumLinearVelocity
    public AnimationCurve SpeedDropRateCurve = AnimationCurve.Linear(0, 1, 1, 0.5f); // Curve to control speed drop rate till the softMaximumLinearVelocity
    public float firstHitSpeed = 10f; // Speed of the ball on the first hit
    public float playerCollisionSpeed = 2f; // Speed of the ball when hit by a player
    
    
    [Header("State settings")]
    public float hitDuration = 0.2f; // Duration of the hit state
    
    // Method Variables
    [HideInInspector] public Vector3 currentVelocityVec3; // Current velocity vector of the ball (unused)
    [HideInInspector] public float currentVelocityMagnitude; // Current speed of the ball, updated on hit
    private float velocityFloor; // Minimum speed of the ball after the first hit, used to clamp the speed
    [HideInInspector]public Rigidbody rb; // Reference to the Rigidbody component
    [HideInInspector] public BallSM ballSM; // Reference to the BallStateMachine component
    [HideInInspector] public float speedDropTimer; // Timer for the speed drop effect
    [HideInInspector] public GameObject ownerPlayer; // Reference to the player who owns the ball
    
    // Events
    public UnityEvent OnWallCollision;
    public UnityEvent OnPlayerCollision;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        ballSM = GetComponent<BallSM>();
    }

    private void FixedUpdate()
    {
        GradualSpeedDrop();
    }

    private void GradualSpeedDrop()
    {
        if (rb.linearVelocity.magnitude < softMaximumLinearVelocity)
        {
            return;
        }
        
        
        if (speedDropTimer < speedDropDuration)
        {
            speedDropTimer += Time.fixedDeltaTime;
            float dropRate =
                SpeedDropRateCurve.Evaluate(Mathf.Clamp01(speedDropTimer / speedDropDuration));
            float ballSpeedDrop =
                Mathf.Lerp(currentVelocityMagnitude, softMaximumLinearVelocity, dropRate);
            
            rb.linearVelocity = rb.linearVelocity.normalized * ballSpeedDrop;
        }
    }

    public void BallSpeedClamp() // Use this function to clamp the speed to the minimum when under the first hit speed, under the soft maximum linear velocity, or when collided with a player.
    {
        float magnitude = rb.linearVelocity.magnitude;
        
        // Clamp to first hit speed if below threshold or hit by player
        if (magnitude < firstHitSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * firstHitSpeed;
            // Debug.Log("Ball speed clamped to first hit speed: " + firstHitSpeed);
            return;
        }
        
        // Update velocity floor if within range
        if (magnitude > velocityFloor && magnitude < softMaximumLinearVelocity)
        {
            velocityFloor = magnitude;
        }
        
        // Remove Y component and clamp velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized // Current ball direction
                            * Mathf.Clamp(magnitude, velocityFloor, maximumLinearVelocity); // Clamped speed
        
    }

    private void OnCollisionEnter(Collision other)
    {
        BallState ballState = ballSM.currentState;
        
        // If the ball is in the HitBState, we don't want to clamp the speed again.

        

        switch (other.gameObject.tag)
        {
            case "NeutralWall":
                OnWallCollision?.Invoke();
                
                break;
            case "Player":
                OnPlayerCollision?.Invoke();
                break;
        }
        
    }
}
