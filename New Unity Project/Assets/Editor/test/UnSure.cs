//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.Net.Sockets;
//using System.Linq;
//using NUnit.Framework.Constraints;
//using Unity.Collections;
//using UnityEngine.UIElements;
//using System;


//using PlasticGui.Configuration.CloudEdition.Welcome;

//public class UnSure : MonoBehaviour
//{
//    private static int Once;
//    private static int OncePro;
//    [MenuItem("GameObject/策划/对一个非怪异鱼使用创建眩晕挂点，再次使用自动计算位置", false, 40)]
//    private static void DoOnlyOnce()
//    {
//        if (Once == 0)
//        {
//            //
//            //
//            //
//            //
//            //下面放置要用的方法
//            Debug.Log("做了");
//            //先设置signal（检测dizzle，没有dizzle的话，直接把signal设置成body的范围
//            SetSignal();
//            DizzlyPoint();
//            //
//            //
//            //
//            //
//            Once++;
//            if(Selection.gameObjects.Length == 1)
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
//    private static void Fun2()
//    {
//        Debug.Log("Fun2");
//    }
//    public static void DizzlyPoint()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            SkillPointRev[] points = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//            bool isDizzle = false;
//            for (int j = 0; j < points.Length; j++)
//            {
//                if (points[j].skillEffectPoint == 11)
//                {
//                    isDizzle = true;
//                    //在这里设置眩晕挂点的位置
//                    Transform tar = points[j].gameObject.transform;
//                    Vector3 ppos = tar.parent.transform.position;

//                    Transform rotation = Selection.gameObjects[i].transform.GetChild(0);
//                    float scale = rotation.GetChild(0).localScale.x;
//                    float hight = rotation.GetChild(1).localScale.y;
//                    ppos = ppos + new Vector3(0, hight * 0.7f, 0);


//                    tar.position = ppos;
//                    GameObject skin = FindSkinBody(Selection.gameObjects[i]);
//                    float xRange = skin.GetComponent<SkinnedMeshRenderer>().bounds.size.x;
//                    float yRange = skin.GetComponent<SkinnedMeshRenderer>().bounds.size.y;
//                    float targetScale = (xRange + yRange) / 2;
//                    float targetScale01 = targetScale * 0.05f + 1f;
//                    tar.localScale = targetScale01 * Vector3.one;


//                    break;
//                }
//            }
//            if (isDizzle == false)
//            {
//                GameObject obj = new GameObject("Dizzle", typeof(SkillPointRev));
//                Transform head = SetAIRange.FindByName(Selection.gameObjects[i].transform, "head");
//                if (head == null)
//                {
//                    head = SetAIRange.FindByName(Selection.gameObjects[i].transform, "Head");
//                }
//                if (head != null)
//                {
//                    obj.transform.SetParent(head, false);
//                    obj.GetComponent<SkillPointRev>().skillEffectPoint = 11;
                    
//                }
//                else
//                {
//                    Debug.LogError(Selection.gameObjects[i].name+"没找到head/Head去创建眩晕挂点");
//                }
//            }
//        }
//    }

//    private static void Fun3()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            Transform tra = SetAIRange.FindByName(Selection.gameObjects[i].transform, "Signal");
//            if (tra.localScale == new Vector3(0, 0, 0))
//            {
//                tra.localScale = Vector3.one;
//            }
//        }
//    }
    
//    [MenuItem("GameObject/策划/把最后一个物体当成技能，加在技能挂点", false, 30)]
//    private static void Fun5()
//    {
//        if (Once == 0)
//        {
//            GameObject[] objs = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray<GameObject>();
//            GameObject obj = objs[objs.Length - 1];
//            for (int i = 0; i < Selection.gameObjects.Length; i++)
//            {
//                SkillPointRev[] skillPoint = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//                if (skillPoint.Length > 0)
//                {
//                    for (int j = 0; j < skillPoint.Length; j++)
//                    {
//                        if (skillPoint[j].skillEffectPoint == 0)
//                        {
//                            GameObject obj2 = Instantiate(obj);
//                            obj2.transform.SetParent(skillPoint[j].gameObject.transform,false);
//                        }
//                    }
//                }
//            }
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
//        Debug.Log(Selection.activeObject.name);
//    }

