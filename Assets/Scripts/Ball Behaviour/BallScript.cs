using System;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    // Editor Variables
    [Header("Ball Settings")]
    public float maximumLinearVelocity = 100f;
    public float firstHitSpeed = 10f; // Speed of the ball on the first hit
    
    [Header("State settings")]
    public float hitDuration = 0.2f; // Duration of the hit state
    
    // Method Variables
    [HideInInspector] public Vector3 currentVelocityVec3;
    private float velocityFloor;
    [HideInInspector]public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ClampBallSpeed()
    {
        if (rb.linearVelocity.magnitude < firstHitSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * firstHitSpeed;
            Debug.Log("Ball speed clamped to first hit speed: " + firstHitSpeed);
            return;
        }
        
        
        if (rb.linearVelocity.magnitude > velocityFloor)
        {
            velocityFloor = rb.linearVelocity.magnitude;
        }
        
        rb.linearVelocity = rb.linearVelocity.normalized * Mathf.Clamp(rb.linearVelocity.magnitude, velocityFloor, maximumLinearVelocity);
        Debug.Log("Clamped ball speed: " + rb.linearVelocity.magnitude);
    }
}
