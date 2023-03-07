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
    [SerializeField]
    private MainCharacter mainCharacter;
    
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
        OnceAttack = false;
        if (mainCharacter.enemys.Count != 0)
        {
            mainCharacter.selectEnemy = HelpTools.FindClosest(CharacterActor.gameObject, mainCharacter.enemys);
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - CharacterActor.transform.position).normalized;
            CharacterActor.Forward = new(Forward.x,0,Forward.z);
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
        if (!isAttack)
        {
            //这样在攻击中的时候，不会被上一次的CannotGetInput重置combo
            combo = 0;
            CharacterActor.Animator.SetInteger("combo", 0);
        }
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
