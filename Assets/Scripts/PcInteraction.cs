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
    private CameraController CameraEnable;
    [SerializeField]
    private CharacterMovement Player;
    [SerializeField]
    private Transform maincam;
    [SerializeField]
    private Transform CameraOrigin;
    [SerializeField]
    private Transform Target;
    private bool IsInPcOrNot = true;
    // Update is called once per frame

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (CameraEnable.transform.position - transform.position).magnitude < 3 && IsInPcOrNot)
        {
            StartCoroutine(Zoominfunction(maincam, Target, false ));
            
            IsInPcOrNot = false;
        }
    }
    private IEnumerator Zoominfunction(Transform Current, Transform target, bool OnOrOff)
    {
        bool i = false;
        float timelerp = 0;
        CameraEnable.transform.position = Player.cameraTarget.position;
        if (OnOrOff == false)
        {
            CameraEnable.enabled = OnOrOff;
            Player.enabled = OnOrOff;
        }
        while (timelerp  < 0.3f)
        {
            timelerp += 0.3f * Time.deltaTime;
            Current.position = Vector3.Lerp(Current.position, target.position, timelerp / 1f);
            Current.rotation = Quaternion.Lerp(Current.rotation, target.rotation, timelerp / 1f);
            yield return new WaitForFixedUpdate();
        }
        
        while (i == false)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(Zoominfunction(maincam, CameraOrigin, true ));
                IsInPcOrNot = true;
                i = true;
            }
            else if(OnOrOff == true)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        
        
        if (OnOrOff == true)
        {
            CameraEnable.enabled = OnOrOff;
            Player.enabled = OnOrOff;
        }

        Current.position = target.position;
        Current.rotation = target.rotation;
    }
}
