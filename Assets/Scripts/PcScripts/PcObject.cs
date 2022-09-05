// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this PcObject do?
/// </summary>
public sealed class PcObject : MonoBehaviour
{
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
        CharacterMovement.Instance.enabled = false;
        cam.enabled = false;
        LeanTween.move(GetHead, CameraPostion.position, 60.0f * Time.deltaTime).setOnComplete(()=> 
        {
            Pcmanager.Instance.CanClick = true;
        });
        LeanTween.rotateLocal(GetHead, new Vector3(-20, 0, 0) , 60.0f * Time.deltaTime);
    }
    private void ExitThing()
    {
        LeanTween.moveLocal(GetHead,Vector3.zero, 30.0f * Time.deltaTime).setEaseOutSine().setOnComplete(() =>
        {
            Pcmanager.Instance.CanClick = false;
            CharacterMovement.Instance.enabled = true;
            cam.enabled = true;
        }); ;
    }

}
