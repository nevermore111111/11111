using Lightbug.CharacterControllerPro.Implementation;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIHittedBehavior : CharacterAIBehaviour
{
    //һ��AI�ܻ���

    //һ���ܻ��ͽ������������

    //���ݴ���������ֵȥ���㹥������Է���Ȼ����Ӧ��ȥ�����ܻ��ķ���֮��ȥ�����ܻ��ķ���ȥ���Ŷ���
    // virtual (optional)


    public bool IsDraw = true;


    //���������ܻ��ķ�ʽ��һ����ȫ��ģ�һ���ǲ��ֵģ����ֵ������ڵڶ���ֲ㶯���У�ȫ���ȥ���õ�ǰ�Ķ�����
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

    //����������ݴ������Ĺ�������ȥȷ�ϵ�ǰ�Ĺ��������λ��
    private Vector3 GetWeaponDirection( WeaponManager weapon)
    {
        //�����������෴����
        return -TransformHelper.ConvertVector(weapon.WeaponDirection,weapon.transform,transform);
    }
    private Vector3 GetWeaponPos(WeaponManager weapon)
    {

        //����������λ��
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