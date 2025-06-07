using UnityEngine;
[CreateAssetMenu(fileName = "SprintPState", menuName = "Player/States/SprintPState")]
public class SprintPState : PlayerState
{
    private float _timer;
    private float _currentSprintSpeed;
    [HideInInspector] public float currentSprintBoost;

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;
        _currentSprintSpeed = PlayerScript.sprintSpeed + currentSprintBoost;
        currentSprintBoost = 0f;
    }

    public override void UpdateTick()
    {
        base.UpdateTick();
        _timer += Time.deltaTime;
        float r = 0f;
        if (PlayerScript.sprintBoostDecayTime > 0)
        {
            r = _timer / PlayerScript.sprintBoostDecayTime;
        }
        else
        {
            Debug.LogError("Sprint boost decay time is 0, please set a value in the player prefab.");
        }
        r = Mathf.Clamp01(r);
        float curveVal = PlayerScript.sprintCurve.Evaluate(r);
        
        _currentSprintSpeed = Mathf.Lerp(_currentSprintSpeed, PlayerScript.sprintSpeed, curveVal);
        
        if (PlayerScript.moveVec2 == Vector2.zero)
        {
            PlayerSM.ChangeState(PlayerSM.states[0]); // Change to NeutralState if no movement input
        }
    }
}
