using JetBrains.Annotations;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{

    public float mouseSensitivity = 500f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;
    
    void Start()
    {
        // Locking the cursor and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    
    void Update()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate around x axis
        xRotation -= mouseY;
        // Block the rotation
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        
        // Rotate around y axis
        yRotation += mouseX;

        // Apply rotation 
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
