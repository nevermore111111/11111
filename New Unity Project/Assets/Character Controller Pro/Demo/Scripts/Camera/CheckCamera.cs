using UnityEngine;

public class CheckCamera : MonoBehaviour
{
    /// 检查摄像机夹角是否满足切换条件 <summary>
    /// 检查摄像机夹角是否满足切换条件
    /// </summary>
    /// <returns></returns>
    public bool ShouldSwitchToSubCamera()
    {
        // 如果夹角sin值>于0.5，返回true，否则返回false.<0.5的话，是不能转换的
        return Mathf.Sin(Vector3.Angle(transform.forward, Vector3.up)) > 0.5f;//这个值越大，容忍范围越小
    }
}
