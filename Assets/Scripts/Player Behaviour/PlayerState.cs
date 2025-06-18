using UnityEngine;

public class PlayerState : MonoBehaviour
{
    protected PlayerScript PlayerScript;
    protected PlayerSM PlayerSM;

    public void Initialize(PlayerScript playerScript, PlayerSM playerSM)
    {
        PlayerSM = playerSM;
        PlayerScript = playerScript;
    }

    public virtual void Enter() { }

    public virtual void UpdateTick() { }
    
    public virtual void FixedTick() { }

    public virtual void Exit() { }
    
}
