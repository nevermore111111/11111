using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class  TransformHelper:MonoBehaviour
{
    //һ��������
   
    /// <summary>
    /// ��һ��������Դ����ϵת��ΪĿ������ϵ
    /// </summary>
    /// <param name="sourceVector"></param>
    /// <param name="sourceTransform"></param>
    /// <param name="targetTransform"></param>
    /// <returns></returns>
    public static Vector3  ConvertVector(Vector3 sourceVector, Transform sourceTransform, Transform targetTransform)
    {
        // ��Դ������Դ����ϵת��Ϊ��������ϵ
        Vector3 worldVector = sourceTransform.TransformDirection(sourceVector);

        // ����������ϵ�е�����ת��ΪĿ������ϵ�е�����
        Vector3 targetVector = targetTransform.InverseTransformDirection(worldVector);

        return targetVector;
    }

    /// <summary>
    /// ��һ�����ԭ����ϵת��Ϊ��ǰ����ϵ�еĵ�
    /// </summary>
    /// <param name="originalPoint"></param>
    /// <param name="originalTransform"></param>
    /// <param name="currentTransform"></param>
    /// <returns></returns>
    public static Vector3 ConvertPoint(Vector3 originalPoint, Transform originalTransform, Transform currentTransform)
    {
        // ��ԭ���ԭ����ϵת������������ϵ
        Vector3 worldPoint = originalTransform.TransformPoint(originalPoint);

        // ����������ϵ�еĵ�ת��Ϊ��ǰ����ϵ�еĵ�
        Vector3 currentPoint = currentTransform.InverseTransformPoint(worldPoint);

        return currentPoint;
    }
}
