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
    public void InvokePc()
    {
        InteractPc.Invoke();
    }
    private void Update()
    {
        if (CanOpenPc == true && Input.GetKeyDown(KeyCode.E))
        {
            InvokePc();
        }
        else if (CanClick == true && Input.GetKeyDown(KeyCode.Mouse0))
        {
            InvokePc();
        }
    }
}
