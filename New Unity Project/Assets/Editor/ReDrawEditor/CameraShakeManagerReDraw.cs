using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraShakeManager))]
public class CameraShakeManagerReDraw : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("��"))
        {
            CameraShakeManager shake = (CameraShakeManager)target;
            shake.Shake();

            // ǿ��ˢ��Inspector����
            Repaint();
        }
    }
}
