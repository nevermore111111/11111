using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AnimatorFunction : Attack
{

    public void Idle()
    {
        if (isJustEnter)
        {
            isJustEnter = false;
            combo = 1;
            CharacterActor.Animator.SetInteger("combo",combo);
            canChangeState = false;
        }
        else
        {
            isAttack =false;
            CharacterActor.Animator.SetBool("attack", false);
            combo = 0;
            CharacterActor.Animator.SetInteger("combo", combo);
            canInput = true;
            canChangeState = true;
        }
    }


    public void AttackEnd()
    {
        isAttack=false;
        CharacterActor.Animator.SetBool("attack",false);
    }
    public void AttackStart()
    {
        isAttack = true;
        CharacterActor.Animator.SetBool("attack", true);
        canChangeState = false;
        if(selectEnemy = null)
        {

        }
        else
        {
            //CharacterActor.Forward =( selectEnemy.transform.position - CharacterActor.transform.position).normalized;
        }
    }
    public void CanGetInput()
    {
        canInput = true;
    }
    public void CannotGetInput()
    {
        combo = 0;
        CharacterActor.Animator.SetInteger("combo",0);
    }
    /// <summary>
    /// 在一个连招开始时执行，定义这个连招的最大连击数量。
    /// </summary>
    public void ComboStart(int num)
    {
        MaxCombo = num;
        CharacterActor.Animator.SetInteger("specialAttack", 0);
    }
    public void SpAtk(int kind)
    {
        CharacterActor.Animator.SetInteger("specialAttack", kind);
    }
}
