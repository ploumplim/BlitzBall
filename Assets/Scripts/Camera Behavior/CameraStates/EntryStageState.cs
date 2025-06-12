using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Vector3 = System.Numerics.Vector3;

public class EntryStageState: CameraState
{
    private PlayableDirector director;
    private float cameraDistance;
    private float minFOV;
    
    public EntryStageState(CameraScript cameraScript, PlayableDirector director, float cameraDistance, float minFOV) : base(cameraScript)
    {
        this.director = director;
        this.cameraDistance = cameraDistance;
        this.minFOV = minFOV;
    }
    
    public override void OnEnter()
    {
        cameraDistance = cameraScript.minDistance;
        cameraScript.mainCamera.fieldOfView = minFOV;
        director.Play();
    }
    
    public override void OnUpdate()
    {
        
    }
        
    public override void OnExit()
    {
        
    }
}
