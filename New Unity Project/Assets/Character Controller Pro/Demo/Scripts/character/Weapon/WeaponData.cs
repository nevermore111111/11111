using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
public class WeaponData : MonoBehaviour
{
    [Space(10)]
    [Header("是否只应用垂直方向震动")]
    public bool onlyUseVirticalShake = false;
    [Space(10)]
    [Tooltip("是否使用dotween的震动")]
    public bool isUseDotweenShake = false;
    [Space(10)]
    [Tooltip("是否忽略z的震动")]
    public bool isIgnoreZshake = true;

    [Header("是否打印攻击类型")]
    public bool PrintHit = true;

    public bool isNeedChangeCurrentHit = false;
    public int ChangeCurrentHitNum = -1;



    [Header("特殊攻击配置")]
    [SerializeField]
    [Range(0f, 1f)]
    public float sp11Duration = 1.0f;
    [SerializeField]
    [Range(0f, 5f)]
    public float sp11Force = 1.0f;

    public CinemachineImpulseDefinition.ImpulseShapes ImpulseShapes = CinemachineImpulseDefinition.ImpulseShapes.Bump;
    public float DropSpeed = 10f;
    public List<WeaponNum> weaponNumList = new List<WeaponNum>();

}
[System.Serializable]
public class WeaponNum
{
    public float Strength = 0.3f;
    public float Frequence = 1f;
    public float Duration = 0.3f;

    // 可以添加构造函数和其他方法，根据需要进行扩展
}


