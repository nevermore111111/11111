using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HitData))]
public class HitDataEditor : Editor
{
    SerializedProperty currentHit;

    SerializedProperty fadeTime00;
    SerializedProperty stayTime00;
    SerializedProperty timeScale00;

    SerializedProperty fadeTime01;
    SerializedProperty stayTime01;
    SerializedProperty timeScale01;

    SerializedProperty fadeTime02;
    SerializedProperty stayTime02;
    SerializedProperty timeScale02;

    SerializedProperty fadeTime03;
    SerializedProperty stayTime03;
    SerializedProperty timeScale03;

    private void OnEnable()
    {
        // Setup the SerializedProperties
        currentHit = serializedObject.FindProperty("CurrentHit");

        fadeTime00 = serializedObject.FindProperty("fadeTime00");
        stayTime00 = serializedObject.FindProperty("stayTime00");
        timeScale00 = serializedObject.FindProperty("timeScale00");

        fadeTime01 = serializedObject.FindProperty("fadeTime01");
        stayTime01 = serializedObject.FindProperty("stayTime01");
        timeScale01 = serializedObject.FindProperty("timeScale01");

        fadeTime02 = serializedObject.FindProperty("fadeTime02");
        stayTime02 = serializedObject.FindProperty("stayTime02");
        timeScale02 = serializedObject.FindProperty("timeScale02");

        fadeTime03 = serializedObject.FindProperty("fadeTime03");
        stayTime03 = serializedObject.FindProperty("stayTime03");
        timeScale03 = serializedObject.FindProperty("timeScale03");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(currentHit);

        string[] options = new string[] { "��������00", "��������01", "��������02", "��������03" };
        currentHit.intValue = EditorGUILayout.Popup("��ǰ��������", currentHit.intValue, options);

        // ����ѡ����ʾ��Ӧ��ʱͣ����
        switch (currentHit.intValue)
        {
            case 0:
                DrawTimeStopParameters(fadeTime00, stayTime00, timeScale00);
                break;
            case 1:
                DrawTimeStopParameters(fadeTime01, stayTime01, timeScale01);
                break;
            case 2:
                DrawTimeStopParameters(fadeTime02, stayTime02, timeScale02);
                break;
            case 3:
                DrawTimeStopParameters(fadeTime03, stayTime03, timeScale03);
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTimeStopParameters(SerializedProperty fadeTime, SerializedProperty stayTime, SerializedProperty timeScale)
    {
        // �����������Ҫ�Ļ��ַ�Χ��0.0f��1.0f
        EditorGUILayout.Slider(fadeTime, 0.0f, 0.2f, new GUIContent("���뽥��ʱ��"));
        EditorGUILayout.Slider(stayTime, 0.0f, 0.4f, new GUIContent("����ʱ��"));
        EditorGUILayout.Slider(timeScale, 0.0f, 1.0f, new GUIContent("ʱ������"));
    }
}
