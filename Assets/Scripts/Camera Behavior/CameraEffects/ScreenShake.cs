using UnityEngine;
using UnityEngine.Serialization;

public class ScreenShake : MonoBehaviour
{
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;

    void Update()
    {
        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition += shakeOffset;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}
