using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Item", menuName = "Items")]
public class ItemsScriptableobject : ScriptableObject
{

    public string ItemName;

    public bool IsActive = true;

    public Sprite ItemIcon;

    public int damage = 5;

    public bool CanAttack = true;
}
