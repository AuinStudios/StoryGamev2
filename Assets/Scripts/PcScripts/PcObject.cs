// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
///     What does this PcObject do?
/// </summary>
/// 
public abstract class functions : PcObject
{
    public Transform applicationopen = null;
    private void Start()
    {
        applicationopen = holdapplication;
    }
    public virtual void fireray()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask, QueryTriggerInteraction.Collide))
        {
            applicationopen = raycas.transform;
        }
    }
}

public class openapplicationclass : functions
{
    private void openapplication()
    {
        LeanTween.move(applicationopen.GetChild(0).gameObject, applicationopen.position + new Vector3(0.1f, 0.1f, 0.0f), 60.0f * Time.deltaTime);
        LeanTween.scale(applicationopen.GetChild(0).gameObject, new Vector3(8.0f, 5.0f, 0.0f), 60.0f * Time.deltaTime);
    }

}

public class PcObject : MonoBehaviour
{
    [Header("Ui")]
    [SerializeField]
    private CanvasGroup staminaandhealth;
    [SerializeField]
    private TextMeshProUGUI interactDialog;
    [SerializeField]
    private Transform interactHoverTransform;
    [Header("Pc Click And Drag Propertys")]
    // HAVE AN EXTRA ONE IN WIDTH TO FIX GRID IDK WHY BUT IT FIXES IT
    [SerializeField]
    private int width = 10, height = 5;
    [SerializeField]
    private Transform startpos, endpos;
    [SerializeField]
    private GameObject[,] gridarray;
    [HideInInspector]
    public Transform holdapplication;
    [SerializeField]
    private Transform getgrid;
    public LayerMask mask;
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
    [SerializeField]
    private Camera CamRay;
    [Header("fix a bug from being in the same xgrid")]
    private int fixgridx = 0;
    [Header("bools")]
    private bool canexitorenter;
    private bool stoprotation;
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
            ShowInteractDialog();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pcmanager.Instance.InteractPc -= enterthing;

