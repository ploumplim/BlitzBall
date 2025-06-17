using UnityEngine;

public class BallCollision : MonoBehaviour
{
    private Rigidbody ballRB;
    private float ballSpeed;
    private float maxBallSpeed;
    
    [Space (10)]
    [Header("Shake Intensity Settings")]
    public float minShakeIntensity = 0f;
    public float maxShakeIntensity = 0.1f;
    public AnimationCurve shakeIntensityCurve;
    
    [Space (10)]
    [Header("Shake Duration Settings")]
    public float minShakeDuration = 0f;
    public float maxShakeDuration = 0.1f;
    public AnimationCurve shakeDurationCurve;
    
    public ScreenShake screenShake;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRB = GetComponent<Rigidbody>();
        maxBallSpeed = ballRB.GetComponent<BallScript>().maximumLinearVelocity;
    }

    public void TriggerShakeOnCollisison()
    {
        ballSpeed = ballRB.GetComponent<Rigidbody>().linearVelocity.magnitude;
        
        float normalizedSpeed = Mathf.InverseLerp(0f, maxBallSpeed, ballSpeed);
        
        float curveValueShakeIntensity = shakeIntensityCurve.Evaluate(normalizedSpeed);
        float shakeIntensity = Mathf.Lerp(minShakeIntensity, maxShakeIntensity, curveValueShakeIntensity);
        
        float curveValueShakeDuration = shakeDurationCurve.Evaluate(normalizedSpeed);
        float shakeDuration = Mathf.Lerp(minShakeDuration, maxShakeDuration, curveValueShakeDuration);
        
        screenShake.TriggerShake(shakeDuration,shakeIntensity);
    }
}
