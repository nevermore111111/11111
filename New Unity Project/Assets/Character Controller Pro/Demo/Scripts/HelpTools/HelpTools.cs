using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// 
/// <summary>
public class HelpTools : MonoBehaviour
{
    /// <summary>
    /// 寻找在objects中距离target最近的物体
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
}
