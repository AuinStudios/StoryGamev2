// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Door do?
/// </summary>
public sealed class Door : MonoBehaviour
{
    // the layer of the checkbox
    [SerializeField]
    private LayerMask layer = 9;
    // DistanceBetweenPlayerAndDoor
    private Vector3 OffsetForCheckBox;
    // variables
    private Vector3 EndPos;
    private Vector3 StartPos;
    private bool CanActiveOrNot = true;
    private void Start()
    {
        StartPos = transform.position;
        EndPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        OffsetForCheckBox = new Vector3(transform.position.x, transform.position.y - 3, transform.position.z);

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
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(StartPos, EndPos);

        //Debug.DrawLine(transform.position, transform.position + transform.up * -2);
        //  Gizmos.DrawCube(transform.position + transform.up * -2, transform.localScale);
    }
}
