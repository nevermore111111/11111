using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NUnit.Framework.Internal;

using UnityEditor.Animations;

public class MoveFile : MonoBehaviour
{
    [MenuItem("Assets/策划/这个方法去分配动画")]
    private static void MoveAnim()
    {
        //需要两个目标，一个是来自目标，另外一个是放进去的目标；
        string[] animPath = Selection.assetGUIDs;
        string[] path = new string[animPath.Length];//来源目标
        string[] fishName = new string[path.Length];
        string[] fromTar = new string[path.Length];
        string[] moveTar = new string[path.Length];
        string[] moveTarControl = new string[path.Length];
        AnimatorController[] anim = new AnimatorController[path.Length];
        for (int i = 0; i < animPath.Length; i++)
        {
            path[i] = AssetDatabase.GUIDToAssetPath(animPath[i]);
            fromTar[i] = AssetDatabase.LoadMainAssetAtPath(path[i]).name;//fish@xxxx
            fishName[i] = fromTar[i].Split("@")[0];
            moveTar[i] = "Assets/SharkRes/ToonyFish/Models/" + fishName[i] + "/" + fromTar[i] + ".fbx";
            moveTarControl[i] = "Assets/SharkRes/ToonyFish/Models/" + fishName[i] + "/" + fishName[i] + ".controller";
            anim[i] = AssetDatabase.LoadMainAssetAtPath(moveTarControl[i]) as AnimatorController;
            Object objFrom = AssetDatabase.LoadMainAssetAtPath(path[i]);
            Object objTo = AssetDatabase.LoadMainAssetAtPath(moveTar[i]);
            if (objFrom != null && objTo != null && objFrom.name == objTo.name)
            {
                FileUtil.ReplaceFile(path[i], moveTar[i]);
                //增加一个方法，去检查
            }
            else
            {
                Debug.Log(fromTar[i] +"没找到，或者对应文件没找到");
            }
            AssetDatabase.Refresh();
            int layer = 0;
            if (fromTar[i].Split("@")[1] == "attack")
            {
                layer = 1;
            }
            if (fromTar[i].Split("@")[1].Contains("eye"))
            {
                layer = 2;
            }
            //这里需要找到新的动画
            ChildAnimatorState[] states = anim[i].layers[layer].stateMachine.states;
            for (int j = 0; j < states.Length; j++)
            {
                if (states[j].state.name == fromTar[i].Split("@")[1])
                {//判定一下对应名字的物体的动画是不是空的，如果是空的，那么去指定一下
                    if (states[j].state.motion == null)
                    {
                        Debug.Log(fishName[i] + $"Controller缺失动画{fromTar[i].Split("@")[1]}，已经修复");
                        Motion motion = AssetDatabase.LoadAssetAtPath<Motion>(moveTar[i]);
                        states[j].state.motion = motion;
                    }
                }
            }
        }
    }
    [MenuItem("Assets/策划/啊啊啊啊啊啊啊啊")]
    private static void Test()
    {
        ////"Assets/SharkRes/ToonyFish/Models/F_3030/F_3030@attack3.fbx"
        ////"Assets/SharkRes/ToonyFish/test/FC_3003@attack.fbx"
        ////"Assets/SharkRes/ToonyFish/Models/F_3007/F_3007@attack.fbx"
        ////AvatarMask mask;
        ////mask = AssetDatabase.LoadMainAssetAtPath( AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0])) as AvatarMask;

        ////检测动画层中是否含有xxx动画

        ////需要两个目标，一个是来自目标，另外一个是放进去的目标；
        //string[] animPath = Selection.assetGUIDs;
        //string[] path = new string[animPath.Length];//来源目标
        //string[] fishName = new string[path.Length];
        //string[] fromTar = new string[path.Length];
        //string[] moveTar = new string[path.Length];
        //string[] moveTarControl = new string[path.Length];
        //AnimatorController[] anim = new AnimatorController[path.Length];
        //for (int i = 0; i < animPath.Length; i++)
        //{
        //    path[i] = AssetDatabase.GUIDToAssetPath(animPath[i]);
        //    fromTar[i] = AssetDatabase.LoadMainAssetAtPath(path[i]).name;//fish@xxxx
        //    fishName[i] = fromTar[i].Split("@")[0];
        //    moveTar[i] = "Assets/SharkRes/ToonyFish/Models/" + fishName[i] + "/" + fromTar[i] + ".fbx";
        //    moveTarControl[i] = "Assets/SharkRes/ToonyFish/Models/" + fishName[i] + "/" + fishName[i] + ".controller";
        //    anim[i] = AssetDatabase.LoadMainAssetAtPath(moveTarControl[i]) as AnimatorController;
        //    Object objFrom = AssetDatabase.LoadMainAssetAtPath(path[i]);
        //    Object objTo = AssetDatabase.LoadMainAssetAtPath(moveTar[i]);
        //    if (objFrom != null && objTo != null && objFrom.name == objTo.name)
        //    {
        //        //FileUtil.ReplaceFile(path[i], moveTar[i]);
        //    }
        //    else
        //    {
        //        Debug.Log(fromTar[i] + "没找到，或者对应文件没找到");
        //    }
        //    //检查一下是不是attack，如果是attack，那么就在第2层去找attack，检查一下是不是空
        //    //否则在第一层检查是不是空，如果是空，就指定一下
        //    //ChildAnimatorState[] state =  anim[i].layers[1].stateMachine.states;
        //    //for(int j = 0; j < state.Length; j++)
        //    //{
        //    //    Debug.Log(state[j].state.motion );
        //    //}
        //    int layer = 0;
        //    if (fromTar[i].Split("@")[1] == "attack")
        //    {
        //        layer = 1;
        //    }
        //    //这里需要找到新的动画
        //    ChildAnimatorState[] states = anim[i].layers[layer].stateMachine.states;
        //    for (int j = 0; j < states.Length; j++)
        //    {
        //        if (states[j].state.name == fromTar[i].Split("@")[1])
        //        {//判定一下对应名字的物体的动画是不是空的，如果是空的，那么去指定一下
        //            if (states[j].state.motion == null)
        //            {
        //                Debug.Log(fromTar[i]+"Controller缺失动画，已经修复");
        //                Motion motion = AssetDatabase.LoadAssetAtPath<Motion>(moveTar[i]);
        //                states[j].state.motion = motion;
        //            }
        //        }
        //    }
        //}
    }
}
