using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.Drawing.Text;

public class FbxChange : MonoBehaviour
{
    [MenuItem("Assets/策划/给对应的fbx设置loop，这个方法没用")]
    static void ChangeLoop()
    {
        ModelImporter importer;
        string[] FbxStrs = Selection.assetGUIDs;
        for(int i = 0; i < FbxStrs.Length; i++)
        {
            importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(FbxStrs[i])) as ModelImporter;
            Debug.Log(importer.name);
        }
    }

    public static void ChangeFbxLoop(string FishName)
    {
        string[] tarPaths = new string[3];
        tarPaths[0]= "Assets/SharkRes/ToonyFish/Models/" + FishName + "/" + FishName + "@idle.fbx";
        tarPaths[1] = "Assets/SharkRes/ToonyFish/Models/" + FishName + "/" + FishName + "@move.fbx";
        tarPaths[2] = "Assets/SharkRes/ToonyFish/Models/" + FishName + "/" + FishName + "@speedUp.fbx";
        ModelImporter model;
        for(int j = 0; j < tarPaths.Length; j++)
        {
            object obj = AssetDatabase.LoadMainAssetAtPath(tarPaths[j]);
            Debug.Log(obj);
            model = AssetImporter.GetAtPath(tarPaths[j]) as ModelImporter;
            model.defaultClipAnimations[0].loopTime = true;
        }
    }
    [MenuItem("Assets/策划/SetLoop")]
    private static void SetLoopEx()
    {
        ModelImporter model;
        model =  AssetImporter.GetAtPath( AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0]))) as ModelImporter;
        model.clipAnimations = model.defaultClipAnimations;
        model.clipAnimations[0].loopTime = true;
        Debug.Log(model.clipAnimations[0].name);
        Debug.Log(model.clipAnimations[0].loopTime);
        Debug.Log(model.clipAnimations.Length);
    }





    //public class AddAnimEvent
    //{
    //    public static string animationEventName = "OnAnimEvent";
    //    public static string[] eventForSingleAttack = new[] { "meleeDamageJudge", "reset", "attackEnd" };
    //    public static string[] eventForDoubleAttack = new[] { "meleeDamageJudge", "resetOnly", "reset", "attackEnd" };
    //    public static string[] eventForTrippleAttack = new[] { "meleeDamageJudge", "resetOnly", "resetOnly", "reset", "attackEnd" };

    //    //这个是用来验证，选中的是不是模型，有没有动画的。
    //    [MenuItem("Assets/Add Attack Event", true)]
    //    private static bool ValidAddAttackEvent()
    //    {
    //        if (Selection.activeObject)
    //        {
    //            ModelImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)) as ModelImporter;
    //            if (importer)
    //            {
    //                ModelImporterClipAnimation[] animation = importer.clipAnimations;
    //                if (animation.Length > 0)
    //                {
    //                    return true;
    //                }
    //            }
    //        }
    //        return false;
    //    }

    //    [MenuItem("Assets/Add Attack Event")]
    //    private static void AddAttackEvent()
    //    {
    //        ModelImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.activeObject)) as ModelImporter;
    //        if (importer)
    //        {
    //            ModelImporterClipAnimation[] animations = importer.clipAnimations;
    //            List<AnimationEvent> events = new List<AnimationEvent>();
    //            ModelImporterClipAnimation animation = animations[0];

    //            string[] paramGroup = eventForSingleAttack;
    //            // 单动画，2段，3段攻击的命名，要和美术同学约定好。
    //            if (animation.name.Contains("2attack") || animation.name.Contains("Attack_2"))
    //            {
    //                paramGroup = eventForDoubleAttack;
    //            }
    //            else if (animation.name.Contains("3attack") || animation.name.Contains("Attack_3"))
    //            {
    //                paramGroup = eventForTrippleAttack;
    //            }

    //            for (int eventIndex = 0; eventIndex < paramGroup.Length; eventIndex++)
    //            {
    //                AnimationEvent _event = new AnimationEvent();
    //                _event.functionName = animationEventName;
    //                _event.time = 0.1f * eventIndex;
    //                _event.floatParameter = 0;
    //                _event.intParameter = 0;
    //                _event.stringParameter = paramGroup[eventIndex];
    //                events.Add(_event);
    //            }
    //            animation.events = events.ToArray();
    //            importer.clipAnimations = animations;
    //            importer.SaveAndReimport();
    //        }
    //        AssetDatabase.Refresh();
    //    }
    //}
}
