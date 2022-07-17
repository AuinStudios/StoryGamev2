// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Door do?
/// </summary>
public sealed class Door : MonoBehaviour
{
    private enum DoorState
    {
        NormalDoor,
        twoOpenedDoor,
        ElevatorDoor
    }
    // the layer of the checkbox
    [SerializeField]
    private DoorState doorstate = DoorState.NormalDoor;
    [SerializeField]
    private LayerMask layer = 9;
    [Header("InbetweenNormalDoor And TwoDoors")]
    private Vector3 OffsetForCheckBox;
    [Header("TwoDoorsVariables")]
    private Transform SecoundDoor;
    private Transform FirstDoor;
    [Header("NormalDoorVariables")]
    private Vector3 EndPos;
    private Vector3 StartPos;
    [Header("cooldownForOpeningDoor")]
    private bool CanActiveOrNot = true;
    private void Start()
    {
        // StartPos.localPosition = doorstate == DoorState.twoOpenedDoor ? : transform.localPosition;
       
        
        if (doorstate == DoorState.twoOpenedDoor)
        {
            FirstDoor = transform.GetChild(0);
            SecoundDoor = transform.GetChild(1);
            OffsetForCheckBox = new Vector3(FirstDoor.position.x + 3, FirstDoor.position.y, FirstDoor.position.z);
        }
        else
        {
         OffsetForCheckBox = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);
         EndPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
         StartPos = transform.position;
        }
        //  SecoundDoor.localPosition = doorstate == DoorState.twoOpenedDoor ? : transform.localPosition;

    }
    public void OpenDoor()
    {
        if (CanActiveOrNot == true)
        {
            StartCoroutine(IenumOpenDoor());
            CanActiveOrNot = false;
        }

    }
    private IEnumerator IenumOpenDoor()
    {
        float TimeUntllDoor = 0.0f;
        float FixTimer = 0.0f;
        switch (doorstate)
        {
            case DoorState.NormalDoor:
                {
                    
                    while (transform.position != EndPos)
                    {
                        TimeUntllDoor += 0.3f * Time.deltaTime;
                        transform.position = Vector3.Lerp(transform.position, EndPos, TimeUntllDoor / 1.0f);
                        yield return new WaitForFixedUpdate();
                    }
                    TimeUntllDoor = 0.0f;

                    while (transform.position != StartPos)
                    {

                        bool checkbox = Physics.CheckBox(OffsetForCheckBox, transform.localScale, transform.rotation, layer);
                        if (checkbox)
                        {

                            TimeUntllDoor = 0;
                            FixTimer += 0.3f * Time.deltaTime;
                            transform.position = Vector3.Lerp(transform.position, EndPos, FixTimer / 1.0f);
                        }
                        else
                        {
                            FixTimer = 0;
                            TimeUntllDoor += 0.3f * Time.deltaTime;
                            transform.position = Vector3.Lerp(transform.position, StartPos, TimeUntllDoor / 1.0f);
                        }
                        //  yield return new WaitUntil(() => (Vector3.Distance(CurrentPosPlayer , currentpos) < 0.7f));

                        yield return new WaitForFixedUpdate();
                    }
                    CanActiveOrNot = true;
                }
                break;
            case DoorState.twoOpenedDoor:
                {

                    doorstate = transform.childCount < 1 ? doorstate = DoorState.NormalDoor : doorstate = DoorState.twoOpenedDoor;
                    // hoow long will the while looop last
                    int i = 0;
                    // check if the player is inside the door to open it --- I DONT KNOW WHY BUT THIS SOMEHOW IS DIFFRENT  -- the checkbox doesnt like local positions for some reason
                    Vector3 CheckBoxPos = new Vector3(FirstDoor.position.x, FirstDoor.position.y, FirstDoor.position.z);
                    // the offset of the pos
                    Vector3 FirstDoorPos = new Vector3(FirstDoor.localPosition.x + 2, FirstDoor.localPosition.y, FirstDoor.localPosition.z);
                    Vector3 SecoundDoorPos = new Vector3(SecoundDoor.localPosition.x - 2, SecoundDoor.localPosition.y, SecoundDoor.localPosition.z);
                    // make the pos  back to normal
                    Vector3 FirstDoorSaveEndPos = new Vector3(FirstDoor.localPosition.x + 2, FirstDoor.localPosition.y, FirstDoor.localPosition.z);
                    Vector3 SecoundDoorSaveEndPos = new Vector3(SecoundDoor.localPosition.x - 2 , SecoundDoor.localPosition.y, SecoundDoor.localPosition.z);
                    // the door opening
                    while (i < 60)
                    {
                        TimeUntllDoor += 0.3f * Time.deltaTime;
                        FirstDoor.localPosition = Vector3.Lerp(FirstDoor.localPosition, FirstDoorPos, TimeUntllDoor / 1.0f);
                        SecoundDoor.localPosition = Vector3.Lerp(SecoundDoor.localPosition, SecoundDoorPos, TimeUntllDoor / 1.0f);
                        i++;
                        yield return new WaitForFixedUpdate();
                    }
                    TimeUntllDoor = 0.0f;
                    i = 0;
                    FirstDoorPos = new Vector3(FirstDoorPos.x - 2, FirstDoorPos.y, FirstDoorPos.z);
                    SecoundDoorPos = new Vector3(SecoundDoorPos.x + 2, SecoundDoorPos.y, SecoundDoorPos.z);
                    while (i < 60)
                    {
                        bool CheckFirstDoor = Physics.CheckBox(CheckBoxPos, FirstDoor.localScale * 1.1f, FirstDoor.rotation, layer);
                        if (CheckFirstDoor)
                        {

                            i = 0;
                            TimeUntllDoor = 0;
                            FixTimer += 0.3f * Time.deltaTime;
                            FirstDoor.localPosition = Vector3.Lerp(FirstDoor.localPosition, FirstDoorSaveEndPos, FixTimer / 1.0f);
                            SecoundDoor.localPosition = Vector3.Lerp(SecoundDoor.localPosition, SecoundDoorSaveEndPos, FixTimer / 1.0f);
                        }
                        else
                        {
                           
                            FixTimer = 0;
                            TimeUntllDoor += 0.3f * Time.deltaTime;
                            FirstDoor.localPosition = Vector3.Lerp(FirstDoor.localPosition, FirstDoorPos, TimeUntllDoor / 1.0f);
                            SecoundDoor.localPosition = Vector3.Lerp(SecoundDoor.localPosition, SecoundDoorPos, TimeUntllDoor / 1.0f);
                            i++;
                        }
                        //  yield return new WaitUntil(() => (Vector3.Distance(CurrentPosPlayer , currentpos) < 0.7f));

                        yield return new WaitForFixedUpdate();
                    }
                    // makes the door openable again
                    CanActiveOrNot = true;
                }
                break;
            case DoorState.ElevatorDoor:
                {

                }
                break;
            default:
                break;
        }

    }
   //  private void OnDrawGizmos()
   //  {
   //      //Gizmos.DrawLine(StartPos, EndPos);
   // 
   //      //Debug.DrawLine(transform.position, transform.position + transform.up * -2);
   //      if(doorstate == DoorState.twoOpenedDoor)
   //     {
   //      //   Gizmos.matrix = transform.GetChild(0).localToWorldMatrix;
   //       Gizmos.DrawCube(FirstDoor.TransformPoint(FirstDoor.position.x + 0.9f, FirstDoor.position.y, FirstDoor.position.z), FirstDoor.localScale);
   //     }
   //       
   //  }
}
