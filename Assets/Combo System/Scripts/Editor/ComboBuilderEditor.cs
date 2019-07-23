using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEditor;

[CustomEditor(typeof(ComboController))]
public class ComboControllerEditor  : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ComboController myScript = (ComboController)target;

        if (GUILayout.Button("Order Combos"))
        {
            myScript.OrderList();
        }
        if (GUILayout.Button("Remove Duplicates"))
        {
            myScript.RemoveDuplicates();
        }
    }
}
