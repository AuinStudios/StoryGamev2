// Created by Vladis.

//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this DoorManager do?
/// </summary>
public sealed class DoorManager : MonoBehaviour
{
    private List<Door> Doors;
    [SerializeField]
    private Transform Player;
    public Transform test;
    private void Start()
    {
        Doors = new List<Door>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Doors.Add(transform.GetChild(i).GetComponent<Door>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (Door door in Doors)
            {
                if ((Player.position - door.transform.position).magnitude < 3)
                {

                    door.OpenDoor();

                }
            }
        }
    }
}
