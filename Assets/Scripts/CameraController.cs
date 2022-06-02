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

    private Transform player;

    // Start is called before the first frame update
    private void Start()
    {
        player = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        mousePosition.x = Input.GetAxis("Mouse X") * settings.mouseSensitivity;
        mousePosition.y = Input.GetAxis("Mouse Y") * settings.mouseSensitivity;

        currentYRotation = Mathf.Clamp(currentYRotation, -yRotationLimit, yRotationLimit);
        currentYRotation += mousePosition.y;

        Quaternion xQuaternion = Quaternion.AngleAxis(mousePosition.x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(currentYRotation, Vector3.left);

        transform.localRotation = Quaternion.Lerp(transform.localRotation, xQuaternion * yQuaternion, Time.deltaTime * 10.0f);

        player.Rotate(Vector3.up * mousePosition.x);
    }
}
