using System;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class CameraScript : MonoBehaviour
{
    public PlayableDirector director;
    
    public CameraState currentState { get; set; }
    [SerializeField, ReadOnly] private string currentStateName;
    
    [HideInInspector] public Camera mainCamera;
    public GameObject ballRB;
    public float ballSpeed;
    public float maxBallSpeed;
    
    #region State 
    
    public EntryStageState EntryStageState;    
    public NeutralState NeutralState;
    public ExitStageState ExitStageState;
   
    #endregion
    
    #region StateNeutral 
    
    [Header("Objects")]
    public Transform camHolder;
    public Transform object1;
    public Transform object2;
    public float lerpPosition;

    [Range(0f, 1f)]
    public float influence = 0.5f;
    private Vector3 targetPoint;
    
    [Space (10)]
    [Header("Distance Settings")]
    public float cameraDistance;
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
    
    #endregion
    
    #region UI 
    [SerializeField] TextMeshProUGUI ballSpeedText;
    [SerializeField] TextMeshProUGUI fovText;
    [SerializeField] TextMeshProUGUI distanceText;
    #endregion

    private void Awake()
    {
        EntryStageState = new EntryStageState(this, director);
        NeutralState = new NeutralState(this,  camHolder,  object1,  object2,  lerpPosition,  influence, 
             cameraDistance, minDistance, maxDistance, distanceCurve, lerpDistance,
             minFOV,  maxFOV, fovCurve, lerpFOV,
             cameraRotation, rotationMultiplier, lerpRotation);
        ExitStageState = new ExitStageState(this);
    }

    private void Start()
    {
        camHolder = transform.parent;
        maxBallSpeed = ballRB.GetComponent<BallScript>().maximumLinearVelocity;
        mainCamera = Camera.main;
        
        ChangeState(EntryStageState);
    }

    public void ChangeState(CameraState newState)
    {
        currentState?.OnExit();
        currentState = newState;
        currentStateName = currentState.ToString();
        currentState.OnEnter();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            ChangeState(NeutralState);
        
        if(director.state != PlayState.Playing && currentState == EntryStageState)
            ChangeState(NeutralState);
        
        ballSpeed = ballRB.GetComponent<Rigidbody>().linearVelocity.magnitude;
        
        currentState?.OnUpdate();
        
        // Affichage UI
        ballSpeedText.text = $"Speed: {ballSpeed:F2}";
        distanceText.text = $"Distance: {cameraDistance:F2}";
        fovText.text = $"FOV: {mainCamera.fieldOfView:F2}";
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(targetPoint, 0.5f);
    }
}