            Pcmanager.Instance.CanOpenPc = false;
            HideInteractDialog();
        }
    }

    private void enterthing()
    {
        if (canexitorenter == false && stoprotation == false)
        {
            HideInteractDialog();
            canexitorenter = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            CharacterMovement.Instance.enabled = false;
            cam.enabled = false;
            LeanTween.alphaCanvas(staminaandhealth, ConstValues.Float.zero, 60.0f * Time.deltaTime);
            LeanTween.move(GetHead, CameraPostion.position, 60.0f * Time.deltaTime).setEaseOutSine().setOnComplete(() =>
            {

                Pcmanager.Instance.CanClick = true;
                stoprotation = true;
                StartCoroutine(WaitUntllPressExitKey());
                StartCoroutine(clickfunction());
            });
            StartCoroutine(rotationlerp(CameraPostion.rotation));
        }

    }
    private IEnumerator WaitUntllPressExitKey()
    {
        yield return new WaitUntil(() => Pcmanager.Instance.CanClick == true && Input.GetKeyDown(KeyCode.T));
        Pcmanager.Instance.InteractPc -= enterthing;
        Pcmanager.Instance.InteractPc += ExitThing;

        Pcmanager.Instance.InvokePc();
    }
    private IEnumerator rotationlerp(Quaternion rot)
    {
        float timedelta = 0;
        while (!stoprotation)
        {
            timedelta += ConstValues.Float.one * Time.deltaTime;
            GetHead.transform.rotation = Quaternion.Lerp(GetHead.transform.rotation, rot, timedelta / ConstValues.Float.one);
            yield return new WaitForFixedUpdate();
        }
        GetHead.transform.rotation = rot;
        stoprotation = false;
    }
    private void ExitThing()
    {
        if (canexitorenter == true && stoprotation == false)
        {
            canexitorenter = false;
            Pcmanager.Instance.InteractPc -= getmousepos;

            LeanTween.alphaCanvas(staminaandhealth, ConstValues.Float.one, 60.0f * Time.deltaTime);
            LeanTween.moveLocal(GetHead, Vector3.zero, 60.0f * Time.deltaTime).setEaseOutSine().setOnComplete(() =>
             {
                 Cursor.visible = false;
                 Cursor.lockState = CursorLockMode.Locked;
                 Pcmanager.Instance.CanClick = false;
                 Pcmanager.Instance.CanOpenPc = true;
                 CharacterMovement.Instance.enabled = true;
                 cam.enabled = true;
                 ShowInteractDialog();
                 stoprotation = true;
                 Pcmanager.Instance.InteractPc += enterthing;
                 Pcmanager.Instance.InteractPc -= ExitThing;
             }); ;
            StartCoroutine(rotationlerp(GetHead.transform.parent.rotation));
        }


    }
    private IEnumerator clickfunction()
    {
        int i = 0;
        bool a = false;
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        Ray ray = CamRay.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask, QueryTriggerInteraction.Collide))
        {
            while (!a)
            {
                Debug.Log("A");
                if (Input.GetMouseButton(0) && Input.GetAxis("Mouse X") < 0.0f || Input.GetAxis("Mouse X") > 0.0f || Input.GetAxis("Mouse Y") < 0.0f || Input.GetAxis("Mouse Y") > 0.0f)
                {
                    Pcmanager.Instance.InteractPc += getmousepos;
                    Pcmanager.Instance.InvokePc();
                    break;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    i++;
                    Debug.Log(i);
                }
                yield return new WaitForFixedUpdate();
            }

        }
        else
        {
            StartCoroutine(clickfunction());
        }
        if (i > 1)
        {
            // test
        }

    }
    private void getmousepos()
    {
        Ray ray = CamRay.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask, QueryTriggerInteraction.Collide))
        {
            Debug.Log("hmm");
            Pcmanager.Instance.CanClick = false;
            holdapplication = raycas.transform; ;
            mZCoord = CamRay.WorldToScreenPoint(holdapplication.position).z;
            mOffset = holdapplication.position - mouseposinput();
            StartCoroutine(bringapplicationtomousepos());
        }
    }

    private IEnumerator bringapplicationtomousepos()
    {
        holdapplication.parent = null;
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

        while (Input.GetMouseButton(0))
        {
            holdapplication.position = mouseposinput() + mOffset;
            holdapplication.position = clamp(holdapplication.position);
            yield return new WaitForFixedUpdate();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                getgridobject = gridarray[x, y];
                distance = Vector3.Distance(holdapplication.position, getgridobject.transform.position);
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
                fixgridx = ysave > height - 2 ? fixgridx += ConstValues.Int.one : fixgridx;
                xvalue = ysave > height - 2 ? xsave + ConstValues.Int.one : xsave + fixgridx;
                xvalue = xvalue < width - ConstValues.Int.one ? xvalue : ConstValues.Int.zero;
                godowntocheck = ConstValues.Int.one;
                godowntocheck = ysave > height - 2 ? ConstValues.Int.zero : godowntocheck;
                getnextrow = gridarray[xvalue, ysave = ysave > height - 2 ? godowntocheck : ysave + godowntocheck];

                isdonesearching = getnextrow.transform.childCount > ConstValues.Int.zero ? false : true;
            }
            else
            {
                isdonesearching = true;
            }
            yield return new WaitForEndOfFrame();
        }
        if (holdapplication != null)
        {
            holdapplication.parent = savegridobject.transform.childCount > ConstValues.Int.zero ? getnextrow.transform : savegridobject.transform;
            holdapplication.localPosition = Vector3.zero;
        }
        holdapplication = null;
        Pcmanager.Instance.CanClick = true;
        Pcmanager.Instance.InteractPc -= getmousepos;
        StartCoroutine(clickfunction());
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
    private void ShowInteractDialog()
    {
        LeanTween.cancel(interactHoverTransform.gameObject);
        LeanTween.scale(interactHoverTransform.gameObject, Vector3.one, 30.0f * Time.deltaTime).setEaseOutBounce();

        interactDialog.text = "Press 'E'";
       // LeanTween.alphaText(interactDialog.rectTransform, ConstValues.Float.one, 30.0f * Time.deltaTime);
    }
    private void HideInteractDialog()
    {
        LeanTween.cancel(interactHoverTransform.gameObject);
        LeanTween.scale(interactHoverTransform.gameObject, Vector3.zero, 30.0f * Time.deltaTime).setOnComplete(() =>
        {
            interactDialog.text = "";
        });
        
    }
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
        return CamRay.ScreenToWorldPoint(mousepos);
    }


}
