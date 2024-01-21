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

            Vector3 _tar=  GetLocaltransForm(SelectTra.transform.forward, tarTransForm);//�������actor����ϵ�µķ���
            Debug.Log($"{_tar.x:F2},{_tar.y:F2},{_tar.z:F2}");
        }
    }

    private static Vector3 GetLocaltransForm( Vector3 vector3ToChange,Transform targetTransfom)
    {
        return targetTransfom.InverseTransformDirection(vector3ToChange);
    }

    //private static void DrawDir(Vector3 startPos,Vector3 endPos,int type = 1,Transform AxisTransform = null)
    //{
    //    Gizmos.color = Color.green;
    //    if (type != 1) 
    //    {
    //        Gizmos.matrix = Matrix4x4.TRS(AxisTransform.position, AxisTransform.rotation, Vector3.one);
    //    }
    //    Gizmos.DrawLine(startPos, endPos);
    //}
}
