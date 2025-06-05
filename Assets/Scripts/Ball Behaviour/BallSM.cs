using UnityEngine;

public class BallSM : MonoBehaviour
{
    [Header("Ball States")]
    [HideInInspector] public BallState currentState;
    public BallState[] states;

    // Private Vars
    private BallScript ballScript;
    
    void Start()
    {
        ballScript = GetComponent<BallScript>();

        // Initialize all states
        foreach (var state in states)
        {
            state.Initialize(ballScript, this);
        }

        // Optionally set an initial state
        if (states.Length > 0)
        {
            currentState = states[0];
            currentState.Enter();
        }
    }
    
    void Update()
    {
        // Call the Tick method of the current state
        currentState?.UpdateTick();
    }
    
    void FixedUpdate()
    {
        currentState?.FixedTick();
    }
    
    public void ChangeState(BallState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
