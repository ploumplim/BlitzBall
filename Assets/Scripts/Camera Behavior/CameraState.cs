using System;
using UnityEngine;

public abstract class CameraState
{
    protected CameraScript cameraScript;
    
    public CameraState(CameraScript cameraScript)
    {
        this.cameraScript = cameraScript;
    }
    
    public event Action OnEnterState;
    public event Action OnUpdateState;
    public event Action OnExitState;

    public virtual void OnEnter()
    {
        OnEnterState?.Invoke();
    }

    public virtual void OnUpdate()
    {
        OnUpdateState?.Invoke();
    }

    public virtual void OnExit()
    {
        OnExitState?.Invoke();
    }
}
