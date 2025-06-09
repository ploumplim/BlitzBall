using UnityEngine;

public class NeutralState : CameraState
{
    private Transform camHolder;
    private Transform object1;
    private Transform object2;
    private float lerpPosition;
    
    private float influence;
    private Vector3 targetPoint;
    
    private float cameraDistance;
    private float minDistance = 5f;
    private float maxDistance = 15f;
    private AnimationCurve distanceCurve;
    private float lerpDistance = 0.1f;
    
    private float minFOV = 40f;
    private float maxFOV = 80f;
    private AnimationCurve fovCurve;
    private float lerpFOV = 0.1f;
    
    private Vector3 cameraRotation;
    private float rotationMultiplier;
    private float lerpRotation = 0.1f;

    // private float ballSpeed;
    // private float maxBallSpeed;
    
    public NeutralState(CameraScript cameraScript, Transform camHolder, Transform object1, Transform object2, float lerpPosition, float influence, 
        float cameraDistance, float minDistance, float maxDistance, AnimationCurve distanceCurve, float lerpDistance,
        float minFOV, float maxFOV, AnimationCurve fovCurve, float lerpFOV,
        Vector3 cameraRotation, float rotationMultiplier, float lerpRotation) 
        : base(cameraScript)
    {
        this.camHolder = camHolder;
        this.object1 = object1;
        this.object2 = object2;
        this.lerpPosition = lerpPosition;
        this.influence = influence;
        
        this.cameraDistance = cameraDistance;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.distanceCurve = distanceCurve;
        this.lerpDistance = lerpDistance;
        
        this.minFOV = minFOV;
        this.maxFOV = maxFOV;
        this.fovCurve = fovCurve;
        this.lerpFOV = lerpFOV;
        
        this.cameraRotation = cameraRotation;
        this.rotationMultiplier = rotationMultiplier;
        this.lerpRotation = lerpRotation;
    }

    public override void OnEnter()
    {
        Debug.Log("OnEnter : neutral");
    }
    
    public override void OnUpdate()
    {
        targetPoint = Vector3.Lerp(object1.position, object2.position, influence);
        
        float normalizedSpeed = Mathf.InverseLerp(0f, cameraScript.maxBallSpeed, cameraScript.ballSpeed);
        // Distance dynamique
        float curveValueDistance = distanceCurve.Evaluate(normalizedSpeed);
        float targetDistance = Mathf.Lerp(minDistance, maxDistance, curveValueDistance);
        cameraDistance = Mathf.Lerp(cameraDistance, targetDistance, lerpDistance);
        
        // Suivre la target
        camHolder.position = Vector3.Lerp(camHolder.position, targetPoint, lerpPosition);
        cameraScript.transform.position = camHolder.position + cameraScript.transform.forward * -cameraDistance;
        
        var rotation = cameraScript.transform.rotation;
        rotation.eulerAngles = new Vector3(
            Mathf.Lerp(cameraRotation.x, cameraRotation.x + -targetPoint.z * rotationMultiplier, lerpRotation),
            Mathf.Lerp(cameraRotation.y, cameraRotation.y + targetPoint.x * rotationMultiplier, lerpRotation), 
            cameraRotation.z);
        cameraScript.transform.rotation = rotation;
        
        // FOV dynamique
        float curveValueFOV = fovCurve.Evaluate(normalizedSpeed);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, curveValueFOV);
        cameraScript.mainCamera.fieldOfView = Mathf.Lerp(cameraScript.mainCamera.fieldOfView, targetFOV, lerpFOV);
    }
        
    public override void OnExit()
    {
        Debug.Log("OnExit : neutral");

    }
}
