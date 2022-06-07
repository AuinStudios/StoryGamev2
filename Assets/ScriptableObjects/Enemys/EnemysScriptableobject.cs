// Created by Vladis.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     What does this Enemys do?
/// </summary>
/// 
[CreateAssetMenu(fileName = "New Enemy", menuName = "enemys")]
public sealed class EnemysScriptableobject : ScriptableObject
{
    
    public int speed;

    public int damage;

    public int health;

    public int minAttackDelay;

    public int maxAttackDelay;
}
