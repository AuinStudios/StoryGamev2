// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Pcmanager do?
/// </summary>
/// 
public sealed class Pcmanager : MonoBehaviour
{

    #region Singleton
    public static Pcmanager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }
    #endregion
    public delegate void Pcs();
    public event Pcs InteractPc;
    public bool CanClick = false;
    public bool CanOpenPc = false;
    
    [Header("Pc Click And Drag Propertys")]
    [SerializeField]
    private Transform temphold;
    private Collider tempcollider;
    [SerializeField]
    private SpriteRenderer ParentScreencolidder;
    [SerializeField]
    private LayerMask mask;
    private float cellsize = 0.06f;

    // placeing the objects in a grid
    private Vector3 mOffset;
    private float mZCoord;
    [SerializeField]
    private Vector2 GridOffset;
    private Vector2 boundicontopc = new Vector2(-1.406404f , 0.8490362f);
    #region testing propertys
    //[SerializeField]
    //private GameObject testobject;
    //  private void Start()
    //  {
    //      for (int t = 0; t < 10; t++)
    //      {
    //          for (int x = 0; x < 10; x++)
    //          {
    //              testobject.transform.position = new Vector3(t * cellsize , x *cellsize , 0) ;
    //              GameObject test = Instantiate(testobject, testobject.transform.position, Quaternion.identity);
    //              test.name = "";
    //          }
    //      }
    //  }
    #endregion
    public void InvokePc()
    {
        InteractPc.Invoke();
    }

    private void getXZ(Vector3 worldpos, out int x, out int y)
    {
        bool walkable = !(Physics.CheckSphere(worldpos, cellsize, mask));
        x = Mathf.FloorToInt(worldpos.x / cellsize);
        //z = Mathf.FloorToInt(worldpos.z / cellsize);
        y = Mathf.FloorToInt(worldpos.y / cellsize);
        if (!walkable)
        {
          // Ray ray = Camera.main.ScreenPointToRay(new Vector3(temphold.localPosition.x , temphold.localPosition.y , 0.3f));
            if (Physics.Raycast(mouseposinput() + mOffset, temphold.forward * -2.0f, out RaycastHit raycas, 999f, mask))
            {
                //int offset = clamp(raycas.transform.localPosition).x >= getworldpositon(x, y).x ? 
                Debug.Log("a");
                tempcollider.enabled = false;
                raycas.transform.position =  raycas.transform.localPosition.y >= clamp(raycas.transform.localPosition).y  ? getworldpositon(x, y - 1) : getworldpositon(x + 1, y + 7)  ;
                raycas.transform.localPosition = new Vector3(raycas.transform.localPosition.x, raycas.transform.localPosition.y, 0.0f);
                raycas.transform.localPosition = clamp(raycas.transform.localPosition);
            }
        }
    }
    private Vector3 getworldpositon(int x, int y)
    {
        return new Vector3(x - GridOffset.x, y - GridOffset.y) * cellsize;
    }
    private Vector3 getmousepos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask))
        {
            temphold = raycas.transform;
            ParentScreencolidder = ParentScreencolidder != ParentScreencolidder? raycas.transform.parent.GetComponent<SpriteRenderer>() : ParentScreencolidder;
            tempcollider = raycas.collider;
            tempcollider.enabled = false;
            mZCoord = Camera.main.WorldToScreenPoint(temphold.position).z;
            mOffset = temphold.position - mouseposinput();
            return raycas.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
 
    private void Update()
    {
        if (CanOpenPc == true && Input.GetKeyDown(KeyCode.E))
        {
            InvokePc();
        }
        if (CanClick == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            getmousepos();
        }
        else if (CanClick == true && Input.GetKeyUp(KeyCode.Mouse0))
        {
            getXZ(temphold.position,out  int x,  out int y);
            temphold.position = getworldpositon(x, y);
            temphold.localPosition = new Vector3(temphold.localPosition.x, temphold.localPosition.y, 0);
            tempcollider.enabled = true;
            tempcollider = null;
            temphold = null;
        }
        if (temphold != null)
        {
            temphold.position = mouseposinput() + mOffset;
            temphold.localPosition = clamp(temphold.localPosition);
        }
    }
    private Vector3 clamp(Vector3 pos)
    {
        Vector3 clamp = pos;
        clamp.x = Mathf.Clamp(clamp.x, -ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.4f, ParentScreencolidder.bounds.size.x * ParentScreencolidder.sprite.bounds.size.x / 1.43f);
        clamp.y = Mathf.Clamp(clamp.y, -ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y /0.95f, ParentScreencolidder.bounds.size.y * ParentScreencolidder.sprite.bounds.size.y / 0.92f);
        return clamp;
    }
    private Vector3 mouseposinput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousepos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(mouseposinput() + mOffset, mouseposinput() + mOffset);
    }
}
