using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MoveTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Cette fonction est appelée automatiquement via PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed;
            rb.linearVelocity = movement;
        }
        else
        {
            // Si pas de Rigidbody, fallback déplacement simple
            transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.fixedDeltaTime);
        }
    }
}