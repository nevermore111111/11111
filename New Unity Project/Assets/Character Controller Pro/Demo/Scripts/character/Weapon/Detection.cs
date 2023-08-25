using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Detection : MonoBehaviour
{
    public string[] targetTags;
    public List<GameObject> wasHit = new List<GameObject>();
    public bool isHited;
    public WeaponManager Weapon;//�������̽����������ļ��
    public WeaponDetector WeaponDetector = WeaponDetector.sword;


    public void Awake()
    {
        WeaponManager[] weapons = GetComponentsInParent<WeaponManager>();
        if ((int)WeaponDetector < 4)//�����ͽ�ֵ�
        {
            Weapon = weapons.FirstOrDefault(_ => _.kind == WeaponKind.fist);
        }
        else if ((int)WeaponDetector == 4)
        {
            Weapon = weapons.FirstOrDefault(_ => _.kind == WeaponKind.sword);
        }
    }


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
    public abstract List<Collider> GetDetection(out bool isHited);
}
public enum WeaponDetector
{
    leftHand = 0,
    rightHand = 1,
    letfFoot = 2,
    rightFoot = 3,
    //ǰ�ĸ�����fist��
    sword = 4,
    //�������sword
    arm02 = 5,
}

