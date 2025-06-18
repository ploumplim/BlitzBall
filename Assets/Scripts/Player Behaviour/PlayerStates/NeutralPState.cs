using UnityEngine;
[CreateAssetMenu(fileName = "NeutralPState", menuName = "Player/States/NeutralPState")]
public class NeutralPState : PlayerState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void FixedTick()
    {
        
        if (!PlayerScript.rb)
        {
            return;
        }
        else
        {
            base.FixedTick();
            PlayerScript.Move(PlayerScript.baseSpeed);
            PlayerScript.Aim(PlayerScript.baseRotationSpeed);
        }
        
    }
}
