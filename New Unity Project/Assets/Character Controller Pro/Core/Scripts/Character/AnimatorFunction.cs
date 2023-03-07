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
            //�����ڹ����е�ʱ�򣬲��ᱻ��һ�ε�CannotGetInput����combo
            combo = 0;
            CharacterActor.Animator.SetInteger("combo", 0);
        }
    }
    /// <summary>
    /// ��һ�����п�ʼʱִ�У�����������е��������������
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
