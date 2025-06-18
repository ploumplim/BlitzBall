using UnityEngine;

public class HitPState : PlayerState
{
    private float _timer;
    private GameObject hitBall;
    private bool _ballHit;

    public override void Enter()
    {
        base.Enter();
        PlayerScript.onHitPressed?.Invoke();
        _timer = 0;
        _ballHit = false;
        hitBall = null; // Reset hitBall to ensure it's null at the start
    }

    
    public override void UpdateTick()
    {
        base.UpdateTick();
        _timer += Time.deltaTime;
        
        if (!hitBall)
        {
            hitBall = PlayerScript.BallInConeHitBox(PlayerScript.hitRadius,forwardOffset: PlayerScript.hitForwardOffset, fixedAngle: PlayerScript.hitAngle);
        }

        if (_timer > PlayerScript.hitDuration && (!hitBall || _ballHit))
        {
            PlayerSM.ChangeState(PlayerSM.states[0]);
        }
    }
    
    public override void FixedTick()
    {
        base.FixedTick();
        PlayerScript.Aim(PlayerScript.baseRotationSpeed);
        PlayerScript.Move(PlayerScript.baseSpeed);
        if (hitBall && !_ballHit)
        {
            _ballHit = true;
            
            BallSM hitBallSM = hitBall.GetComponent<BallSM>();
            BallScript hitBallScript = hitBall.GetComponent<BallScript>();
            // Apply a force to the ball using the forward direction of the player
            Rigidbody ballRigidbody = hitBall.GetComponent<Rigidbody>();
            
            if (ballRigidbody != null)
            {
                Vector3 forceDirection = PlayerScript.transform.forward;
                ballRigidbody.linearVelocity =
                    forceDirection * (ballRigidbody.linearVelocity.magnitude + PlayerScript.hitForce);
                hitBallScript.ownerPlayer = PlayerScript.gameObject; // Set the owner of the ball to the player
                hitBallSM.ChangeState(hitBallSM.states[1]); // Change to HitBState after hitting
                hitBall = null; // Reset hitBall after hitting
            }
        }
        
    }
    
}
