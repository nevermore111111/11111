using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detection  : MonoBehaviour
{
    public string[] targetTags;
    public List<GameObject> wasHit = new List<GameObject>();
    /// <summary>
    /// 攻击完成时清空
    /// </summary>
    public void ClaerWasHit() => wasHit.Clear();
    /// <summary>
    /// 攻击检测方法
    /// </summary>
    /// <returns></returns>
    public abstract List<Collider> GetDetection();
}
