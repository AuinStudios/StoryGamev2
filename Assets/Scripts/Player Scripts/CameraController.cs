using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameSettings settings = null;

    private readonly float yRotationLimit = 75.0f;
    private float currentYRotation;
    private Vector2 mousePosition = Vector2.zero;
    [HideInInspector]
    public bool CanMoveCamera = true;
    // Start is called before the first frame update
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    // Update is called once per frame
    private void Update()
    {
        if (CanMoveCamera == true)
        {
            mousePosition.x += Input.GetAxis("Mouse X") * settings.mouseSensitivity;
            mousePosition.y = Input.GetAxis("Mouse Y") * settings.mouseSensitivity;

            
            currentYRotation += mousePosition.y;
        }
        currentYRotation = Mathf.Clamp(currentYRotation, -yRotationLimit, yRotationLimit);
        Quaternion xQuaternion = Quaternion.Euler(0, mousePosition.x, 0);
        Quaternion yQuaternion = Quaternion.Euler(-currentYRotation, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, xQuaternion * yQuaternion, Time.deltaTime * 10.0f);
    }
}
