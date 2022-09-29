// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this setparenttransform do?
/// </summary>
[ExecuteInEditMode]
public sealed class setparenttransform : MonoBehaviour
{
    public void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position;
    }
}