//    private static void SetSignal()
//    {
//        //只执行一次的方法，仅仅在signal的缩放全部为0的时候，把signal的缩放数值设置成body的bound的大小
//        GameObject[] objs= Selection.gameObjects;
//        for(int i = 0; i < objs.Length; i++)
//        {
//            Transform signal = SetAIRange.FindByName(objs[i].transform, "Signal");
//            if(signal.localScale == Vector3.zero)
//            {
//                signal.localScale = Selection.gameObjects[i].GetComponentInChildren<SkinnedMeshRenderer>().bounds.size;
//            }
//        }
//    }
//    [MenuItem("GameObject/策划/把最后一个物体当成眩晕buff，加在技能挂点", false, 30)]
//    private static void Fun6()
//    {
//        if (Once == 0)
//        {
//            GameObject[] objs = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray<GameObject>();
//            GameObject obj = objs[objs.Length - 1];
//            for (int i = 0; i < Selection.gameObjects.Length; i++)
//            {
//                SkillPointRev[] skillPoint = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//                if (skillPoint.Length > 0)
//                {
//                    for (int j = 0; j < skillPoint.Length; j++)
//                    {
//                        if (skillPoint[j].skillEffectPoint == 11)
//                        {
//                            GameObject obj2 = Instantiate(obj);
//                            obj2.transform.SetParent(skillPoint[j].gameObject.transform, false);
//                            break;
//                        }
                       
//                    }
//                }
//            }
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
//        Debug.Log(Selection.activeObject.name);
//    }
//    /// <summary>
//    /// 传递父物体，返回带有<SkinnedMeshRenderer>这个组件的子物体，且名字带body。
//    /// </summary>
//    /// <param name="obj"></param>
//    /// <returns></returns>
//    public static GameObject FindSkinBody(GameObject obj)
//    {
//        SkinnedMeshRenderer[] skins = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
//        for(int i = 0; i < skins.Length; i++)
//        {
//            if (skins[i].gameObject.name.Contains("body"))
//            {
//                return skins[i].gameObject;
//            }
//        }
        
//        Debug.LogError(obj.name + "没找到body,这样直接反悔了第一个SkinnedMeshRenderer");
//        {
//            return skins[0].gameObject;
//        }
//        return null;
//    }
//    [MenuItem("GameObject/策划/修改鱼类的材质",false,100)]
//    private static void Fun66()
//    {
//        if(DoOnlyOncePro())
//        {
//            string path = "Assets/SharkRes/ToonyFish/Materials/SmallFish_3001.mat";
//            GameObject[] obj = Selection.gameObjects;
//            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
//            for (int i = 0; i < obj.Length; i++)
//            {
//                if (!obj[i].name.Contains("3024"))
//                {
//                    //Assets/SharkRes/ToonyFish/Materials/SmallFish_3001.mat
//                    //切换每个鱼的材质球
//                    SkinnedMeshRenderer[] skins = obj[i].GetComponentsInChildren<SkinnedMeshRenderer>();
//                    for( int j = 0; j < skins.Length; j++)
//                    {
//                        skins[j].material = mat;
//                    }
//                }
//            }
//        }
        
