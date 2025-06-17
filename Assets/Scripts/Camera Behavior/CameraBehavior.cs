using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraBehavior : MonoBehaviour
{
    [Header("Objects")]
    private Transform camHolder;
    public Transform object1;
    public Transform object2;
    public float lerpPosition;

    [Range(0f, 1f)]
    public float influence = 0.5f;
    [HideInInspector] public Vector3 targetPoint;
    
    [Space (10)]
    [Header("Distance Settings")]
    private float _cameraDistance;
    public float minDistance = 5f;
    public float maxDistance = 15f;
    public AnimationCurve distanceCurve;
    public float lerpDistance = 0.1f;

    [Space (10)]
    [Header("FOV Settings")]
    public float minFOV = 40f;
    public float maxFOV = 80f;
    public AnimationCurve fovCurve;
    public float lerpFOV = 0.1f;
    
    [Space (10)]
    [Header("Rotation Settings")]
    public Vector3 cameraRotation;
    public float rotationMultiplier;
    public float lerpRotation = 0.1f;

    // public bool debugSpeed = false;
    // [Range(0f, 100f)] public float manualSpeed;
    public GameObject ballRB;
    private float ballSpeed;
    private float maxBallSpeed;

    [SerializeField] TextMeshProUGUI ballSpeedText;
    [SerializeField] TextMeshProUGUI fovText;
    [SerializeField] TextMeshProUGUI distanceText;

    private Camera mainCamera;

    void Start()
    {
        camHolder = transform.parent;
        maxBallSpeed = ballRB.GetComponent<BallScript>().maximumLinearVelocity;
        mainCamera = Camera.main;
    }

    void Update()
    {
        targetPoint = Vector3.Lerp(object1.position, object2.position, influence);

        // if (debugSpeed)
        //     ballSpeed = manualSpeed;
        // else
            ballSpeed = ballRB.GetComponent<Rigidbody>().linearVelocity.magnitude;

        float normalizedSpeed = Mathf.InverseLerp(0f, maxBallSpeed, ballSpeed);

        // Distance dynamique
        float curveValueDistance = distanceCurve.Evaluate(normalizedSpeed);
        float targetDistance = Mathf.Lerp(minDistance, maxDistance, curveValueDistance);
        _cameraDistance = Mathf.Lerp(_cameraDistance, targetDistance, lerpDistance);
        
        // Suivre la target
        camHolder.position = Vector3.Lerp(camHolder.position, targetPoint, lerpPosition);
        transform.position = camHolder.position + transform.forward * -_cameraDistance;

        var rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(
            Mathf.Lerp(cameraRotation.x, cameraRotation.x + -targetPoint.z * rotationMultiplier, lerpRotation),
            Mathf.Lerp(cameraRotation.y, cameraRotation.y + targetPoint.x * rotationMultiplier, lerpRotation), 
            cameraRotation.z);
        transform.rotation = rotation;
        
        // FOV dynamique
        float curveValueFOV = fovCurve.Evaluate(normalizedSpeed);
        float targetFOV = Mathf.Lerp(minFOV, maxFOV, curveValueFOV);
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, lerpFOV);

        // Affichage UI
        ballSpeedText.text = $"Speed: {ballSpeed:F2}";
        distanceText.text = $"Distance: {_cameraDistance:F2}";
        fovText.text = $"FOV: {mainCamera.fieldOfView:F2}";
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPoint, 0.5f);
    }
}