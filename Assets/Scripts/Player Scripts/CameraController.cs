using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstValues
{
    public static class Float
    {
        public const float zero = 0.0f;
        public const float one = 1.0f;
    }

    public static class Int
    {
        public const int zero = 0;
        public const int one = 1;
    }
}

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameSettings settings = null;
    private readonly float yRotationLimit = 75.0f;
    private readonly float ZRotationLimit = 40.0f;
    private float currentYRotation;
    private float currentXrotation;
    private Vector2 mousePosition = Vector2.zero;
    [HideInInspector]
    public bool CanMoveCamera = true;

    private const float localRotationSlerpConstTime = 20.0f;

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
            mousePosition.x = Input.GetAxis("Mouse X") * settings.mouseSensitivity * Time.deltaTime;
            mousePosition.y = Input.GetAxis("Mouse Y") * settings.mouseSensitivity * Time.deltaTime;

            currentXrotation += mousePosition.x;
            currentYRotation += mousePosition.y;
        }
    }

    private void LateUpdate()
    {
           currentYRotation = Mathf.Clamp(currentYRotation, -yRotationLimit, yRotationLimit);
        mousePosition.x = Mathf.Clamp(mousePosition.x, -ZRotationLimit, ZRotationLimit);
        Quaternion xQuaternion = Quaternion.Euler(ConstValues.Float.zero, currentXrotation, ConstValues.Float.zero);
        Quaternion yQuaternion = Quaternion.Euler(-currentYRotation, ConstValues.Float.zero, -mousePosition.x * 3.0f);
       // Quaternion Tst = Input.GetAxis("Mouse X") == 0 || Input.GetAxis("Mouse Y") == 0 ?  Quaternion.Euler(0,0,-CameraDirRotate * 3): Quaternion.Euler(0, 0, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, xQuaternion * yQuaternion , Time.deltaTime * localRotationSlerpConstTime);
    }
}
