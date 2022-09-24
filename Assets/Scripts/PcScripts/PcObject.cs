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
    [SerializeField]
    private Grid grid;
    [Header("Pc Click And Drag Propertys")]
    private Transform temphold;
    private Collider tempcollider;
    private SpriteRenderer ParentScreencolidder;
    [SerializeField]
    private LayerMask mask;
    private float cellsize = 0.06f;
    private Transform getotherapplication;
    // placeing the objects in a grid
    private Vector3 mOffset;
    private float mZCoord;
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
        Pcmanager.Instance.InteractPc += getmousepos;

        CharacterMovement.Instance.enabled = false;
        cam.enabled = false;
        LeanTween.move(GetHead, CameraPostion.position, 60.0f * Time.deltaTime).setOnComplete(()=> 
        {
            Pcmanager.Instance.CanClick = true;
            
        });
        StartCoroutine(enterthingrotation());
    }

    private void letgoofobject()
    {
        temphold.position = translatetocell(temphold.position);
        temphold.localPosition = new Vector3(temphold.localPosition.x, temphold.localPosition.y, 0);
        tempcollider.enabled = true;
        tempcollider = null;
        temphold = null;
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
        Pcmanager.Instance.InteractPc -= getmousepos;
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
    private void getmousepos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask , QueryTriggerInteraction.Collide))
        {
            temphold = raycas.transform;
            ParentScreencolidder = raycas.transform.parent.GetComponent<SpriteRenderer>();
            tempcollider = raycas.collider;
            tempcollider.enabled = false;
            mZCoord = Camera.main.WorldToScreenPoint(temphold.position).z;
            mOffset = temphold.position - mouseposinput();
            StartCoroutine(bringapplicationtomousepos());
        }
        
    }

    private IEnumerator bringapplicationtomousepos()
    {
        while (Input.GetKey(KeyCode.Mouse0))
        {
            temphold.position = mouseposinput() + mOffset;
            temphold.localPosition = clamp(temphold.localPosition);
            yield return new WaitForFixedUpdate();
        }
       StartCoroutine(checkforotherspot());

    }
    private IEnumerator checkforotherspot()
    {

        if (Physics.BoxCast(temphold.position + new Vector3(0, 0, 0.3f), new Vector3(cellsize, cellsize, cellsize) / 3, temphold.forward, out RaycastHit raycas, Quaternion.identity, 999f, mask))
        {
            raycas = temphold == raycas.transform ? new RaycastHit() : raycas;
            getotherapplication = raycas.transform;
            float e = translatetocell(raycas.transform.position).y;
            raycas.transform.position = e < 7.22f ? translatetocell(raycas.transform.position + new Vector3(0.03f, ConstValues.Float.one, ConstValues.Float.zero)) : translatetocell(raycas.transform.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));

            raycas.transform.localPosition = clamp(raycas.transform.localPosition);
            raycas.transform.localPosition = new Vector3(raycas.transform.localPosition.x, raycas.transform.localPosition.y, 0.0f);
            raycas.collider.enabled = false;
            bool isslottaken = false;
            while (isslottaken == false)
            {
                if (Physics.Raycast(getotherapplication.position + new Vector3(0, 0, 0.3f), getotherapplication.forward, 999f, mask))
                {
                    getotherapplication.transform.position = translatetocell(getotherapplication.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));
                    getotherapplication.localPosition = new Vector3(getotherapplication.localPosition.x, getotherapplication.localPosition.y, ConstValues.Float.zero);

                }
                else
                {
                    raycas.collider.enabled = true;
                    
                    isslottaken = true;
                }
                //yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
            }
            letgoofobject();
        }
        else
        {
            letgoofobject();
        }
    }


    private Vector3 translatetocell(Vector3 pos)
    {
        Vector3Int cell = grid.WorldToCell(pos);
        pos = grid.GetCellCenterWorld(cell);
        return pos;
    }
    private Vector3 clamp(Vector3 pos)
    {
        Vector3 clamp = pos;
        clamp.x = Mathf.Clamp(clamp.x, -ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.4f, ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.43f);
        clamp.y = Mathf.Clamp(clamp.y, -ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y / 0.95f, ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y / 0.92f);
        return clamp;
    }
    private Vector3 mouseposinput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousepos);
    }
}
