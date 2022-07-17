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
    [HideInInspector]
    public bool CanSway = true;
    // Update is called once per frame
    void Update()
    {
        if(CanSway == true)
        {
          // inputs -----------------------------------------------------------
          x = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
          y = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;
        }
        else
        {
            x = 0;
            y = 0;
        }
        // transfer input to move the rotation and poition of the gameobject ----------------------------------------
        Quaternion rotations = new Quaternion(-y, -x, transform.localRotation.z, transform.localRotation.w);
        Vector3 positions = new Vector3(x * 1.5f, -y * 1.1f, transform.localPosition.z);
        // lerp the gameobjects rotation and position towards the mouse input value  --------------------------------------------
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rotations, Time.deltaTime * smoothness);
        transform.localPosition = Vector3.Lerp(transform.localPosition, positions, Time.deltaTime * smoothness);
    }
}
