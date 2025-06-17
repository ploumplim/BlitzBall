using TMPro;
using UnityEngine;

public class DebugCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI ballStats;
    public GameObject ball;
    
    private BallScript ballScript;
    private BallSM ballSM;
    
    
    private void Start()
    {
        ballScript = ball.GetComponent<BallScript>();
        ballSM = ball.GetComponent<BallSM>();
    }

    private void Update()
    {
        //Debug the ball's state, the current velocity, and the speed drop timer if above 0. Round all floats to two decimal places
        if (ballSM != null && ballScript != null)
        {
            string currentState = ballSM.currentState.GetType().Name;
            string currentVelocity = ballScript.rb.linearVelocity.magnitude.ToString("F2");
            string speedDropTimer = ballScript.speedDropTimer.ToString("F2");

            ballStats.text = $"Ball State: {currentState}\n" +
                             $"Current Velocity: {currentVelocity}\n" +
                             $"Speed Drop Timer: {speedDropTimer}\n" +
                             "---------------------------------";
        }
        else
        {
            ballStats.text = "Ball Script or Ball State Machine not found.";
        }
    }
}
