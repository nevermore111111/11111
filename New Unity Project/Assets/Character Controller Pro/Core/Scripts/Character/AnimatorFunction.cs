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
    public void NormalIdle()
    {
      CharacterActor.SetUpRootMotion(false,false);
    }
    public void Stop()
    {
        
        CharacterActor.Animator.SetBool("stop", false);
    }
    public void AttackEnd()
    {
        if (!CharacterActor.Animator.IsInTransition(0))
        {
            isAttack = false;
            CharacterActor.Animator.SetBool("attack", false);
        }
    }
    public void AttackStart(int num)
    {
        combo = num;
        isAttack = true;
        CharacterActor.Animator.SetBool("attack", true);
        canChangeState = false;
        OnceAttack = false;
        CharacterActor.Animator.SetInteger("specialAttack", 0);
        //if(Input.GetButton("Lock"))
        //{
        //    CharacterActor.Forward = Vector3.zero;
        //}
        if (mainCharacter.enemys.Count != 0)
        {
            //���﷨
            GameObject[] gamesEnemy = mainCharacter.enemys.Select(m => m.gameObject).ToArray();
            
            mainCharacter.selectEnemy = HelpTools.FindClosest(CharacterActor.gameObject, gamesEnemy).GetComponent<CharacterInfo>();
            Vector3 Forward = (mainCharacter.selectEnemy.transform.position - CharacterActor.transform.position).normalized;
            CharacterActor.Forward = new(Forward.x,0,Forward.z);
        }
        else
        {
            //û�е�λ�Ϳ�������ת�򣬵���ֻ���ڹ�����ʼ��ʱ��ת��
            CharacterActor.Forward = CharacterStateController.InputMovementReference;
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
