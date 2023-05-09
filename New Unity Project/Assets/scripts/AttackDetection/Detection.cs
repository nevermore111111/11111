using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detection  : MonoBehaviour
{
    public string[] targetTags;
    public List<GameObject> wasHit = new List<GameObject>();
    /// <summary>
    /// �������ʱ���
    /// </summary>
    public void ClaerWasHit() => wasHit.Clear();
    /// <summary>
    /// ������ⷽ��
    /// </summary>
    /// <returns></returns>
    public abstract List<Collider> GetDetection();
}
