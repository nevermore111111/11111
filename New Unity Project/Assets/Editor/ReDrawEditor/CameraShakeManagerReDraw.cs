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

        if (GUILayout.Button("震动"))
        {
          CameraShakeManager.Instance.Shake();

            // 强制刷新Inspector界面
            Repaint();
        }
    }
}
