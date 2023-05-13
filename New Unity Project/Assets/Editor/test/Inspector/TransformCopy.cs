//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using Unity.Mathematics;
//using NUnit.Framework.Internal;
//using System.Runtime.CompilerServices;

//public static class TransformCopy
//{
//    [MenuItem("CONTEXT/Transform/策划/CopyAndPaste/copyAllLocal", false, 1)]
//    static void CopyLocalNum()
//    {
//        Vector3 LPos = Selection.activeTransform.localPosition;
//        string LocalPos = "LocalPos:" + LPos.ToString();
//        Vector3 LRot = Selection.activeTransform.localEulerAngles;

//        string LocalRot = "LocalRot:" + LRot.ToString();
//        Vector3 LSca = Selection.activeTransform.localScale;
//        string LocalScale = "LocalSca" + LSca.ToString();

//        GUIUtility.systemCopyBuffer = LocalPos + LocalRot + LocalScale;
//    }
//    [MenuItem("CONTEXT/Transform/策划/CopyAndPaste/copyWorldPos&Rotate", false, 2)]
//    static void CopyWorld()
//    {
//        Vector3 LPos = Selection.activeTransform.position;
//        string LocalPos = "LocalPos:" + LPos.ToString();
//        Vector3 LRot = Selection.activeTransform.eulerAngles;

//        string LocalRot = "LocalRot:" + LRot.ToString();
//        Vector3 LSca = Selection.activeTransform.localScale;
//        string LocalScale = "LocalSca" + LSca.ToString();

//        GUIUtility.systemCopyBuffer = LocalPos + LocalRot + LocalScale;
//    }
//    [MenuItem("CONTEXT/Transform/策划/CopyAndPaste/pasteAllWorld", false, 2)]
//    static void PasteWorldNum()
//    {
//        string Local = GUIUtility.systemCopyBuffer;
//        string[] Local01 = Local.Split(new char[2] { '(', ')' });
//        string[] Local02 = new string[3];
//        for (int i = 0; i < 3; i++)
//        {
//            Local02[i] = Local01[2 * i + 1];
//            Debug.Log(Local02[i]);
//        }
//        Selection.activeTransform.position = stringToVector(Local02[0]);
//        Selection.activeTransform.eulerAngles = stringToVector(Local02[1]);
//    }
//    [MenuItem("CONTEXT/Transform/策划/CopyAndPaste/pasteAllLocal", false, 1)]
//    static void PasteLocalNum()
//    {
//        string Local = GUIUtility.systemCopyBuffer;
//        string[] Local01 = Local.Split(new char[2] { '(', ')' });
//        string[] Local02 = new string[3];
//        for (int i = 0; i < 3; i++)
//        {
//            Local02[i] = Local01[2 * i + 1];
//            Debug.Log(Local02[i]);
//        }
//        Selection.activeTransform.localPosition = stringToVector(Local02[0]);
//        Selection.activeTransform.localEulerAngles = stringToVector(Local02[1]);
//        Selection.activeTransform.localScale = stringToVector(Local02[2]);

//    }
//    public static Vector3 stringToVector(string Local)
//    {
//        string[] tar = Local.Split(',');
//        float[] num = new float[tar.Length];
//        for (int i = 0; i < tar.Length; i++)
//        {
//            num[i] = float.Parse(tar[i]);
//        }
//        Vector3 vector3s = new Vector3(num[0], num[1], num[2]);
//        return vector3s;
//    }
//    [MenuItem("CONTEXT/SheelRev/美术/复制贝壳位置")]
//    private static void PasteWorld()
//    {
//        string world = GUIUtility.systemCopyBuffer;
//        string[] World01 = world.Split(new char[2] { '(', ')' });
//        string[] World02 = new string[3];
//        for (int i = 0; i < 3; i++)
//        {
//            World02[i] = World01[2 * i + 1];
//            Debug.Log(World02[i]);
//        }
//        //Selection.activeTransform.position = stringToVector(Local02[0]);
//        //Selection.activeTransform.eulerAngles = stringToVector(Local02[1]);
//        SheelRev sheelRev = Selection.activeGameObject.GetComponent<SheelRev>();
//        Vector3[] midPos = new Vector3[sheelRev.OceSheelPos.Length + 1];
//        for (int i = 0; i < midPos.Length - 1; i++)
//        {
//            midPos[i] = sheelRev.OceSheelPos[i];
//        }
//        midPos[midPos.Length - 1] = stringToVector(World02[0]);//这里写复制的pos
//        Debug.Log(midPos[midPos.Length - 1]);
//        sheelRev.OceSheelPos = midPos;
//        EditorUtility.SetDirty(Selection.activeGameObject);//记录物体上自定义类中的序列化数据
//        AssetDatabase.SaveAssets();
//    }
//    [MenuItem("CONTEXT/SheelRev/美术/保存")]
//    private static void PasteGameObject()
//    {

//        EditorUtility.SetDirty(Selection.activeGameObject);
//        AssetDatabase.SaveAssets();
//    }
//    [MenuItem("CONTEXT/Transform/策划/发现鱼对位置")]
//    private static void CopyTransformValues(MenuCommand command)
//    {
//        Transform transform = (Transform)command.context;
//        if (transform == null)
//            return;

//        string clipboardText = transform.localScale.x.ToString() + "," +
//                               transform.parent.localPosition.x.ToString() + "," +
//                               transform.parent.localPosition.y.ToString();

//        EditorGUIUtility.systemCopyBuffer = clipboardText;
//        Debug.Log("Transform values copied to clipboard: " + clipboardText);
//    }
//    [MenuItem("CONTEXT/Transform/策划/解锁鱼对位置")]
//    private static void CopyTransformValues01(MenuCommand command)
//    {
//        Transform transform = (Transform)command.context;
//        if (transform == null)
//            return;

//        string clipboardText = transform.localScale.x.ToString() + "," +
//                               transform.localPosition.x.ToString() + "," +
//                               transform.localPosition.y.ToString();

//        EditorGUIUtility.systemCopyBuffer = clipboardText;
//        Debug.Log("Transform values copied to clipboard: " + clipboardText);
//    }

//    public static void ApplyInverseLocalPositionToPrefab(this Transform transform, string folderPath)
//    {
//        string prefabName = transform.gameObject.name;
//        string[] prefabPaths = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

//        foreach (string prefabPath in prefabPaths)
//        {
//            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(prefabPath);
//            GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
//            if (prefab != null && prefab.name == prefabName)
//            {
//                EyeManage eyeManage = prefab.GetComponentInChildren<EyeManage>();
//                Transform prefabTransform = prefab.transform;
//                Vector3 invertedLocalPosition = -transform.localPosition;
//                eyeManage.ShowVector3 = invertedLocalPosition;
//                UnityEditor.EditorUtility.SetDirty(prefab); // 标记预制体为已修改
//                break;
//            }
//        }
//    }
//    [MenuItem("CONTEXT/Transform/策划/主界面鱼类对位置(传参给预制体)")]
//    public static void Fun()
//    {
//        string foldPath = "Assets/SharkRes/ToonyFish/Models";
//        Transform targetTransform = Selection.transforms[0];
//        Debug.Log(targetTransform.position);
//        ApplyInverseLocalPositionToPrefab(targetTransform, foldPath);
//    }
//}
