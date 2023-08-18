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
    /// 攻击完成时清空击中列表，击中判定改为FALSE
    /// </summary>
    public void ClaerWasHit()
    {
        wasHit.Clear();
        isHited = false;
        Weapon.HittedCharacter.Clear();
    }
    /// <summary>
    /// 攻击检测方法，传入当前的isHited变量，表示这个detection是否击中了目标
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
    //前四个属于fist的
    arm01 = 4,
    //这个属于sword
    arm02 = 5,
}

