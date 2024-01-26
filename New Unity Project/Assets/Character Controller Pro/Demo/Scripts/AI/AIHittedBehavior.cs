using Lightbug.CharacterControllerPro.Implementation;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIHittedBehavior : CharacterAIBehaviour
{
    //一个AI受击类

    //一旦受击就进入这个方法。

    //根据传进来的数值去计算攻击的相对方向，然后相应的去设置受击的方向，之后去根据受击的方向去播放动画
    // virtual (optional)


    public bool IsDraw = true;


    //增加两种受击的方式，一种是全面的，一种是部分的，部分的设置在第二层分层动画中，全面的去设置当前的动画层
    public override void EnterBehaviour(float dt)
    {

    }

    // abstract (mandatory)
    public override void UpdateBehaviour(float dt)
    {
    }

    // virtual (optional)
    public override void ExitBehaviour(float dt)
    {
    }

    //这个方法根据传进来的攻击方向去确认当前的攻击方向和位置
    private Vector3 GetWeaponDirection( WeaponManager weapon)
    {
        //返回武器的相反方向
        return -TransformHelper.ConvertVector(weapon.WeaponWorldDirection,weapon.transform,transform);
    }
    private Vector3 GetWeaponPos(WeaponManager weapon)
    {

        //返回武器的位置
        return transform.InverseTransformPoint(weapon.transform.position);
    }
    //private void OnDrawGizmos()
    //{
    //    if(IsDraw)
    //    {
    //        Gizmos.DrawSphere();
    //    }
    //}
}