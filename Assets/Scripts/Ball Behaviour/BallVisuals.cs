using UnityEngine;

public class BallVisuals : MonoBehaviour
{
    [Header("Ball Visuals Settings")]
    // Ball Colors
    public Color ballInHitState = Color.yellow; // Color when the ball is in hit state
    public GameObject ballMeshObject; // Reference to the ball mesh object
    
    
    // Ball References
    private Material currentBallMaterial;
    private Color neutralBallColor;
    private BallScript ballScript;
    private BallSM ballSM;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the BallScript and BallSM components
        ballScript = GetComponent<BallScript>();
        ballSM = GetComponent<BallSM>();
        
        // Get the mterial from the ballMeshObject
        if (ballMeshObject != null)
        {
            currentBallMaterial = ballMeshObject.GetComponent<Renderer>().material;
            neutralBallColor = currentBallMaterial.color; // Store the initial color of the ball
        }
        else
        {
            Debug.LogError("Ball Mesh Object is not assigned in BallVisuals.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Create a switch case with all ball states
        switch (ballSM.currentState)
        {
            case HitBState:
                // Change the ball color to hit state color
                if (currentBallMaterial != null)
                {
                    currentBallMaterial.color = ballInHitState;
                }
                break;
            default:
                // Reset the ball color to neutral when not in hit state
                if (currentBallMaterial != null)
                {
                    currentBallMaterial.color = neutralBallColor;
                }
                break;
        }
    }
}
