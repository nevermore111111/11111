using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Detection  : MonoBehaviour
{
    public string[] targetTags;
    public List<GameObject> wasHit = new List<GameObject>();
    public bool isHited;
    public WeaponManager Weapon;
    public WeaponDetector WeaponDetector = WeaponDetector.arm01;
    
    /// <summary>
    /// �������ʱ��ջ����б������ж���ΪFALSE
    /// </summary>
    public void ClaerWasHit()
    {
        wasHit.Clear();
        isHited = false;
        Weapon.HittedCharacter.Clear();
    }
    /// <summary>
    /// ������ⷽ�������뵱ǰ��isHited��������ʾ���detection�Ƿ������Ŀ��
    /// </summary>
    /// <returns></returns>
    public abstract List<Collider>  GetDetection(out bool isHited);
}
public enum WeaponDetector
{
    leftHand = 0,
    rightHand = 1,
    letfFoot = 2,
    rightFoot = 3,
    //ǰ�ĸ�����fist��
    arm01 = 4,
    //�������sword
    arm02 = 5,
}

