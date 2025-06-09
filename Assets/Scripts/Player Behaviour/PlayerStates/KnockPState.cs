using UnityEngine;
[CreateAssetMenu(fileName = "KnockPState", menuName = "Player/States/KnockPState")]
public class KnockPState : PlayerState
{
    private float timer;
    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        // Set the player's velocity to zero when entering the knockdown state
        PlayerScript.rb.linearVelocity = Vector3.zero;
        // Change the player's mass and linear damping to simulate knockback
        PlayerScript.rb.mass *= PlayerScript.knockbackMassMult;
        PlayerScript.rb.linearDamping *= PlayerScript.knockbackLinearDampingMult;
        // Apply knockback force using the lastCollidedBall if it exists
        if (PlayerScript.lastCollidedBall != null)
        {
            Vector3 knockbackDirection = (PlayerScript.transform.position - PlayerScript.lastCollidedBall.transform.position).normalized;
            PlayerScript.rb.AddForce(knockbackDirection * PlayerScript.knockbackForce, ForceMode.Impulse);
        }
        
    }
    
    public override void UpdateTick()
    {
        base.UpdateTick();
        timer += Time.deltaTime;
        // Transition to the idle state after a certain duration
        if (timer >= PlayerScript.knockbackDuration)
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
