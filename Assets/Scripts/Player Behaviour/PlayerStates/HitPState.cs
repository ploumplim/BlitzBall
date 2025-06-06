using UnityEditor.VersionControl;
using UnityEngine;
[CreateAssetMenu(fileName = "HitPState", menuName = "Player/States/HitPState")]
public class HitPState : PlayerState
{
    private float _timer;
    private GameObject hitBall;

    public override void Enter()
    {
        base.Enter();
        PlayerScript.onHitPressed?.Invoke();
        _timer = 0;
    }

    
    public override void UpdateTick()
    {
        base.UpdateTick();
        _timer += Time.deltaTime;
        
        if (!hitBall)
        {
            hitBall = PlayerScript.BallInConeHitBox(PlayerScript.hitRadius, fixedAngle: PlayerScript.hitAngle);
        }

        if (_timer > PlayerScript.hitDuration && !hitBall)
        {
            PlayerSM.ChangeState(PlayerSM.states[0]);
        }
    }
    
    public override void FixedTick()
    {
        base.FixedTick();
        if (hitBall)
        {
            BallSM hitBallSM = hitBall.GetComponent<BallSM>();
            // Apply a force to the ball using the forward direction of the player
            Rigidbody ballRigidbody = hitBall.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                Vector3 forceDirection = PlayerScript.transform.forward;
                ballRigidbody.linearVelocity =
                    forceDirection * (ballRigidbody.linearVelocity.magnitude + PlayerScript.hitForce);
                
                hitBallSM.ChangeState(hitBallSM.states[1]); // Change to HitBState after hitting
                hitBall = null; // Reset hitBall after hitting
                PlayerSM.ChangeState(PlayerSM.states[0]); // Change to NeutralState after hitting
            }
        }
        
    }
}
