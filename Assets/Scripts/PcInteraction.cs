// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///     What does this PcInteraction do?
/// </summary>
public sealed class PcInteraction : MonoBehaviour
{
    [Header("Variables For ZoomIn Function")]
    [SerializeField]
    private CameraController Camera;
    [SerializeField]
    private CharacterMovement HeadPosAndPlayer;
    [SerializeField]
    private Transform TargetPos;
    private bool IsInPcOrNot = true;
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (Camera.transform.position - transform.position).magnitude < 3 && IsInPcOrNot)
        {
            StartCoroutine(Zoominfunction(Camera.transform, TargetPos, false));
            IsInPcOrNot = false;
        }
        else if ((Camera.transform.position - transform.position).magnitude < 3 && Input.GetKeyDown(KeyCode.Escape) && !IsInPcOrNot)
        {
            // the getchild is for the headpos
           // StopCoroutine(Zoominfunction(Camera.transform, TargetPos, false));
            StartCoroutine(Zoominfunction(Camera.transform, HeadPosAndPlayer.transform.GetChild(0).GetChild(0), true));
            IsInPcOrNot = true;
        }
    }
    public IEnumerator Zoominfunction(Transform Current, Transform Target, bool OnOrOff)
    {
        int i = 0;
        float timelerp = 0;
        if (OnOrOff == false)
        {
            Camera.enabled = OnOrOff;
            HeadPosAndPlayer.enabled = OnOrOff;
            while (Current.position.magnitude <= Target.position.magnitude)
            {
                timelerp += 0.3f * Time.deltaTime;
                Current.position = Vector3.Lerp(Current.position, Target.position, timelerp / 1f);
                Current.rotation = Quaternion.Lerp(Current.rotation, Target.rotation, timelerp / 1f);
                i++;
                yield return new WaitForFixedUpdate();
            }
        }
        else
        {
            while (Current.position.magnitude >= Target.position.magnitude)
            {
                timelerp += 0.3f * Time.deltaTime;
                Current.position = Vector3.Lerp(Current.position, Target.position, timelerp / 1f);
                Current.rotation = Quaternion.Lerp(Current.rotation, Target.rotation, timelerp / 1f);
                i++;
                yield return new WaitForFixedUpdate();
            }
            Camera.enabled = OnOrOff;
            HeadPosAndPlayer.enabled = OnOrOff;
        } 
        Current.position = Target.position;
        Current.rotation = Target.rotation;
    }
}
