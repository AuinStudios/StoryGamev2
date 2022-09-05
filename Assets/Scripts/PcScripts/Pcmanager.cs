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
    private Transform temphold;
    [SerializeField]
    private LayerMask mask;
    private float cellsize = 0.3f;
    public void InvokePc()
    {
		InteractPc.Invoke();
    }
    
    private void getXZ(Vector3 worldpos, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldpos.x / cellsize);
        z = Mathf.FloorToInt(worldpos.z / cellsize);
        y = Mathf.FloorToInt(worldpos.y / cellsize);
    }
    private Vector3 getworldpositon(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellsize;
    }
    private Vector3 getmousepos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycas, 999f ,mask ))
        {
            temphold = raycas.transform;
            return raycas.point;
        }
        else if(temphold != null)
        {
            return temphold.position;
        }
        else
        {
            return Vector3.zero;
        }
    }
    private void Update()
    {
        if(CanOpenPc == true && Input.GetKeyDown(KeyCode.E))
        {
            InvokePc();
        }
        if(CanClick == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            getmousepos();
            temphold.position = Input.mousePosition;
        }
        else if(CanClick == true && Input.GetKeyUp(KeyCode.Mouse0))
        {
            getXZ(getmousepos(), out int x, out int y, out int z);
            temphold.position = getworldpositon(x, y, z);
        }
    }
}
