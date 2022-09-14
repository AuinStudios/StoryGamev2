// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this PcObject do?
/// </summary>
public sealed class PcObject : MonoBehaviour
{
    //[Header("Grid propertys")]
    //[SerializeField]
    //private Grid gridstuff;
    [Header("Camera And Positon propertys")]
    [SerializeField]
    private Transform CameraPostion;
    [SerializeField]
    private GameObject GetHead;
    [SerializeField]
    private CameraController cam;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pcmanager.Instance.InteractPc += enterthing;
            Pcmanager.Instance.InteractPc -= ExitThing;

            Pcmanager.Instance.CanOpenPc = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pcmanager.Instance.InteractPc -= enterthing;
            Pcmanager.Instance.InteractPc += ExitThing;
            Pcmanager.Instance.CanOpenPc = false;
            
        }
    }
    private void enterthing()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;


        CharacterMovement.Instance.enabled = false;
        cam.enabled = false;
        LeanTween.move(GetHead, CameraPostion.position, 60.0f * Time.deltaTime).setOnComplete(()=> 
        {
            Pcmanager.Instance.CanClick = true;
        });
        StartCoroutine(enterthingrotation());
    }
    private IEnumerator enterthingrotation()
    {
        int i = 0;
        while(i < 60)
        {
            GetHead.transform.rotation = Quaternion.Lerp(GetHead.transform.rotation, CameraPostion.rotation, 10.0f * Time.deltaTime);
            i++;
            yield return new WaitForFixedUpdate();
        }
    }

    private void ExitThing()
    {

        LeanTween.moveLocal(GetHead,Vector3.zero, 30.0f * Time.deltaTime).setEaseOutSine().setOnComplete(() =>
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Pcmanager.Instance.CanClick = false;
            CharacterMovement.Instance.enabled = true;
            cam.enabled = true;
        }); ;
        LeanTween.rotateLocal(GetHead, Vector3.zero, 30.0f * Time.deltaTime);
    }

    
}
