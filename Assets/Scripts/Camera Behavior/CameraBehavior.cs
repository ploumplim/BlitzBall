using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public CinemachineCamera vcam;
    private CinemachineComponentBase componentBase;


    [Space]
    [Header("FOV")]
    
    [SerializeField] private float _cameraDistance;
    public float minDistance;
    public float maxDistance;
    public AnimationCurve distanceCurve;
    public float lerpDistance;
    
    public float minFOV;
    public float maxFOV;
    public AnimationCurve fovCurve;
    public float lerpFOV;
    
    public GameObject ballRB;
    private float ballSpeed;
    private float maxBallSpeed;
    [SerializeField] TextMeshProUGUI ballSpeedText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        componentBase = vcam.GetCinemachineComponent(CinemachineCore.Stage.Body);
        maxBallSpeed = ballRB.GetComponent<BallScript>().maximumLinearVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        ballSpeed = ballRB.GetComponent<Rigidbody>().linearVelocity.magnitude;

        ballSpeedText.text = ballSpeed.ToString();
        
        Debug.Log(componentBase);
        
        if (componentBase is CinemachinePositionComposer transposer)
        {
            float normalizedSpeed = Mathf.InverseLerp(0f, maxBallSpeed , ballSpeed);
            
            float curveValueDistance = distanceCurve.Evaluate(normalizedSpeed);
            float curveValueFOV = distanceCurve.Evaluate(normalizedSpeed);
            
            float currentDistance = transposer.CameraDistance;
            float targetDistance = Mathf.Lerp(minDistance, maxDistance, curveValueDistance);
            
            transposer.CameraDistance = Mathf.Lerp(currentDistance, targetDistance, lerpDistance);
            _cameraDistance = transposer.CameraDistance;
            
            float currentFOV = vcam.Lens.FieldOfView;
            float targetFOV = Mathf.Lerp(minFOV, maxFOV, curveValueFOV);
            
            vcam.Lens.FieldOfView = Mathf.Lerp(currentFOV, targetFOV, lerpFOV);
            
            Debug.Log(ballSpeed);
        }
    }
}
