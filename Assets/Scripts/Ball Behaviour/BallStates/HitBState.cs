using UnityEngine;
[CreateAssetMenu(fileName = "HitBState", menuName = "Ball/States/HitBState")]

public class HitBState : BallState
{
    private float _timer;
    public override void Enter()
    {
        base.Enter();
        BallScript.ClampBallSpeed();
        _timer = 0f;
    }

    public override void UpdateTick()
    {
        base.UpdateTick();
        _timer += Time.deltaTime;
        if (_timer > BallScript.hitDuration)
        {
            BallSM.ChangeState(BallSM.states[2]); // Change to FlightState after hit duration
        }
    }
}
