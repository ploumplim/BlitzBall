using System;
using UnityEngine;
using UnityEngine.Events;

public class BallScript : MonoBehaviour
{
    // Editor Variables
    [Header("Ball Settings")]
    public float maximumLinearVelocity = 100f;
    public float firstHitSpeed = 10f; // Speed of the ball on the first hit
    public float playerCollisionSpeed = 2f; // Speed of the ball when hit by a player
    
    [Header("State settings")]
    public float hitDuration = 0.2f; // Duration of the hit state
    
    // Method Variables
    [HideInInspector] public Vector3 currentVelocityVec3;
    private float velocityFloor;
    [HideInInspector]public Rigidbody rb;
    
    // Events
    public UnityEvent OnWallCollision;
    public UnityEvent OnPlayerCollision;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ClampBallSpeed(Collision hitCollider = null)
    {
        float magnitude = rb.linearVelocity.magnitude;

        if (hitCollider != null)
        {
            switch (hitCollider.gameObject.tag)
            {
                case "Player":
                    // If player is hit, reduce disc speed.
                    rb.linearVelocity = rb.linearVelocity.normalized * playerCollisionSpeed;                    // Debug.Log("Ball speed clamped to player collision speed: " + playerCollisionSpeed);
                    return;
                case "NeutralWall": 
                    // ball does not need clamping.
                    return;
            }
        }
        
        // Clamp to first hit speed if below threshold or hit by player
        if (magnitude < firstHitSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * firstHitSpeed;
            // Debug.Log("Ball speed clamped to first hit speed: " + firstHitSpeed);
            return;
        }
        // Update velocity floor if within range
        if (magnitude > velocityFloor && magnitude < maximumLinearVelocity)
        {
            velocityFloor = magnitude;
        }
        // Remove Y component and clamp velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).normalized 
                            * Mathf.Clamp(magnitude, velocityFloor, maximumLinearVelocity);
        // Debug.Log("Clamped ball speed: " + rb.linearVelocity.magnitude);
    }

    private void OnCollisionEnter(Collision other)
    {
        ClampBallSpeed(other);

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
