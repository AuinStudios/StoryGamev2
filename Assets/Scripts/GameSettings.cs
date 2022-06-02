using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Settings/MouseOptions")]
public class GameSettings : ScriptableObject
{
    [Range(1.0f, 10.0f)]
    public float mouseSensitivity = 5.0f;
}
