using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ItemsScriptableobject)), CanEditMultipleObjects]
public class ItemEditor : Editor
{
    ItemsScriptableobject itemsScriptableobject;
    private void OnEnable()
    {
        itemsScriptableobject = target as ItemsScriptableobject;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        itemsScriptableobject.IsKeycard = EditorGUILayout.Toggle("IsKeycard", itemsScriptableobject.IsKeycard);
        if (!itemsScriptableobject.IsKeycard)
        {
            itemsScriptableobject.ItemName = EditorGUILayout.TextField("Put Item Name", itemsScriptableobject.ItemName);
            itemsScriptableobject.ItemIcon = (Sprite)EditorGUILayout.ObjectField("ItemIcon: ", itemsScriptableobject.ItemIcon, typeof(Sprite), false);

            GUILine();

            itemsScriptableobject.CanAttack = EditorGUILayout.Toggle("Can Attack", itemsScriptableobject.CanAttack);

            GUILayout.MaxWidth(EditorGUIUtility.labelWidth);
            if (itemsScriptableobject.CanAttack)
            {
                GUILayout.ExpandWidth(false);
                itemsScriptableobject.MaxDamage = EditorGUILayout.IntSlider("MaxDamage", itemsScriptableobject.MaxDamage, 0, 30);
                itemsScriptableobject.NormalDamage = EditorGUILayout.IntSlider("Damage", itemsScriptableobject.NormalDamage, 0, itemsScriptableobject.MaxDamage);
            }
        }
        else
        {
            itemsScriptableobject.ItemName = EditorGUILayout.TextField("Put Item Name", itemsScriptableobject.ItemName);
            itemsScriptableobject.KeycardLevel = EditorGUILayout.IntSlider("KeycardLevel", itemsScriptableobject.KeycardLevel, 0, 10);
        }
        bool somethingChanged = EditorGUI.EndChangeCheck();
        if (somethingChanged)
        {
            EditorUtility.SetDirty(itemsScriptableobject);
        }
        serializedObject.ApplyModifiedProperties();
    }
    void GUILine()
    {
        EditorGUILayout.Space(8);

        Rect rect = EditorGUILayout.GetControlRect(false, 1);

        rect.height = 1;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        EditorGUILayout.Space(6);
    }
}
