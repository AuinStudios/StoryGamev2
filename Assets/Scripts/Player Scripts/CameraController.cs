using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameSettings settings = null;

    private readonly float yRotationLimit = 75.0f;
    private readonly float ZRotationLimit = 40.0f;
    private float currentYRotation;
    private float currentXrotation;
    private Vector2 mousePosition = Vector2.zero;
    private float CameraDirRotate;
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
            mousePosition.x = Input.GetAxis("Mouse X") * settings.mouseSensitivity;
            mousePosition.y = Input.GetAxis("Mouse Y") * settings.mouseSensitivity;

            currentXrotation += mousePosition.x;
            currentYRotation += mousePosition.y;
        }
        CameraDirRotate = Input.GetAxisRaw("Horizontal");
        currentYRotation = Mathf.Clamp(currentYRotation, -yRotationLimit, yRotationLimit);
        mousePosition.x = Mathf.Clamp(mousePosition.x, -ZRotationLimit, ZRotationLimit);
        Quaternion xQuaternion = Quaternion.Euler(0, currentXrotation, 0);
        Quaternion yQuaternion = Quaternion.Euler(-currentYRotation, 0,-mousePosition.x * 3);
       // Quaternion Tst = Input.GetAxis("Mouse X") == 0 || Input.GetAxis("Mouse Y") == 0 ?  Quaternion.Euler(0,0,-CameraDirRotate * 3): Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, xQuaternion * yQuaternion , Time.deltaTime * 20.0f);
    }
}
