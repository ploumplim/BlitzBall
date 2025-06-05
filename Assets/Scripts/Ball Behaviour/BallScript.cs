using UnityEngine;

public class BallScript : MonoBehaviour
{
    [Header("Ball Settings")]
    public float maximumLinearVelocity = 100f;
    [HideInInspector] public Vector3 currentVelocityVec3;
    private float currentVelocity;
    [HideInInspector]public Rigidbody rb;
    
    public void PreventSpeedDrop()
    {
        // Prevent the ball from dropping below a certain speed by comparing its current linearvelocity against the
        // currentVelocity. 
    }
}
