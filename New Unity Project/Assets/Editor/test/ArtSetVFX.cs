//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.Linq;

//public class ArtSetVFX : MonoBehaviour
//{
//    [MenuItem("GameObject/把选择的最后一个物体当成新特效增加在特效挂点", false, 10)]
//    public static void AddSkillPointToSelectedObjects()
//    {
//        GameObject[] selectedObjects = Selection.gameObjects.OrderBy(_ =>_.transform.GetSiblingIndex()).ToArray();

//        if (selectedObjects.Length == 0)
//        {
//            Debug.LogWarning("No objects selected.");
//            return;
//        }

//        // Get the last object in the selection to add as a child
//        GameObject childObject = selectedObjects[selectedObjects.Length - 1];

//        // Keep track of any changes we make for undo purposes
//        Undo.RegisterCompleteObjectUndo(selectedObjects, "Add Skill Point");

//       for(int i = 0; i < selectedObjects.Length-1; i++)
//        {
//            // Make sure the parent object has a "skillpoint" child
//            SkillPointRev[] skillPoints = selectedObjects[i].GetComponentsInChildren<SkillPointRev>().OrderBy(_ =>_.skillEffectPoint).ToArray();

//            bool isSet = false;
//            foreach (SkillPointRev skillPointRev in skillPoints)
//            {
//                if (skillPointRev != null && skillPointRev.transform != null && skillPointRev.transform.childCount == 0&&skillPointRev.skillEffectPoint<6 && isSet == false)
//                {
//                    GameObject newChild = GameObject.Instantiate(childObject) as GameObject;
//                    newChild.transform.parent = skillPointRev.transform;
//                    newChild.transform.localPosition = Vector3.zero;
//                    newChild.transform.localRotation = Quaternion.identity;
//                    newChild.transform.localScale = Vector3.one;
//                    isSet = true;
//                    break;
//                }
//            }
//        }
//    }
//    [MenuItem("GameObject/美术/特效/3", false, 10)]
//    static void Fun()
//    {

//    }
//}
