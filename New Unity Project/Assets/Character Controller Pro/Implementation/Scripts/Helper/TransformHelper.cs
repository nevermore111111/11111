using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class  TransformHelper:MonoBehaviour
{
    //一个工具类
   
    /// <summary>
    /// 将一个向量从源坐标系转换为目标坐标系
    /// </summary>
    /// <param name="sourceVector"></param>
    /// <param name="sourceTransform"></param>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    public static Vector3  ConvertVector(Vector3 sourceVector, Transform sourceTransform, Transform targetTransform)
    {
        // 将源向量从源坐标系转换为世界坐标系
        Vector3 worldVector = sourceTransform.TransformDirection(sourceVector);

        // 将世界坐标系中的向量转换为目标坐标系中的向量
        Vector3 targetVector = targetTransform.InverseTransformDirection(worldVector);

        return targetVector;
    }

    /// <summary>
    /// 将一个点从原坐标系转换为当前坐标系中的点
    /// </summary>
    /// <param name="originalPoint"></param>
    /// <param name="originalTransform"></param>
    /// <param name="currentTransform"></param>
    /// <returns></returns>
    public static Vector3 ConvertPoint(Vector3 originalPoint, Transform originalTransform, Transform currentTransform)
    {
        // 将原点从原坐标系转换到世界坐标系
        Vector3 worldPoint = originalTransform.TransformPoint(originalPoint);

        // 将世界坐标系中的点转换为当前坐标系中的点
        Vector3 currentPoint = currentTransform.InverseTransformPoint(worldPoint);

        return currentPoint;
    }
}
