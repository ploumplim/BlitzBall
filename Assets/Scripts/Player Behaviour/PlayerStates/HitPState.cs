using UnityEditor.VersionControl;
using UnityEngine;
[CreateAssetMenu(fileName = "HitPState", menuName = "Player/States/HitPState")]
public class HitPState : PlayerState
{
    private float _timer;
    private GameObject hitBall;

    public override void Enter()
    {
        _timer = 0;
    }

    
    public override void UpdateTick()
    {
        base.UpdateTick();
        _timer += Time.deltaTime;
        
        if (!hitBall)
        {
            hitBall = PlayerScript.BallInConeHitBox(PlayerScript.hitMinAngle, PlayerScript.hitMaxAngle, PlayerScript.hitRadius);
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
            // Apply a force to the ball using the forward direction of the player
            Rigidbody ballRigidbody = hitBall.GetComponent<Rigidbody>();
            if (ballRigidbody != null)
            {
                Vector3 forceDirection = PlayerScript.transform.forward;
                ballRigidbody.AddForce(forceDirection * PlayerScript.hitForce, ForceMode.Impulse);
                hitBall = null; // Reset hitBall after hitting
            }
        }
        
    }
}
