using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EntryStageState: CameraState
{
    private PlayableDirector director;
    
    public EntryStageState(CameraScript cameraScript, PlayableDirector director) : base(cameraScript)
    {
        this.director = director;
    }
    
    public override void OnEnter()
    {
        director.Play();
    }
    
    public override void OnUpdate()
    {
        
    }
        
    public override void OnExit()
    {
        
    }
}
