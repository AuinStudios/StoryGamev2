// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
///     What does this DoorV2 do?
/// </summary>
public sealed class DoorV2 : MonoBehaviour
{
    

    [System.Serializable]
    private class DoorProperties
    {
        public enum Direction
        {
            up,
            down,
            right,
            left
        }

        public GameObject door;
        public Direction direction;
        public float amount;
        public float time;
        [HideInInspector]
        public Vector3 ogpos;
        [HideInInspector]
        public Vector3 Endpos;
    }
    [SerializeField]
    private DoorProperties[] doorProperties;
    [SerializeField]
    private int LevelDoorClearance = 0;
    [Space(5.0f)]
    [SerializeField]
    private Transform interactHoverTransform;
    [SerializeField]
    private TextMeshProUGUI interactDialog;
    private LTDescr Timepassed;
    private void Start()
    {
        foreach(DoorProperties door in doorProperties)
        {
            door.ogpos = door.door.transform.localPosition;
            door.Endpos = door.door.transform.localPosition + GetDirection(door) * door.amount;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DoorController.Instance.GetTag) && CharacterMovement.Instance.LevelClearance >= LevelDoorClearance)
        {
            DoorController.Instance.InteractDoor += OpenDoor;
            DoorController.Instance.InteractDoor -= CloseDoor;
            if (DoorController.Instance.CanOpenDoor != true)
            {
                ShowInteractDialog();

            }
            else
            {
                DoorController.Instance.invoketheevent();
            }
                 
           
        }
    }

    private void ShowInteractDialog()
    {
        LeanTween.scale(interactHoverTransform.gameObject, Vector3.one, 30.0f * Time.deltaTime).setEaseOutBounce();

        interactDialog.text = "Press 'E'";
        LeanTween.alphaText(interactDialog.rectTransform, ConstValues.Float.one, 30.0f * Time.deltaTime);
    }

    private void OpenDoor()
    {
        //Debug.Log("Opening doors");

        OpenAlldoors(() =>
        {
           // Debug.Log("Done opening all doors");
            
            DoorController.Instance.InteractDoor -= OpenDoor;
        });
    }
    private void OpenAlldoors(System.Action onComplete)
    {
        //Debug.Log("Opening all doors");
        Timepassed = null;
        foreach (DoorProperties doorProperty in doorProperties)
        {
            LeanTween.cancel(doorProperty.door);
          LTDescr animation =  LeanTween.moveLocal(doorProperty.door, doorProperty.Endpos, doorProperty.time * Time.deltaTime)
                     .setEaseOutSine()
                     .setOnStart(() =>
                     {
                        
                         HideInteractDialog();
                         DoorController.Instance.CanOpenDoor = true;

                     });
        }

        onComplete?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(DoorController.Instance.GetTag) && CharacterMovement.Instance.LevelClearance >= LevelDoorClearance)
        {
            HideInteractDialog();
           
             DoorController.Instance.InteractDoor -= OpenDoor;
             DoorController.Instance.InteractDoor += CloseDoor;
            
            if(DoorController.Instance.CanOpenDoor == true)
            {
                DoorController.Instance.invoketheevent();
            }
            
        }
    }
    
    private void HideInteractDialog()
    {
        LeanTween.scale(interactHoverTransform.gameObject, Vector3.zero, 30.0f * Time.deltaTime).setOnComplete(() => 
        {
            interactDialog.text = "";
        });
    }

    private void CloseDoor()
    {
        //Debug.Log("Closing doors");
        
        ClosingAllDoors(() =>
        {
            DoorController.Instance.InteractDoor -= CloseDoor;
        });
    }
    private void ClosingAllDoors(System.Action onComplete)
    {
       // Debug.Log("Closing all doors");

        foreach (DoorProperties doorProperty in doorProperties)
        {

            LeanTween.cancel(doorProperty.door);
            Timepassed = LeanTween.moveLocal(doorProperty.door, doorProperty.ogpos, doorProperty.time * Time.deltaTime)
                       .setEaseOutSine();
            

            StartCoroutine(makedoornotopenwhenclose());
            
        }
        onComplete?.Invoke();
    }
    private IEnumerator makedoornotopenwhenclose()
    {
       
        while(DoorController.Instance.CanOpenDoor == true && Timepassed != null )
        {
          float remaintime = Timepassed.time - Timepassed.passed;
            if(remaintime < 0.8f )
            {
             DoorController.Instance.CanOpenDoor = false;
                Debug.Log("Close");
            }
            yield return new WaitForFixedUpdate();
        }
    }
    private Vector3 GetDirection(DoorProperties doorProperty)
    {
        Vector3 direction = Vector3.zero;
        switch (doorProperty.direction)
        {
            case DoorProperties.Direction.up:
                direction = Vector3.up;
                break;
            case DoorProperties.Direction.down:
                direction = Vector3.down;
                break;
            case DoorProperties.Direction.right:
                direction = Vector3.right;
                break;
            case DoorProperties.Direction.left:
                direction = Vector3.left;
                break;
        }
        return direction;
    }
}
