using UnityEngine;

public class PlayerSM : MonoBehaviour
{
    [Header("Player States")]
    [HideInInspector] public PlayerState currentState;
    public PlayerState[] states;

    // Private Vars
    private PlayerScript playerScript;

    void Start()
    {
        playerScript = GetComponent<PlayerScript>();

        // Initialize all states
        foreach (var state in states)
        {
            state.Initialize(playerScript, this);
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

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}