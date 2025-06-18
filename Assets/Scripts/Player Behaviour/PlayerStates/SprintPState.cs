using UnityEngine;

public class SprintPState : PlayerState
{
    private float _timer;
    private float _currentSprintSpeed;

    public override void Enter()
    {
        base.Enter();
        _timer = 0f;
        PlayerScript.onSprintStarted?.Invoke();
        _currentSprintSpeed = PlayerScript.sprintSpeed + PlayerScript.currentSprintBoost;
        PlayerScript.currentSprintBoost = 0f;
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
        
        if (PlayerScript.moveVec2 == Vector2.zero )
        {
            PlayerSM.ChangeState(PlayerSM.states[0]); // Change to NeutralState if no movement input
        }
    }

    public override void FixedTick()
    {
        base.FixedTick();
        PlayerScript.Move(_currentSprintSpeed);
        PlayerScript.Aim(PlayerScript.baseRotationSpeed);
    }

    public override void Exit()
    {
        base.Exit();
        PlayerScript.onSprintEnded?.Invoke();
        _timer = 0f;
    }
}
