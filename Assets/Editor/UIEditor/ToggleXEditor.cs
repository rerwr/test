using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
[CustomEditor(typeof(ToggleX),true)]
[CanEditMultipleObjects]
public class ToggleXEditor : ToggleEditor
{
    private SerializedProperty m_toggleIdProperty;
    private SerializedProperty m_OnTogXSelectedProperty;
    protected override void OnEnable()
    {
        base.OnEnable();
        m_toggleIdProperty = serializedObject.FindProperty("ToggleId");
        m_OnTogXSelectedProperty = serializedObject.FindProperty("OnTogXSelected");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        serializedObject.Update();
        EditorGUILayout.LabelField("ToggleId",m_toggleIdProperty.intValue.ToString());

        base.OnInspectorGUI();

        serializedObject.Update();
//        EditorGUILayout.PropertyField(m_OnTogXSelectedProperty);
        serializedObject.ApplyModifiedProperties();
    }
}
