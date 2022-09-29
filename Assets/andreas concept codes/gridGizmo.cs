// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this gridGizmo do?
/// </summary>
public sealed class gridGizmo : MonoBehaviour
{
    public Color color = Color.red;
    public Vector3 size = Vector3.zero;
    public bool isWireFrame = true;
    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        if (isWireFrame)
        {
            Gizmos.DrawWireCube(transform.position, size);
        }
        else
        {
            Gizmos.DrawCube(transform.position, size);
        }
    }
}
