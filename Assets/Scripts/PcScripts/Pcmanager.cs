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
    [SerializeField]
    private Grid grid;
    [Header("Pc Click And Drag Propertys")]
    [SerializeField]
    private Transform temphold;
    private Collider tempcollider;
    [SerializeField]
    private SpriteRenderer ParentScreencolidder;
    [SerializeField]
    private LayerMask mask;
    private float cellsize = 0.06f;
    private Transform getotherapplication;
    // placeing the objects in a grid
    private Vector3 mOffset;
    private float mZCoord;
    [SerializeField]
    private Vector2 GridOffset;
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
    // private void getXZ(Vector3 worldpos, out int x, out int y)//, out bool work)
    // {
    //     //bool walkable = !(Physics.CheckSphere(worldpos, cellsize, mask));
    //     x = Mathf.FloorToInt(worldpos.x / cellsize);
    //     //z = Mathf.FloorToInt(worldpos.z / cellsize);
    //     y = Mathf.FloorToInt(worldpos.y / cellsize);
    //     Debug.Log(getworldpositon(x, y));
    //     if (Physics.BoxCast(temphold.position + new Vector3(0, 0, 0.3f), new Vector3(cellsize, cellsize, cellsize) / 2, temphold.forward, out RaycastHit raycas, Quaternion.identity, 999f, mask))
    //     {
    //         // Debug.Log("a");
    //         // Debug.Log(raycas.transform.gameObject.name);
    //         if (temphold == raycas.transform)
    //         {
    //             raycas = new RaycastHit();
    //             //  -0.8382121f / cellsize
    //         }
    //        
    //         raycas.transform.position = y < 18 ? getworldpositon(x + 1, y + 7) : getworldpositon(x, y - 1); ;
    //         
    //         raycas.transform.localPosition = clamp(raycas.transform.localPosition);
    //         raycas.transform.localPosition = new Vector3(raycas.transform.localPosition.x, raycas.transform.localPosition.y, 0.0f);
    //     }
    // }
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
            ParentScreencolidder = raycas.transform.parent.GetComponent<SpriteRenderer>();
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
    private IEnumerator helpme()
    {

        if (Physics.BoxCast(temphold.position + new Vector3(0, 0, 0.3f), new Vector3(cellsize, cellsize, cellsize) / 3, temphold.forward, out RaycastHit raycas, Quaternion.identity, 999f, mask))
        {
            // Debug.Log("a");
            // Debug.Log(raycas.transform.gameObject.name);
            if (temphold == raycas.transform)
            {
                raycas = new RaycastHit();
                //  -0.8382121f / cellsize
            }
            getotherapplication = raycas.transform;
            float e = translatetocell(raycas.transform.position).y;
            raycas.transform.position = e < 7.22f ? translatetocell(raycas.transform.position + new Vector3(0.03f, ConstValues.Float.one, ConstValues.Float.zero)) : translatetocell(raycas.transform.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));

            raycas.transform.localPosition = clamp(raycas.transform.localPosition);
            raycas.transform.localPosition = new Vector3(raycas.transform.localPosition.x, raycas.transform.localPosition.y, 0.0f);
            raycas.collider.enabled = false;
            bool test = false;
            while (test == false)
            {
              
                
                if (Physics.Raycast(getotherapplication.position + new Vector3(0, 0, 0.3f), getotherapplication.forward, out RaycastHit raycs, 999f, mask))
                {
                    Debug.Log("a");
                    getotherapplication.transform.position = translatetocell(getotherapplication.position + new Vector3(ConstValues.Float.zero, -0.06f, ConstValues.Float.zero));
                    getotherapplication.localPosition = new Vector3(getotherapplication.localPosition.x, getotherapplication.localPosition.y, ConstValues.Float.zero);
                    
                }
                else
                {
                    Debug.Log("B");
                    raycas.collider.enabled = true;
                    test = true;
                }
                //yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
            }

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

            temphold.position = translatetocell(temphold.position);
            StartCoroutine(helpme());
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

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawRay(temphold.position, temphold.forward);
    //     Gizmos.DrawWireCube(temphold.position + temphold.forward, new Vector3(cellsize, cellsize, cellsize) / 2);
    // }





}
