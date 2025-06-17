using UnityEngine;

public class ExitStageState : CameraState
{
    public ExitStageState(CameraScript cameraScript) : base(cameraScript)
    {
        
    }

    public override void OnEnter()
    {
        Debug.Log("OnEnter : exit stage");
    }
    
    public override void OnUpdate()
    {
        
    }
        
    public override void OnExit()
    {
        
    }
}