//    }
//    public static bool DoOnlyOncePro()
//    {
//        //对应在hierarchy执行的方法，先执行这个方法，就能够不再执行很多遍，而是只执行一遍；
//        //return一个值，如果是TRUE 直接跳出；
//        if(OncePro == 0) 
//        {
//            OncePro++;
//            if(Selection.gameObjects.Length == 1)
//            {
//                OncePro = 0;
//            }
//            return true;
//        }
//        else
//        {
//            OncePro++;
//            if(OncePro == Selection.gameObjects.Length)
//            { OncePro = 0;}
//            return false;
//        }
//    }
//    [MenuItem("GameObject/策划/把一个鱼当成真圆鳍鱼，挂在技能挂点上面", false, 31)]
//    static void AddFish()
//    {
//        if (DoOnlyOncePro())
//        {
//            GameObject[] gameObjects = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray();
//            GameObject ZYFish =  gameObjects[gameObjects.Length - 1];
//            //获得间距
//            Vector3 vec = GetLength(ZYFish);
//            for( int i = 0; i < gameObjects.Length; i++)
//            {
//                SkillPointRev[] skills = gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//                for( int j = 0; j < skills.Length; j++)
//                {
//                    if (skills[j].skillEffectPoint == 102)
//                    {
//                        GameObject footTar = skills[j].gameObject;
//                        GameObject newFish =  Instantiate(ZYFish,footTar.transform,true);
//                        newFish.transform.localPosition = vec;
//                    }
//                }
//            }
//        }
//    }
//    static private Vector3 GetLength(GameObject obj)
//    {
//        Vector3 length;
//        SkillPointRev[] skills =  obj.GetComponentsInChildren<SkillPointRev>();
//        for(int i = 0; i < skills.Length; i++)
//        {
//            if(skills[i].skillEffectPoint == 101)
//            {
//                return length = obj.transform.position - skills[i].transform.position;
//            }
//        }
//        return Vector3.zero;
//    }
//    [MenuItem("GameObject/策划/测试（在unsure脚本里面1.0）",false,80)]
//    static void Test0001()
//    {
//        GameObject obj = Selection.gameObjects[0];
//        Undo.AddComponent<SkillPointRev>(obj);
//    }
//    [MenuItem("GameObject/策划/测试2（在unsure脚本里面1.0）", false, 80)]
//    static void Test0002()
//    {
//        Undo.PerformRedo();
//    }
//    /// <summary>
//    /// 在普通鱼身上增加一个召唤和变异的特效挂点，位置等同于signal的位置，并且缩放等于缩放的三次跟号
//    /// 增加一个skillpoint的脚本，命名为skillpoint
//    /// 
//    /// </summary>
//    [MenuItem("GameObject/策划/增加召唤挂点，在signal（在unsure脚本里面1.0）", false, 1)]
//    public static void AddSpecialSkillPoint()
//    {
//            for(int i = 0; i < Selection.gameObjects.Length; i++)
//            {
//                GameObject obj = Selection.gameObjects[i];
//                SkillPointRev[] skills = obj.GetComponentsInChildren<SkillPointRev>();
//                bool isExist = false;
//                for(int j = 0; j < skills.Length; j++)
//                {
//                    if(skills[j].skillEffectPoint == 21)
//                    {
//                        isExist = true;
//                        GameObject game = skills[j].gameObject;
//                        game.name = "ZhaoHuanSkillPoint";
//                        Transform signal = SetAIRange.FindByName(obj.transform, "Signal");
//                        float num = signal.localScale.x * signal.localScale.y * signal.localScale.z;
//                        game.transform.localScale = Mathf.Pow(num, 0.333f) * Vector3.one;
//                        game.transform.localPosition = signal.localPosition;
//                }
//                }
//                if(isExist == false)
//                {
//                    Transform signal = SetAIRange.FindByName(obj.transform, "Signal");
//                    GameObject game = new GameObject("ZhaoHuanSkillPoint", typeof(SkillPointRev));
//                    game.transform.SetParent(signal.parent.transform, false);
//                    float num = signal.localScale.x * signal.localScale.y * signal.localScale.z;
//                    game.transform.localScale = Mathf.Pow(num, 0.333f) * Vector3.one;
//                    game.transform.localPosition = signal.localPosition;
//                    game.GetComponent<SkillPointRev>().skillEffectPoint = 21;
//                }
                
//            }
//    }
//    [MenuItem("GameObject/策划/增加修改盛宴头部挂点，（在unsure脚本里面1.0）", false, 1)]
//    public static void AddEatSkillPoint()
//    {
       
