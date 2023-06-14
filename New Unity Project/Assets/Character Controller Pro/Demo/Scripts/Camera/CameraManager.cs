using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///摄像机切换逻辑
///在人物攻击且范围内存在敌人的时候
///在人物受到攻击的时候
///返回正常视角的时间
///当人物自由移动或者范围内没有敌人的时候，持续1.5s后，返回自由视角。
///转动屏幕的时候，立刻返回自动相机
/// </summary>
public class CameraManager : MonoBehaviour
{


    public CinemachineFreeLook Maincamera;

}
