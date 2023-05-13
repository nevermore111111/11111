using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


public class ClipFrame : MonoBehaviour
{
    static string SmallFish;
    static string BigFish;
    //最大鱼类数量
    static readonly int fishNum = 46;
    
    //  ///////"Assets/SharkRes/ToonyFish/Models/" + FishName + "/" + FishName + "@idle.fbx";
    [MenuItem("Assets/策划/查看所有鱼的Frame")]
    private static void CheckFrame()
    {
        AnimatorController SmallAnimatorController;
        int Num = 1;
        for (int i = 0; i < fishNum; i++)
        {

            SmallFish = "FC_" + (3000 + Num).ToString();
            BigFish = "F_" + (3000 + Num).ToString();
            Num++;
            string path01 = "Assets/SharkRes/ToonyFish/Models/" + SmallFish + "/" + SmallFish + ".controller";
            SmallAnimatorController = AssetDatabase.LoadMainAssetAtPath(path01) as AnimatorController;
            if (SmallAnimatorController == null)
            {
                Debug.Log(SmallFish+"没有动画机");
            }
            if (SmallAnimatorController != null)
            {


                AnimatorControllerParameter[] parameters = SmallAnimatorController.parameters;

                AnimatorControllerParameter[] parameters2 = { };
                foreach (AnimatorControllerParameter parameter in parameters)
                {

                    if (parameter.name == "eyelid_blink_speed")
                    {
                        parameter.name = "eyelid_blink_speed";
                        // parameter.defaultBool = true;
                        parameter.defaultFloat = 1f;
                    }
                }
                SmallAnimatorController.parameters = parameters;
                for (int k = 0; k < SmallAnimatorController.layers.Length; k++)
                {
                    ChildAnimatorState[] smallStates = SmallAnimatorController.layers[k].stateMachine.states;
                    for (int j = 0; j < smallStates.Length; j++)
                    {
                        //if (smallStates[j].state.name .Contains("attack") & smallStates[j].state.name!="attack")
                        //{
                        //    AnimationClip clip = smallStates[j].state.motion as AnimationClip;
                        //    float Frame = clip.length * clip.frameRate;
                        //    if (Frame > 35.5f || Frame < 25)
                        //    {
                        //        Debug.Log(SmallFish + "的" + smallStates[j].state.name + "帧数" + Frame);

                        //    }
                        //}

                        if (smallStates[j].state.motion == null & smallStates[j].state.name != "dead"&smallStates[j].state.name != "New State")
                        {

                            Debug.Log(SmallFish + smallStates[j].state.name);
                        }
                    }
                }
                   
            }



            string path02 = "Assets/SharkRes/ToonyFish/Models/" + BigFish + "/" + BigFish + ".controller";
            SmallAnimatorController = AssetDatabase.LoadMainAssetAtPath(path02) as AnimatorController;
            if (SmallAnimatorController == null)
            {
                Debug.Log(BigFish+"没有动画机");
            }
            if (SmallAnimatorController != null)
            {
                AnimatorControllerParameter[] parameters = SmallAnimatorController.parameters;

                AnimatorControllerParameter[] parameters2 = { };
                foreach (AnimatorControllerParameter parameter in parameters)
                {

                    if (parameter.name == "eyelid_blink_speed")
                    {
                        parameter.name = "eyelid_blink_speed";
                        // parameter.defaultBool = true;
                        parameter.defaultFloat = 1f;
                    }
                }
                SmallAnimatorController.parameters = parameters;
                for (int k = 0; k < SmallAnimatorController.layers.Length; k++)
                {
                    ChildAnimatorState[] bigStates = SmallAnimatorController.layers[k].stateMachine.states;
                    for (int j = 0; j < bigStates.Length; j++)
                    {
                        //if (bigStates[j].state.name.Contains("attack") & bigStates[j].state.name !="attack")
                        //{
                        //    AnimationClip clip = bigStates[j].state.motion as AnimationClip;
                        //    float Frame = clip.length * clip.frameRate;
                        //    if (Frame > 35.5f || Frame < 25)
                        //    {
                        //        Debug.Log(BigFish + "的" + bigStates[j].state.name +"帧数" + Frame);
                        //    }
                        //}
                        if (bigStates[j].state.motion == null & bigStates[j].state.name !="dead"& bigStates[j].state.name !="New State")
                        {
                            Debug.Log(BigFish + bigStates[j].state.name);
                        }
                    }
                }

                    
            }
        }
    }
    [MenuItem("Assets/策划/修正鱼类的动画")]
    private static void FixClip()
    {
        AnimatorController SmallAnimatorController;
        int Num = 1;
        for (int i = 0; i < fishNum; i++)
        {

            SmallFish = "FC_" + (3000 + Num).ToString();
            BigFish = "F_" + (3000 + Num).ToString();
            Num++;
            string path01 = "Assets/SharkRes/ToonyFish/Models/" + SmallFish + "/" + SmallFish + ".controller";
            SmallAnimatorController = AssetDatabase.LoadMainAssetAtPath(path01) as AnimatorController;
            if (SmallAnimatorController == null)
            {
                Debug.Log(SmallFish + "没有动画机");
            }
            if (SmallAnimatorController != null)
            {
                for (int k = 0; k    < SmallAnimatorController.layers.Length; k++)
                {
                    ChildAnimatorState[] smallStates = SmallAnimatorController.layers[k].stateMachine.states;
                    for (int j = 0; j < smallStates.Length; j++)
                    {
                        //if (smallStates[j].state.name .Contains("attack") & smallStates[j].state.name!="attack")
                        //{
                        //    AnimationClip clip = smallStates[j].state.motion as AnimationClip;
                        //    float Frame = clip.length * clip.frameRate;
                        //    if (Frame > 35.5f || Frame < 25)
                        //    {
                        //        Debug.Log(SmallFish + "的" + smallStates[j].state.name + "帧数" + Frame);

                        //    }
                        //}

                        if (smallStates[j].state.motion == null & smallStates[j].state.name != "dead"& smallStates[j].state.name != "New State")
                        {

                            Debug.Log(SmallFish + smallStates[j].state.name);
                            string path03 = "Assets/SharkRes/ToonyFish/Models/" + SmallFish + "/" + SmallFish + "@" + smallStates[j].state.name + ".fbx";
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path03);
                            if (clip != null)
                            {
                                smallStates[j].state.motion = clip;
                            }
                            else
                            {
                                Debug.Log(SmallFish + smallStates[j].state.name+"未找到修正动画");
                            }
                        }
                        
                    }
                }
            }



            string path02 = "Assets/SharkRes/ToonyFish/Models/" + BigFish + "/" + BigFish + ".controller";
            SmallAnimatorController = AssetDatabase.LoadMainAssetAtPath(path02) as AnimatorController;
            if (SmallAnimatorController == null)
            {
                Debug.Log(BigFish + "没有动画机");
            }
            if (SmallAnimatorController != null)
            {
                for (int k = 0; k < SmallAnimatorController.layers.Length; k++)
                {
                    ChildAnimatorState[] bigStates = SmallAnimatorController.layers[k].stateMachine.states;
                    for (int j = 0; j < bigStates.Length; j++)
                    {
                        //if (bigStates[j].state.name.Contains("attack") & bigStates[j].state.name !="attack")
                        //{
                        //    AnimationClip clip = bigStates[j].state.motion as AnimationClip;
                        //    float Frame = clip.length * clip.frameRate;
                        //    if (Frame > 35.5f || Frame < 25)
                        //    {
                        //        Debug.Log(BigFish + "的" + bigStates[j].state.name +"帧数" + Frame);
                        //    }
                        //}
                        if (bigStates[j].state.motion == null & bigStates[j].state.name != "dead" & bigStates[j].state.name != "New State")
                        {
                            Debug.Log(BigFish + bigStates[j].state.name);
                            //设置动画
                            string path03 = "Assets/SharkRes/ToonyFish/Models/" + BigFish + "/" + BigFish + "@" + bigStates[j].state.name + ".fbx";
                            AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path03);
                            if(clip != null)
                            {
                                bigStates[j].state.motion = clip;
                            }
                            else
                            {
                                Debug.Log(BigFish + bigStates[j].state.name+"未找到修改正动画");
                            }
                           
                        }
                    }
                    
                }
            }
        }
    }
    [MenuItem("Assets/策划/修改眨眼参数")]
    private static void Fun01()
    {
        UnityEditor.Animations.AnimatorController controller = Selection.objects[0] as UnityEditor.Animations.AnimatorController;

        AnimatorControllerParameter[] parameters = controller.parameters;

        AnimatorControllerParameter[] parameters2 = { };
        foreach (AnimatorControllerParameter parameter in parameters)
        {

            if (parameter.name == "eyelid_blink_speed")
            {
                parameter.name = "eyelid_blink_speed";
                // parameter.defaultBool = true;
                parameter.defaultFloat = 1f;
            }
        }
        //controller.parameters = parameters;
    }
}

