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
    [SerializeField]
    private LayerMask mask;
    private float cellsize = 0.06f;


    private Vector3 mOffset;
    private float mZCoord;

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
        x = Mathf.FloorToInt(worldpos.x / cellsize);
         //z = Mathf.FloorToInt(worldpos.z / cellsize);
        y = Mathf.FloorToInt(worldpos.y / cellsize);
    }
    private Vector3 getworldpositon(int x, int y)
    {
        return new Vector3(x, y) * cellsize;
    }
    private Vector3 getmousepos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f, mask))
        {
            temphold = raycas.transform;
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
            getXZ(mouseposinput() + mOffset, out int x, out int y);
            temphold.position = getworldpositon(x, y);
            temphold.localPosition = new Vector3(temphold.localPosition.x, temphold.localPosition.y, 0);
            temphold = null;
        }
        if (temphold != null)
        {
            temphold.position = mouseposinput() + mOffset;

        }

        // else if(CanClick == true && Input.GetKeyUp(KeyCode.Mouse0))
        // {
        //     getXZ(getmousepos(), out int x, out int y);
        //     temphold.position = getworldpositon(x, y);
        // }
    }
    private Vector3 mouseposinput()
    {
        Vector3 mousepos = Input.mousePosition;
        mousepos.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousepos);
    }
}
