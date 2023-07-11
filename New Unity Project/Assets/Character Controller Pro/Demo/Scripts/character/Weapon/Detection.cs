using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detection  : MonoBehaviour
{
    public string[] targetTags;
    public List<GameObject> wasHit = new List<GameObject>();
    public bool isHited;
    public WeaponManager WeaponManagerOwner;
    /// <summary>
    /// �������ʱ��ջ����б������ж���ΪFALSE
    /// </summary>
    public void ClaerWasHit()
    {
        wasHit.Clear();
        isHited = false;
        WeaponManagerOwner.HittedCharacter.Clear();
    }
    /// <summary>
    /// ������ⷽ�������뵱ǰ��isHited��������ʾ���detection�Ƿ������Ŀ��
    /// </summary>
    /// <returns></returns>
    public abstract List<Collider>  GetDetection(out bool isHited);
}


