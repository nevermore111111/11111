using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lightbug.CharacterControllerPro.Core;
using System;

public class GetWeaponDir : MonoBehaviour
{
    [MenuItem("CONTEXT/Transform/����/ѡ������һ������ǰ����ת��Ϊ������������", false, 100)]
    static void GetDirInCharacterLocal()
    {
        GameObject SelectTra = Selection.gameObjects[0];
        CharacterActor actor = SelectTra.GetComponentInParent<CharacterActor>();
        Transform tarTransForm = actor.transform;
        if (tarTransForm != null)
        {
            //�Ȱѵ�ǰ�����������ת����ѡ���������Ȼ���ڳ����л��������������

            GetLocaltransForm(SelectTra.transform.forward, tarTransForm);//�������actor����ϵ�µķ���
            DrawDir(SelectTra.transform.position,SelectTra.transform.position + SelectTra.transform.forward);//ԭ���
            Vector3 posInActor = tarTransForm.InverseTransformPoint(SelectTra.transform.position);
            DrawDir(posInActor, posInActor + GetLocaltransForm(SelectTra.transform.forward, tarTransForm), 2, tarTransForm);
        }
    }

    private static Vector3 GetLocaltransForm( Vector3 vector3ToChange,Transform targetTransfom)
    {
        return targetTransfom.InverseTransformDirection(vector3ToChange);
    }

    private static void DrawDir(Vector3 startPos,Vector3 endPos,int type = 1,Transform AxisTransform = null)
    {
        Gizmos.color = Color.green;
        if (type != 1) 
        {
            Gizmos.matrix = Matrix4x4.TRS(AxisTransform.position, AxisTransform.rotation, Vector3.one);
        }
        Gizmos.DrawLine(startPos, endPos);
    }
}
