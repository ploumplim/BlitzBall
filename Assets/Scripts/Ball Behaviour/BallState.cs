using UnityEngine;

[CreateAssetMenu(fileName = "NewBallState", menuName = "Ball/States")]

public class BallState : ScriptableObject
{
    protected BallScript BallScript;
    protected BallSM BallSM;

    public void Initialize(BallScript ballScript, BallSM ballSm)
    {
        BallScript = ballScript;
        BallSM = ballSm;
    }

    public virtual void Enter() { }

    public virtual void UpdateTick() { }
    
    public virtual void FixedTick() { }

    public virtual void Exit() { }
    
}