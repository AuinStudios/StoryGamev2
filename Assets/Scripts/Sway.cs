// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Sway do?
/// </summary>
public sealed class Sway : MonoBehaviour
{
    private float sensitivity = 1.3f;
    private float smoothness = 10f;
    private float x, y;
    private float XAnimCurve, YAnimCurve;
    [Header("Animation Curves")]
    [SerializeField]
    private AnimationCurve SwayX;
    [SerializeField]
    private AnimationCurve SwayY;
    [HideInInspector]
    public bool CanSway = true;
    private Vector3 SmoothV;
    // Update is called once per frame
    void Update()
    {
        if(CanSway == true)
        {
          // inputs -----------------------------------------------------------
          x = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
          y = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;
          
            XAnimCurve = SwayX.Evaluate(x);
            YAnimCurve = SwayY.Evaluate(y);
        }
        else
        {
            x = 0;
            y = 0;
        }
        // transfer input to move the rotation and poition of the gameobject ----------------------------------------
      
        // lerp the gameobjects rotation and position towards the mouse input value  --------------------------------------------
     
    }


    private void FixedUpdate()
    { 
        Quaternion rotations = new Quaternion(y, -x, transform.localRotation.z, transform.localRotation.w);
        Vector3 positions = new Vector3(-XAnimCurve, -YAnimCurve, transform.localPosition.z);
          transform.localRotation = Quaternion.Slerp(transform.localRotation, rotations, Time.deltaTime * smoothness);
        // transform.localPosition = Vector3.Lerp(transform.localPosition, positions, Time.deltaTime * smoothness);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, positions, ref SmoothV,0.1f);
    }
}