//            for (int i = 0; i < Selection.gameObjects.Length; i++)
//            {
//                GameObject obj = Selection.gameObjects[i];
//                Transform Dizzle = SetAIRange.FindByName(obj.transform, "Dizzle");
//                GameObject game = null;
//                if (SetAIRange.FindByName(Selection.gameObjects[i].transform, "EatSkillPoint")!= null)
//                {
//                    game = SetAIRange.FindByName(Selection.gameObjects[i].transform, "EatSkillPoint").gameObject;
//                }
//                if (game==null)
//                {
//                    game = new GameObject("EatSkillPoint", typeof(SkillPointRev));
//                    game.GetComponent<SkillPointRev>().skillEffectPoint = 22;
//                    game.transform.SetParent(Dizzle.parent.transform, false);
//                }
                

//                GameObject body = GetBody(Selection.gameObjects[i]);
//                SkinnedMeshRenderer skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
//                float Num = MathF.Pow(skinnedMeshRenderer.bounds.size.x * skinnedMeshRenderer.bounds.size.y * skinnedMeshRenderer.bounds.size.z, 0.333f);
                


//                game.transform.localPosition = Dizzle.localPosition;
//                game.transform.localScale = MathF.Pow(0.35f *Num/1.2f *3,0.4f)/3*1.2f * Vector3.one;
//                game.transform.forward = Selection.gameObjects[i].transform.right;
//                game.transform.Translate(new Vector3 ( -Num * 0.2f, 0, 0 ), Space.Self);
//            }
        
//    }

//    private static GameObject GetBody(GameObject selection)
//    {
//        SkinnedMeshRenderer[] bodys = selection.GetComponentsInChildren<SkinnedMeshRenderer>();
//        for(int i = 0; i < bodys.Length; i++)
//        {
//            if (bodys[i].name.Contains("body"))
//            {
//                return bodys[i].gameObject;
//            }
//        }
//        return null;
//    }
//    [MenuItem("GameObject/策划/把最后一个物体当成盛宴头部特效，加在技能挂点", false, 30)]
//    private static void Fun8()
//    {
//        if (Once == 0)
//        {
//            GameObject[] objs = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray<GameObject>();
//            GameObject obj = objs[objs.Length - 1];
//            for (int i = 0; i < Selection.gameObjects.Length; i++)
//            {
//                SkillPointRev[] skillPoint = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//                if (skillPoint.Length > 0)
//                {
//                    for (int j = 0; j < skillPoint.Length; j++)
//                    {
//                        if (skillPoint[j].skillEffectPoint == 22)
//                        {
//                            GameObject obj2 = Instantiate(obj);
//                            obj2.transform.SetParent(skillPoint[j].gameObject.transform, false);
//                            break;
//                        }
//                    }
//                }
//            }
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

   


   

//    //[MenuItem("GameObject/test01", false, 30)]
//    //static void fun08()
//    //{
//    //    GameObject obj =  Selection.gameObjects[0];
//    //    Undo.AddComponent<SkillPointRev>(obj);
//    //    obj.transform.Translate(Vector3.forward);

//    //}
//    //[MenuItem("GameObject/test02", false, 30)]
//    //static void fun09()
//    //{
//    //    GameObject obj = new GameObject("obj");
//    //    obj.transform.position = Vector3.zero;
//    //}
//    //[MenuItem("GameObject/testEX", false, 30)]
//    //static void fun10()
//    //{
//    //    GameObject gameObject = Selection.gameObjects[0];
//    //    Undo.RecordObject(gameObject.transform, "Move Object");
//    //    gameObject.transform.position = new Vector3(1, 0, 0);
//    //}
//    [MenuItem("GameObject/test", false, 30)]
//    static void ChangeMat()
//    {
//        Material mat = AssetDatabase.LoadMainAssetAtPath("Assets/SharkRes/ToonyFish/Materials/SmallFish_3001.mat") as Material;
//        if(DoOnlyOncePro())
//        {
//            GameObject[] fishes = Selection.gameObjects;
//            for(int i = 0; i < fishes.Length; i++)
//            {
//                SkinnedMeshRenderer[] skinnedMeshRenderers = fishes[i].GetComponentsInChildren<SkinnedMeshRenderer>();
//                for(int j = 0; j < skinnedMeshRenderers.Length; j++)
//                {
//                    skinnedMeshRenderers[j].material =  mat;
//                }
//            }
//        }
//    }
//}
