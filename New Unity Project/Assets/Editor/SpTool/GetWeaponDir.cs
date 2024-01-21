using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lightbug.CharacterControllerPro.Core;
using System;

public class GetWeaponDir : MonoBehaviour
{
    [MenuItem("CONTEXT/Transform/武器/选择武器一个的正前方，转化为相对人物的坐标", false, 100)]
    static void GetDirInCharacterLocal()
    {
        GameObject SelectTra = Selection.gameObjects[0];
        CharacterActor actor = SelectTra.GetComponentInParent<CharacterActor>();
        Transform tarTransForm = actor.transform;
        if (tarTransForm != null)
        {
            //先把当前物体的正方向转化到选择的正方向，然后在场景中绘制物体的正方向

            Vector3 _tar=  GetLocaltransForm(SelectTra.transform.forward, tarTransForm);//这个是在actor坐标系下的方向
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
