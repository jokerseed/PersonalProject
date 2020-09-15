using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
 * 窗口编辑器
 */
[CustomEditor(typeof(Example2))]
public class Inspec : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("example2");
        if (GUILayout.Button("xxx")) {
            Debug.Log("hello world");
        }
        GUILayout.EndVertical();
    }
}