using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 
/// <summary>
public class HelpTools01 : MonoBehaviour
{
    /// <summary>
    /// Ѱ����objects�о���target���������
    /// </summary>
    /// <param name="target"></param>
    /// <param name="objects"></param>
    /// <returns></returns>
    public static GameObject FindClosest(GameObject target, GameObject[]objects)
    {
        float closestDistance = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }

        return closest;
    }
    public static GameObject FindClosest(GameObject target, List<GameObject> objects)
    {
        float closestDistance = float.MaxValue;
        GameObject closest = null;

        foreach (GameObject obj in objects)
        {
            float distance = Vector3.Distance(obj.transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = obj;
            }
        }

        return closest;
    }
    /// <summary>
    /// ���ؽӴ�����ײ�У�
    /// </summary>
    /// <param name="startPoint"></param>
    /// <param name="endpoint"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    //public static Collider[] OverlapBoxByPoint(Vector3 startPoint ,Vector3 endpoint,float height ,float width)
    //{
    //    //����ߣ�
    //    Vector3 center = 0.5f*(startPoint+endpoint);
    //    float length = Vector3.Magnitude(startPoint - endpoint);
    //    Vector3 halfExtents = new Vector3(length / 2f, width / 2f, height / 2f);
    //    Physics.OverlapBox(center, halfExtents)
    //}
}
