//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.CodeDom;

//using System.Linq;

//public class IsFinish : MonoBehaviour
//{
//    private static int Once;
//    /// <summary>
//    /// 检查
//    /// </summary>
//    [MenuItem("GameObject/策划/检查一下(还没做，不会)",false,200)]
//    private static void DoOnlyOnce()
//    {
//        if (Once == 0)
//        {
//            //
//            Debug.Log("开始检查:rvo和skill现在不再检查了");
//            isFinish();
//            //

//            Once++;
//            if (Selection.gameObjects.Length == 1)
//            {
//                Once = 0;
//            }
//            return;
//        }
//        else
//        {
//            Once++;
//            if (Once == Selection.gameObjects.Length)
//            {
//                Once = 0;
//            }
//        }
//    }
//    /// <summary>
//    /// 这个是主要方法
//    /// </summary>
//    private static void isFinish()
//    {
//        //需要检查的地方1 rvo 2碰撞盒 3技能挂点 4眩晕挂点 5攻击区域 6bitepoints 7贴图 8动画 9鱼类中心点位置
//        //我想传递一个gameobject和一个泛型。
//        //以后再说
//        GameObject[] obj = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray<GameObject>();
//        for(int i = 0; i < obj.Length; i++)
//        {
//            //增加要查询的列表
//            ArrayList list = new ArrayList();
//            AddList(obj, i, list);
//            IsMove(obj[i], list);
//            IsCenterMove(obj, i);
//        }
//    }

//    private static void IsCenterMove(GameObject[] obj, int i)
//    {
//        Transform bodyTransform = obj[i].transform.GetChild(0).GetChild(0).transform;
//        if ((obj[i].transform.position - bodyTransform.position).sqrMagnitude < 0.005)
//        {
//            Debug.Log($"{obj[i].name}的中心点没调整");
//        }
//    }

//    private static void AddList(GameObject[] obj, int i, ArrayList list)
//    {
//        // 1 rvo 2碰撞盒 3技能挂点 4眩晕挂点 5攻击区域 6bitepoints 7贴图 8动画
//        //list.Add(obj[i].GetComponentInChildren<RvoColiderReceivers>().gameObject);
//        AttackReceiver[] atks = obj[i].GetComponentsInChildren<AttackReceiver>();
//        for(int j = 0; j < atks.Length; j++)
//        {
//            list.Add(atks[j].gameObject);
//        }
//        SkillPointRev[] skills = obj[i].GetComponentsInChildren<SkillPointRev>();
//        SkillPointRev skillDizzled = null;
//        for (int j = 0; j < skills.Length; j++)
//        {
//            //增加乘骑技能挂点（真圆鳍鱼）
//            if(skills[j].skillEffectPoint == 102)
//            {
//                list.Add(skills[j].gameObject);
//            }
//            if (skills[j].skillEffectPoint == 11)
//            {
//                list.Add(skills[j].gameObject);
//                skillDizzled = skills[j];
//            }
//        }
//        if (skillDizzled == null)
//        {
//            Debug.Log($"{obj[i]}没有没有眩晕挂点");
//        }
//        //for(int k = 0; k < skills.Length; k++)
//        //{
//        //    list.Add(skills[k].gameObject);
//        //}
//        list.Add(obj[i].GetComponentInChildren<AttackModel>().gameObject);
//        AttackbiteRev[] bites = obj[i].GetComponentsInChildren<AttackbiteRev>();
//        for( int k = 0; k < bites.Length; k++)
//        {
//            list.Add(bites[k].gameObject);
//        }
         
        
//        //目前只检测到6，bitepoints,后续以后再写
//    }

//    private static void IsMove(GameObject obj,ArrayList list)
//    {
        
//        for(int i = 0;i<list.Count; i++)
//        {
//            GameObject tar = (GameObject)list[i];
//            if(tar.transform.localPosition == Vector3.zero)
//            {
//                if(tar.transform.localScale == Vector3.one)
//                {
//                    if (tar.transform.localEulerAngles == Vector3.zero)
//                    {
//                        Debug.Log($"{obj.name}中的{tar.name}未修改");
//                    }
//                }
//            }
//        }
//    }
//}
