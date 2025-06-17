using UnityEngine;
[CreateAssetMenu(fileName = "KnockPState", menuName = "Player/States/KnockPState")]
public class KnockPState : PlayerState
{
    private float timer;
    [HideInInspector] public float knockbackDuration; // Duration of the knockback effect
    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        // Set the player's velocity to zero when entering the knockdown state
        PlayerScript.rb.linearVelocity = Vector3.zero;
        // Change the player's mass and linear damping to simulate knockback
        PlayerScript.rb.mass *= PlayerScript.knockbackMassMult;
        PlayerScript.rb.linearDamping *= PlayerScript.knockbackLinearDampingMult;
        // If lastCollidedBall exists, apply a knockback force away from the ball using the knockback force curve
        if (PlayerScript.lastCollidedBall != null)
        {
            BallScript collidedBallScript = PlayerScript.lastCollidedBall.GetComponent<BallScript>();
            if (collidedBallScript != null)
            {
                Vector3 knockbackDirection = (PlayerScript.transform.position - collidedBallScript.transform.position).normalized;
                float knockbackForce = PlayerScript.knockbackForceCurve.Evaluate(collidedBallScript.rb.linearVelocity.magnitude / 
                    collidedBallScript.maximumLinearVelocity) * PlayerScript.fullKnockBackForce;
                PlayerScript.rb.linearVelocity = knockbackDirection * knockbackForce;
            }
        }
        
    }
    
    public override void UpdateTick()
    {
        base.UpdateTick();
        timer += Time.deltaTime;
        
        // Using the duration curve, calculate the knockback duration based on the last ball collided's current velocity
        if (PlayerScript.lastCollidedBall != null)
        {
            BallScript collidedBallScript = PlayerScript.lastCollidedBall.GetComponent<BallScript>();
            if (collidedBallScript != null)
            {
                knockbackDuration = PlayerScript.knockbackDurationCurve.Evaluate(collidedBallScript.rb.linearVelocity.magnitude / 
                    collidedBallScript.maximumLinearVelocity) * PlayerScript.fullKnockBackDuration;
                
            }
        }
        
        
        
        // Transition to the idle state after a certain duration
        if (timer >= knockbackDuration)
        {
            PlayerSM.ChangeState(PlayerSM.states[0]);
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        // Reset the player's mass and linear damping when exiting the knockdown state
        PlayerScript.rb.mass /= PlayerScript.knockbackMassMult;
        PlayerScript.rb.linearDamping /= PlayerScript.knockbackLinearDampingMult;
        // Reset lastCollidedBall to null
        PlayerScript.lastCollidedBall = null;
    }
}
