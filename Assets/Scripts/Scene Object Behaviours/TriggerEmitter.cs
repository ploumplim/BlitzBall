using UnityEngine;
using UnityEngine.Events;

public class TriggerEmitter : MonoBehaviour
{
    public UnityEvent<Collider> OnTriggerEnterEvent; // Event to invoke when the trigger is entered
    
    private void OnTriggerEnter(Collider other)
    {
        // Invoke the event when a collider enters the trigger
        OnTriggerEnterEvent?.Invoke(other);
        Debug.Log("Trigger entered by: " + other.gameObject.name);
    }
}
