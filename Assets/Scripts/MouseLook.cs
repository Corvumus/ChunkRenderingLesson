using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerTransform;

    float xRotation = 0f;

    private InputAction lookIA;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        lookIA = InputSystem.actions.FindAction("Look");

        lookIA.performed += GetLookInput;
    }

    public void GetLookInput(CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        float mouseX = input.x * mouseSensitivity * Time.deltaTime;
        float mouseY = input.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerTransform.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
