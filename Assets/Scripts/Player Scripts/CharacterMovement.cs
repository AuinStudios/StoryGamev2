using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterMovement : MonoBehaviour
{
    //[Header("Gravity properties")]
    //[SerializeField]
    //private LayerMask gravityLayers = 0;
    //[SerializeField]
    //private Transform groundCheck = null;
    //[SerializeField]
    //private float gravityMultiplier = 20.0f;
    //private bool IsGrounded = false;
    //
    //[Header("Player Controller")]
    //[SerializeField]
    //private CharacterController player = null;
    //[SerializeField]
    //private float movementSpeed = 5.0f;
    //// private float MaxSpeed = 20;
    //
    //[Header("Camera References")]
    //[SerializeField]
    //private new Transform camera = null;
    //[SerializeField]
    //private Transform cameraTarget = null;
    //
    //private Vector2 mouseXZ = Vector2.zero;
    //private Vector2 moveXZ = Vector2.zero;
    //private Vector3 velocity = Vector3.zero; // will affect X and Z
    //private Vector2 gravity = Vector2.zero; // will affect Y
    //
    //private void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}
    //private void LateUpdate()
    //{
    //    camera.position = cameraTarget.position;
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //    //Inputs ------------------------------------------------------
    //    moveXZ = new(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    //    //moveY = Input.GetAxis("Vertical");
    //    //moveX = Input.GetAxis("Horizontal");
    //
    //    mouseXZ = new(Input.GetAxis("Mouse X") * MouseSenstivity, Input.GetAxis("Mouse Y") * MouseSenstivity);
    //    //float Mouse_Y = Input.GetAxis("Mouse Y") * MouseSenstivity;
    //    //float Mouse_X = Input.GetAxis("Mouse X") * MouseSenstivity;
    //    // Debug.Log(VerticalAndHorizontal);
    //    moveXZ.y -= mouseXZ.y;
    //    moveXZ.x += mouseXZ.x;
    //    HorizontalMouse -= Mouse_Y;
    //    VerticalMouse += Mouse_X;
    //
    //    if (Input.GetKey(KeyCode.LeftShift))
    //    {
    //        //MaxSpeed = 10;
    //        movementSpeed = 10f;
    //    }
    //    else if (Input.GetKeyUp(KeyCode.LeftShift))
    //    {
    //        // MaxSpeed = 5;
    //        movementSpeed = 5f;
    //    }
    //
    //    // Movement -------------------------------------------------------------------------------------------
    //
    //    velocity = cameraTarget.right * moveX + transform.forward * moveY;
    //    //VerticalAndHorizontal *= 0.95f;
    //
    //    // rotate camera -----------------------------------------------------------------------------------------------------------------
    //    cameraTarget.rotation = Quaternion.Euler(HorizontalMouse, VerticalMouse, 0);
    //    transform.rotation = Quaternion.Euler(0, VerticalMouse, 0);
    //    // Clamp  ---------------------------------------------------------------------------------------
    //    HorizontalMouse = Mathf.Clamp(HorizontalMouse, -80, 80);
    //    //VerticalAndHorizontal =  Vector3.ClampMagnitude(VerticalAndHorizontal, MaxSpeed);
    //    // Move Player -------------------------------------------------------------------------------------------------------------------------------
    //    player.Move(velocity * movementSpeed * Time.deltaTime);
    //
    //    // Check If Player is Grounded ---------------------------------------------------------------------------------------------------------------
    //    IsGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, gravityLayers, QueryTriggerInteraction.Ignore);
    //
    //    if (!IsGrounded)
    //    {
    //        gravity.y -= Time.deltaTime * gravityMultiplier;
    //        player.Move(gravity * Time.deltaTime);
    //    }
    //    else if (gravity.y != 0)
    //    {
    //        gravity.y = 0;
    //    }
    //}

    [Header("Player Controller")]
    [SerializeField]
    private CharacterController player = null;
    [SerializeField]
    private float walkSpeed = 5.0f;
    [SerializeField]
    private float runSpeedMultiplier = 2.0f;
    private float currentRunSpeedMultiplier = 1.0f;
    // private float MaxSpeed = 20;
    
    //private Vector2 mouseXZ = Vector2.zero;
    private Vector2 direction = Vector2.zero;
    private Vector3 move = Vector3.zero;

    [Header("Gravity data")]
    [SerializeField]
    private float gravityForce = -9.81f;
    [SerializeField]
    private Transform groundCheck = null;
    [SerializeField]
    private LayerMask groundLayer = 0;
    [SerializeField]
    private float groundCheckRadius = 0.3f;
    private bool isGrounded = false;
    private Vector3 velocity = Vector3.zero; // will affect X and Z
    [Header("Camera References")]
    [SerializeField]
    private  Transform mainCam = null;
    [SerializeField]
    private Transform cameraTarget = null;
    [SerializeField]
    private Transform ori;
    [Header("Stamina Ui")]
    [SerializeField]
    private Image SliderImage;

    private bool cansprint = true;

    private Quaternion OriQuaternion;
    // Update is called once per frame
    void Update()
    {
        //Inputs --------------------------------------------------------------------------------------------
        direction = new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //currentRunSpeedMultiplier = Input.GetButton("Run") ? runSpeedMultiplier : 1.0f;
        //SliderImage.fillAmount = Input.GetButton("Run") ? SliderImage.fillAmount -= 1 : SliderImage.fillAmount += 0.5f;
        if (Input.GetButton("Run") && cansprint == true  )
        {
            currentRunSpeedMultiplier = runSpeedMultiplier;

            SliderImage.fillAmount = direction.x != 0 || direction.y != 0 ? SliderImage.fillAmount -= 0.1f * Time.deltaTime : SliderImage.fillAmount += 0.02f * Time.deltaTime;
            cansprint = SliderImage.fillAmount < 0.05f ? cansprint = false :cansprint = true;
        }
        else if(SliderImage.fillAmount < 1)
        {
            currentRunSpeedMultiplier = 1.0f;
            SliderImage.fillAmount += 0.02f * Time.deltaTime;
        
            cansprint = SliderImage.fillAmount > 0.15f ? cansprint = true : cansprint = false;    
        }
        // Movement -------------------------------------------------------------------------------------------
        move = currentRunSpeedMultiplier * walkSpeed * (Vector3.Normalize(mainCam.right * direction.x + ori.forward * direction.y));
        player.Move(move * Time.deltaTime);

        // CamDir----------------------------------------------------------------------------------------------
        OriQuaternion = new Quaternion(0, mainCam.rotation.y , 0 , mainCam.rotation.w);
        ori.rotation = OriQuaternion;
        // Gravity -------------------------------------------------------------------------------------------
        velocity.y = GetGravityForce();
        player.Move(velocity * Time.deltaTime);
        // Camera Target ---------------------------
        mainCam.position = cameraTarget.position;
    }
   // public void Stamina()
   // {
   //     SliderImage.color = Color.Lerp(Color.white, Color.black, SliderImage.fillAmount / 100);
   // }
    private float GetGravityForce()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && velocity.y < 0.0f)
        {
            velocity.y = gravityForce;
        }
        velocity.y += gravityForce * Time.deltaTime;

        return velocity.y;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
