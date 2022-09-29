// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this PcObject do?
/// </summary>
public sealed class PcObject : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField]
    private CanvasGroup staminaandhealth;
    [Header("Pc Click And Drag Propertys")]
    // HAVE AN EXTRA ONE IN WIDTH TO FIX GRID IDK WHY BUT IT FIXES IT
    [SerializeField]
    private int width = 10, height = 5;
    [SerializeField]
    private Transform startpos, endpos;
    [SerializeField]
    private GameObject[,] gridarray;
    private Transform temphold;
    [SerializeField]
    private LayerMask mask;
    [SerializeField]
    private Transform getgrid;
    [Header(" placeing the object in the grid")]
    private Vector3 mOffset;
    private float mZCoord;
    [Header("Camera And Positon propertys")]
    [SerializeField]
    private Transform CameraPostion;
    [SerializeField]
    private GameObject GetHead;
    [SerializeField]
    private CameraController cam;
    [Header("fix a bug from being in the same xgrid")]
    private int fixgridx = 0;
    private void Start()
    {
        gridarray = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridarray[x, y] = getgrid.GetChild(y).gameObject;
                gridarray[x, y] = getgrid.GetChild(y).GetChild(x).gameObject;

            }
        }
    }
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
        LeanTween.alphaCanvas(staminaandhealth, 0, 60.0f * Time.deltaTime);
        LeanTween.move(GetHead, CameraPostion.position, 60.0f * Time.deltaTime).setOnComplete(() =>
        {
            Pcmanager.Instance.CanClick = true;

        });
        StartCoroutine(enterthingrotation());
    }

    // private void letgoofobject()
    // {
    //    // temphold.position = translatetocell(temphold.position);
    //     temphold.localPosition = new Vector3(0, 0, 0);
    //     tempcollider.enabled = true;
    //     tempcollider = null;
    //     temphold = null;
    // }
    private IEnumerator enterthingrotation()
    {
        int i = 0;
        while (i < 60)
        {
            GetHead.transform.rotation = Quaternion.Lerp(GetHead.transform.rotation, CameraPostion.rotation, 10.0f * Time.deltaTime);
            i++;
            yield return new WaitForFixedUpdate();
        }

    }

    private void ExitThing()
    {
        Pcmanager.Instance.InteractPc -= getmousepos;
        LeanTween.alphaCanvas(staminaandhealth, 1, 60.0f * Time.deltaTime);
        LeanTween.moveLocal(GetHead, Vector3.zero, 30.0f * Time.deltaTime).setEaseOutSine().setOnComplete(() =>
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
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask, QueryTriggerInteraction.Collide))
        {
            temphold = raycas.transform;;
            mZCoord = Camera.main.WorldToScreenPoint(temphold.position).z;
            mOffset = temphold.position - mouseposinput();
            StartCoroutine(bringapplicationtomousepos());
        }

    }

    private IEnumerator bringapplicationtomousepos()
    {
        temphold.parent = null;
        fixgridx = 0;
        GameObject getgridobject = null;
        GameObject savegridobject = null;
        GameObject getnextrow = null;

        bool isdonesearching = false;

        float distance = 0.0f;
        float maxdistance = 10.0f;
        int xsave = 0, ysave = 0;
        int godowntocheck = 0;
        int xvalue = 0;
        
        while (Input.GetKey(KeyCode.Mouse0))
        {
            temphold.position = mouseposinput() + mOffset;
            temphold.position = clamp(temphold.position);
            yield return new WaitForFixedUpdate();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                getgridobject = gridarray[x, y];
                distance = Vector3.Distance(temphold.position, getgridobject.transform.position);
                if (distance < maxdistance)
                {
                    xsave = x;
                    ysave = y;
                    maxdistance = distance;
                    savegridobject = getgridobject;
                }

            }
        }
        getnextrow = gridarray[xsave, ysave];

        while (!isdonesearching)
        {

            if (getnextrow.transform.childCount > 0)
            {
                fixgridx = ysave > height - 2 ? fixgridx += 1 : fixgridx;
                xvalue = ysave > height - 2 ? xsave + 1 : xsave + fixgridx;
                xvalue = xsave + 1 > width - 1 ? 0 : xvalue;
                godowntocheck += 1;
                godowntocheck = ysave> height - 2 ? 0 : godowntocheck;
                
                getnextrow = gridarray[xvalue, ysave = ysave > height - 2 ? godowntocheck : ysave +godowntocheck];
                
                isdonesearching = getnextrow.transform.childCount> 0 ? false : true;
            }
            else
            {
                // getnextrow = getnextrow != null ? gridarray[xvalue, y + godowntocheck] : savegridobject;
                isdonesearching = true;
            }
            yield return new WaitForEndOfFrame();
        }
        temphold.parent = savegridobject.transform.childCount > 0 ? getnextrow.transform : savegridobject.transform;
        temphold.localPosition = Vector3.zero;
        temphold = null;
        
    }
    // private IEnumerator checkforotherspot()
    // {
    //
    //     if (Physics.BoxCast(temphold.position + new Vector3(0, 0, 0.3f), new Vector3(cellsize, cellsize, cellsize) / 3, temphold.forward, out RaycastHit raycas, Quaternion.identity, 999f, mask))
    //     {
    //         raycas = temphold == raycas.transform ? new RaycastHit() : raycas;
    //         //getotherapplication = raycas.transform;
    //        // float e = translatetocell(raycas.transform.position).y;
    //         //raycas.transform.position = e < 7.22f ? translatetocell(raycas.transform.position + new Vector3(0.03f, ConstValues.Float.one, ConstValues.Float.zero)) : translatetocell(raycas.transform.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));
    //
    //        // raycas.transform.localPosition = clamp(raycas.transform.localPosition);
    //         raycas.transform.localPosition = new Vector3(raycas.transform.localPosition.x, raycas.transform.localPosition.y, 0.0f);
    //         raycas.collider.enabled = false;
    //         bool isslottaken = false;
    //         while (isslottaken == false)
    //         {
    //            // if (Physics.Raycast(getotherapplication.position + new Vector3(0, 0, 0.3f), getotherapplication.forward, 999f, mask))
    //           //  {
    //              //   float t = translatetocell(raycas.transform.position).y;
    //             //    getotherapplication.transform.position = t < 7.22f ? translatetocell(getotherapplication.position + new Vector3(0.03f, ConstValues.Float.one, ConstValues.Float.zero)) : translatetocell(getotherapplication.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));
    //             //    getotherapplication.localPosition = new Vector3(getotherapplication.localPosition.x, getotherapplication.localPosition.y, ConstValues.Float.zero);
    //             //    getotherapplication.localPosition = clamp(getotherapplication.localPosition);
    //           //  }
    //           //  else
    //            // {
    //            //     raycas.collider.enabled = true;
    //                 
    //             //    isslottaken = true;
    //            // }
    //             //yield return new WaitForSeconds(0.1f);
    //             yield return new WaitForFixedUpdate();
    //         }
    //         //letgoofobject();
    //     }
    //     else
    //     {
    //        // letgoofobject();
    //     }
    // }


    //  private Vector3 translatetocell(Vector3 pos)
    //  {
    //      return grid.GetCellCenterWorld(grid.WorldToCell(pos));
    //  }
    private Vector3 clamp(Vector3 pos)
    {
        //Vector3 clamp = pos;
        //  clamp.x = Mathf.Clamp(clamp.x, -ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.4f, ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.43f);
        //  clamp.y = Mathf.Clamp(clamp.y, -ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y / 0.95f, ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y / 0.92f);
        pos.x = Mathf.Clamp(pos.x, endpos.position.x, startpos.position.x);
        pos.y = Mathf.Clamp(pos.y, startpos.position.y, endpos.position.y);
        return pos;
    }
    private Vector3 mouseposinput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousepos);
    }
}
