using UnityEngine;

public class EntryStageState: CameraState
{
    public EntryStageState(CameraScript cameraScript) : base(cameraScript)
    {
        
    }
    
    public override void OnEnter()
    {
        Debug.Log("OnEnter : entry stage");
    }
    
    public override void OnUpdate()
    {
        
    }
        
    public override void OnExit()
    {
        
    }
}
