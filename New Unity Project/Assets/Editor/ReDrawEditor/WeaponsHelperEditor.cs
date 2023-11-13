using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponsHelper))]
public class WeaponsHelperEditor : Editor
{
    private SerializedProperty characterProp;

    private void OnEnable()
    {
        characterProp = serializedObject.FindProperty("character");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(characterProp);
        EditorGUILayout.Space();
        if (GUILayout.Button("Show Debug Logs"))
        {
            ((WeaponsHelper)target).ShowDebugLogs();
        }

        serializedObject.ApplyModifiedProperties();
    }
}