// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this DoorController do?
/// </summary>
public sealed class DoorController : MonoBehaviour
{
    #region Singleton
    public static DoorController Instance { get; private set; }
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

    public delegate void Doors();
	public event Doors InteractDoor;

    [SerializeField]
    private new string tag;
    public string GetTag { get { return tag; } }
    [HideInInspector]
    public bool CanOpenDoor = false;
	// Update is called once per frame
	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.E)&&  CanOpenDoor == false) 
        {
            invoketheevent();
        }
	}

    public void invoketheevent()
    {
      InteractDoor?.Invoke();
    }
}
