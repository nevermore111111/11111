using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HelperTools : MonoBehaviour
{

   // [MenuItem("Assets/执行测试")]
    static void Test()
    {
        Debug.Log(GetSelectedAssetPath());
    }
    static void RetsetTransform(Transform transform)
    {
        transform.position = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.localScale = Vector3.one;
    }
    /// <summary>
    /// 返回路径
    /// </summary>
    /// <returns></returns>
    public static string GetSelectedAssetPath()
    {
        string path = "";
        Object[] objs = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        if (objs.Length > 0)
        {
            path = AssetDatabase.GetAssetPath(objs[0]);
        }

        return path;
    }
    /// <summary>
    /// 转换目标向量，原本的transform到现在的transform
    /// </summary>
    /// <param name="vec3">目标向量（原本坐标系下）</param>
    /// <param name="from">原本坐标系</param>
    /// <param name="tar">现在的坐标系</param>
    /// <returns></returns>
    public static Vector3 ChangeVector(Vector3 vec3,Transform from,Transform tar)
    {
        Vector3 targetVec = new Vector3();
        targetVec = from.TransformDirection(vec3);
        targetVec = tar.InverseTransformVector(targetVec);
        return targetVec;
    }
}
