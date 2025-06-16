using UnityEngine;
[CreateAssetMenu(fileName = "HitBState", menuName = "Ball/States/HitBState")]

public class HitBState : BallState
{
    private float _timer;
    public override void Enter()
    {
        base.Enter();
        BallScript.BallSpeedClamp();
        BallScript.currentVelocityMagnitude = BallScript.rb.linearVelocity.magnitude; // Store the current speed
        _timer = 0;
        BallScript.speedDropTimer = 0; // Reset the speed drop timer
        // Ignore collisions with the owner player.
        if (BallScript.ownerPlayer != null)
        {
            Physics.IgnoreCollision(BallScript.GetComponent<CapsuleCollider>(), BallScript.ownerPlayer.GetComponent<CapsuleCollider>());
        }
        else
        {
            Debug.LogWarning("Ball owner player is null, cannot ignore collision.");
        }

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
    
    public override void Exit()
    {
        base.Exit();
        // Reset any necessary variables or states here if needed
        _timer = 0;
        // Unignore collisions with the owner player.
        if (BallScript.ownerPlayer != null)
        {
            Physics.IgnoreCollision(BallScript.GetComponent<CapsuleCollider>(), BallScript.ownerPlayer.GetComponent<CapsuleCollider>(), false);
        }
        else
        {
            Debug.LogWarning("Ball owner player is null, cannot unignore collision.");
        }

    }
}
