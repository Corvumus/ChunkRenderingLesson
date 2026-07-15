using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    Vector3 movementVector;
    private Vector2 inputVector;
    private Vector3 velocity;
    private float gravity = -9.81f;
    public float jumpHeight = 3f;
    public CharacterController controller;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundLayerMask;

    private bool isGrounded;

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        movementVector = transform.right * inputVector.x + transform.forward * inputVector.y;

        controller.Move(moveSpeed * Time.deltaTime * movementVector);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    public void GetInputMovement(CallbackContext context)
    { 
        inputVector = context.ReadValue<Vector2>();
    }

    public void GetInputJump(CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
