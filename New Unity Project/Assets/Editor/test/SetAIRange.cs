//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditor.SceneManagement;

//using System.Drawing.Text;

//using System.Linq;
//using System.Runtime.CompilerServices;


//public class SetAIRange : MonoBehaviour
//{
//    private static int doNum = 0;
//    [MenuItem("GameObject/策划/SetAIRange", false, 1)]
//    //选择鱼类之后，根据鱼类的缩放比例，修改AIFindRange为固定值（长和高）
//    static void AIFindRange()
//    {

//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            Transform tra = FindByName(Selection.gameObjects[i].transform, "AIFindRange");
//            float scale = tra.parent.parent.parent.transform.localScale.x;
//            CapsuleCollider cap = tra.GetComponent<CapsuleCollider>();
//            cap.radius = 1.6f / scale;
//            cap.height = 6.3f / scale;
//            Vector3 bodyVec = Selection.gameObjects[i].transform.forward;
//            Vector3 suddenTriggerVec = tra.forward;
//            float angel = Vector3.Angle(bodyVec, suddenTriggerVec);
//            if (angel > 60)
//            {
//                cap.direction = 0;
//            }
//        }
//    }
//    static void AddSkinManager()
//    {
//        GameObject[] objs = Selection.gameObjects.OrderBy(_ => _.transform.GetSiblingIndex()).ToArray();
//        for (int i = 0; i < objs.Length; i++)
//        {
//            if (objs[i].GetComponent<SkinManager>() == null)
//            {
//                objs[i].AddComponent<SkinManager>();
//            }
//        }
//    }
//    [MenuItem("GameObject/策划/SetAttackTrigger", false, 1)]
//    static void SetAttackTrigger()
//    {
//        GameObject obj = new GameObject("AttackTrigger", typeof(AttackModel), typeof(SphereCollider));
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            AttackModel target = Selection.gameObjects[i].GetComponentInChildren<AttackModel>();

//            if (target == null)
//            {
//                Transform head = FindByName(Selection.gameObjects[i].transform, "head");
//                obj.GetComponent<SphereCollider>().isTrigger = true;
//                Instantiate(obj, head);
//            }
//            else
//            {
//            }

//        }
//        DestroyImmediate(obj);
//    }


//    /// <summary>
//    /// 查找父物体下的子物体
//    /// </summary>
//    /// <param name="parent"></param>
//    /// <param name="objName"></param>
//    /// <returns></returns>
//    public static Transform FindByName(Transform parent, string objName)
//    {
//        if (parent.Find(objName) != null)
//        {
//            return parent.Find(objName);
//        }
//        else
//        {
//            for (int i = 0; i < parent.childCount; i++)
//            {
//                Transform target = FindByName(parent.GetChild(i), objName);
//                if (target != null)
//                {
//                    return target;
//                }
//            }
//        }

//        return null;
//    }
//    /// <summary>
//    /// 寻找鱼类身上的攻击挂点，如果没有就在head下面增加一个
//    /// </summary>
//    [MenuItem("GameObject/策划/SetBitePoint", false, 1)]
//    static void SetBitePoint()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            AttackbiteRev rev = Selection.gameObjects[i].GetComponentInChildren<AttackbiteRev>();
//            if (rev == null)
//            {
//                GameObject obj = new GameObject("BitePoint", typeof(AttackbiteRev));
//                Transform head = FindByName(Selection.gameObjects[i].transform, "head");
//                obj.transform.SetParent(head, false);
//            }
//        }
//    }
//    /// <summary>
//    /// 寻找鱼类的冲刺范围，根据鱼类的体长自动计算。
//    /// </summary>
//    [MenuItem("GameObject/策划/SetSuddenTrigger", false, 1)]
//    static void SetSuddenTrigger()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            float scale = Selection.gameObjects[i].transform.GetChild(0).GetChild(0).transform.localScale.x;
//            float fishLength = Selection.gameObjects[i].GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size.z;

//            float fishFinalLength = fishLength * scale;//获得鱼类的缩放后的模型长度
//            float fishSuddenTriggerRadius = (fishFinalLength * 0.05f + 0.55f) / scale;
//            Transform tra = FindByName(Selection.gameObjects[i].transform, "SuddenTrigger");

//            if (tra.GetComponent<CapsuleCollider>() != null)
//            {
//                tra.GetComponent<CapsuleCollider>().radius = fishSuddenTriggerRadius;
//                CapsuleCollider cap = tra.GetComponent<CapsuleCollider>();
//                if (cap.center.x != 0)
//                {
//                    cap.center = Vector3.right * fishSuddenTriggerRadius;
//                }
//                if (cap.center.y != 0)
//                {
//                    cap.center = Vector3.up * fishSuddenTriggerRadius;
//                }
//                if (cap.center.z != 0)
//                {
//                    cap.center = Vector3.forward * fishSuddenTriggerRadius;
//                }
//                if (cap.center == Vector3.zero)
//                {
//                    cap.center = Vector3.right * fishSuddenTriggerRadius;
//                }
//                cap.height = 0;
//                tra.gameObject.AddComponent(typeof(SphereCollider));
//                SphereCollider sphere = tra.GetComponent<SphereCollider>();
//                sphere.radius = cap.radius;
//                sphere.center = cap.center;
//                sphere.isTrigger = true;
//                DestroyImmediate(cap);
//            }
//            if (tra.GetComponent<SphereCollider>() != null)
//            {
//                tra.GetComponent<SphereCollider>().radius = fishSuddenTriggerRadius;
//                SphereCollider cap = tra.GetComponent<SphereCollider>();
//                if (cap.center.x != 0)
//                {
//                    cap.center = Vector3.right * fishSuddenTriggerRadius;
//                }
//                if (cap.center.y != 0)
//                {
//                    cap.center = Vector3.up * fishSuddenTriggerRadius;
//                }
//                if (cap.center.z != 0)
//                {
//                    cap.center = Vector3.forward * fishSuddenTriggerRadius;
//                }
//                if (cap.center == Vector3.zero)
//                {
//                    cap.center = Vector3.right * fishSuddenTriggerRadius;
//                }
//            }
//        }
//    }


//    [MenuItem("GameObject/策划/SimplifyNum", false, 1)]
//    static void SimplifyNum()
//    {

//    }
//    [MenuItem("GameObject/策划/setNewMat", false, 1)]
//    static void setNewMat()
//    {
//        string path = "Assets/SharkRes/ToonyFish/Materials/SmallFish_3001.mat";
//        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
//        GameObject[] objs = Selection.gameObjects;
//        for (int i = 0; i < objs.Length; i++)
//        {
//            GameObject fish = Selection.gameObjects[i];
//            SkinnedMeshRenderer[] meshers = fish.GetComponentsInChildren<SkinnedMeshRenderer>();
//            for (int j = 0; j < meshers.Length; j++)
//            {
//                meshers[j].material = mat;
//            }
//        }
//    }
//    [MenuItem("GameObject/策划/增加眩晕点", false, 1)]
//    static void AddDizzle()
//    {
//        UnSure.AddSpecialSkillPoint();
//        UnSure.AddEatSkillPoint();
//    }

//    [MenuItem("GameObject/策划/DoAboveAll", false, 10)]
//    static void DoAll()
//    {
//        if (UnSure.DoOnlyOncePro())
//        {
//            SetSignal();//需要新加一个方法，判断一个鱼的躯干开始执行时设置signal
//            AddSkinManager();//增加一个新方法，增加skinmanager
//            AIFindRange();
//            SetAttackTrigger();//attackTrigger
//            SetSuddenTrigger();
//            AttackRec();
//            SkillPoint();
//            SetBitePoint();
//            AutoSetBitePoint();
//            setNewMat();
//            ResetPosOfFish();// 重置鱼整体的位置，worldPos = attack的中心点位置
//            //ResetSignal();//重置signal的位置到body
//            WaterTrigger();
//            SetTail(); //新加一个方法，把所有的鱼的tailanimator2都关闭。
//            SetMatJudge();
//            UnSure.DizzlyPoint();
//            UnSure.DizzlyPoint();
//            UnSure.AddSpecialSkillPoint();
//            UnSure.AddEatSkillPoint();
            
//            //AddRiderPoint();
//        }
//    }
//    static void SetTail()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            TailAnimator2 tail = Selection.gameObjects[i].GetComponentInChildren<TailAnimator2>();
//            if(tail != null)
//            {
//                tail.enabled = false;
//            }
//        }
//    }

//    /// <summary>
//    /// 这个方法不好，最好拿到tooth 的mesh的size去算
//    /// </summary>
//    private static void AutoSetBitePoint()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            GameObject obj = Selection.gameObjects[i];
//            AttackbiteRev[] revs = obj.GetComponentsInChildren<AttackbiteRev>();
//            bool isChangeAttack = false;
//            GameObject attack = obj.GetComponentInChildren<AttackModel>().gameObject;
//            if(attack.transform.localScale == Vector3.one && attack.transform.localRotation ==Quaternion.identity&&attack.transform.localScale ==Vector3.one)//这样没动过的attack
//            {

//            }
//            else
//            {
//                isChangeAttack = true;
//            }
//            if(revs.Length == 1 && isChangeAttack)
//            {
//                //调整过attack 并且只有一个 bite 代表可以自动生成
//                GameObject bite = revs[0].gameObject;
//                //现在有了第一个 bite 并且需要去调整
//                bite.transform.position = attack.transform.position + new Vector3(0, 0, attack.GetComponent<SphereCollider>().radius * attack.transform.lossyScale.z * 0.8f);
//                //有了第一个，这样需要去复制多个
//                // 定义旋转角度
//                for(int j = 0; j < 2; j++)
//                {
//                    float rotationAngle = 40f;
//                    if(j == 1)
//                    {
//                        rotationAngle = 140f;
//                    }
//                    // 创建旋转四元数
//                    Vector3 tarPos = attack.transform.InverseTransformPoint(bite.transform.position);

//                    Quaternion rotation = Quaternion.AngleAxis(rotationAngle, -Vector3.up);
//                    // 将旋转四元数应用到点的位置上
//                    Vector3 rotatedPosition = rotation * tarPos;
//                    GameObject newBite = GameObject.Instantiate(bite, bite.transform.parent);
//                    newBite.transform.position = attack.transform.lossyScale.x * rotatedPosition + attack.transform.position;
//                }
                
                



















//            }




//        }
//    }
//    /// <summary>
//    /// 重置signal的位置到body
//    /// </summary>
//    static void ResetSignal()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            Transform signal = FindByName(Selection.gameObjects[i].transform, "Signal");
//            GameObject fish = Selection.gameObjects[i].transform.GetChild(0).GetChild(0).gameObject;
//            signal.position = fish.transform.position;
//        }
//    }
//    /// <summary>
//    /// 重置鱼整体的位置，worldPos = attack的中心点位置
//    /// </summary>
//    static void ResetPosOfFish()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            AttackModel attacker = Selection.gameObjects[i].GetComponentInChildren<AttackModel>();
//            if(attacker.transform.localScale != Vector3.one || attacker.transform.localPosition != Vector3.zero )
//            {
//                //这样代表调整过
//                //我需要计算当前的acttack，和鱼类中心点的差距，然后如果大鱼0.1，那么去移动第三级 ，移动的方向是从attack到 pos
//                Vector3 attactPos = attacker.transform.position;
//                Vector3 fish = Selection.gameObjects[i].transform.position;
//                Vector3 Tar = fish - attactPos;
//                GameObject Tfish = Selection.gameObjects[i].transform.GetChild(0).GetChild(0).gameObject;
//                Tfish.transform.position += Tar;
//            }
//        }
//    }
//    /// <summary>
//    /// 根据body的mesh的大小，先预制一个signal
//    /// </summary>
//    static void SetSignal()
//    {
//        GameObject[] objs = Selection.gameObjects;
//        for (int i = 0; i < objs.Length; i++)
//        {
//            SkinnedMeshRenderer[] skins = objs[i].GetComponentsInChildren<SkinnedMeshRenderer>();
//            SkinnedMeshRenderer tarskin = new SkinnedMeshRenderer();
//            Vector3 size = new Vector3();
//            for (int j = 0; j < skins.Length; j++)
//            {
//                if (skins[j].name.Contains("body"))
//                {
//                    tarskin = skins[j].GetComponent<SkinnedMeshRenderer>();
//                }
//            }
//            if (tarskin == null)
//            {
//                tarskin = objs[i].GetComponentInChildren<SkinnedMeshRenderer>();
//            }
//            size = tarskin.bounds.size;
//            GameObject signal = SetAIRange.FindByName(objs[i].transform, "Signal").gameObject;
//            if (signal.transform.localScale == Vector3.zero)
//            {
//                signal.transform.localScale = size * 0.7f;
//            }
//        }
//    }

//    /// <summary>
//    /// 寻找鱼类的碰撞盒，如果没找到，就在鱼类的body下增加一个球形碰撞盒
//    /// </summary>
//    static void AttackRec()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            AttackReceiver First = Selection.gameObjects[i].GetComponentInChildren<AttackReceiver>();
//            if (First == null)
//            {
//                GameObject gam = new GameObject("1", typeof(AttackReceiver), typeof(SphereCollider));
//                gam.transform.SetParent(Selection.gameObjects[i].transform.GetChild(0).GetChild(0).GetChild(0), false);
//            }
//        }
//    }
//    /// <summary>
//    /// 寻找鱼类的技能特效挂点，如果没找到，就在鱼类的body下增加一个技能特效挂点
//    /// </summary>
//    /// 
//    [MenuItem("GameObject/策划/SkillPoint", false, 9)]
//    static void SkillPoint()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            SkillPointRev[] First = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//            bool isSkillPoint = false;
//            for (int j = 0; j < First.Length; j++)
//            {
//                if (First[j].skillEffectPoint == 0)
//                {
//                    isSkillPoint = true;
//                    break;
//                }
//            }
//            if (isSkillPoint == false)
//            {
//                Debug.Log(Selection.gameObjects[i].name);
//                GameObject gam = new GameObject("SkillPoint", typeof(SkillPointRev));
//                gam.transform.SetParent(Selection.gameObjects[i].transform.GetChild(0).GetChild(0).GetChild(0), false);
//            }
//        }
//    }
//    [MenuItem("GameObject/策划/RiderPoint", false, 1)]
//    static void AddRiderPoint()
//    {
//        for (int i = 0; i < Selection.gameObjects.Length; i++)
//        {
//            SkillPointRev[] skills = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//            //如果没有skillpoint，或者skill不是真圆鳍鱼的乘骑点，那么就增加一个skillpoint = 102
//            if (skills.Length == 0 || (skills.Length != 0 && isRidder(skills) == false))
//            {
//                GameObject gam = new GameObject("skillPointRider", typeof(SkillPointRev));
//                Transform body = FindByName(Selection.gameObjects[i].transform, "body");
//                gam.GetComponent<SkillPointRev>().skillEffectPoint = 102;
//                gam.transform.SetParent(body, true);
//                gam.transform.position = body.position;
//                gam.transform.localScale = Vector3.one;
//            }
//        }
//    }
//    static bool isRidder(SkillPointRev[] skills)
//    {
//        for (int k = 0; k < skills.Length; k++)
//        {
//            if (skills[k].skillEffectPoint == 102)
//                return true;
//        }
//        return false;
//    }
//    static void ChangeLoopEx()
//    {
//        GameObject[] games = Selection.gameObjects;
//        for (int i = 0; i < games.Length; i++)
//        {
//            FbxChange.ChangeFbxLoop(games[i].name);
//        }
//    }
//    [MenuItem("GameObject/策划/删除所有的ROV", false, 80)]
//    static void DeRvo()
//    {
//        if (UnSure.DoOnlyOncePro())
//        {
//            GameObject[] objs = Selection.gameObjects;
//            for (int i = 0; i < objs.Length; i++)
//            {
//                GameObject tar = FindByName(Selection.gameObjects[i].transform, "RvoTrigger").gameObject;
//                //DestroyImmediate( tar.GetComponent<RvoColiderReceivers>());
//            }
//        }
//    }
//    /// <summary>
//    /// 增加出水判定点，鱼类的开始和结束点
//    /// </summary>
//    [MenuItem("GameObject/策划/WaterTrigger", false, 1)]
//    static void WaterTrigger()
//    {
//        //增加head上增加一个球，命名WaterTrigger，修改tag为WaterTrigger，方向是鱼类的正方向，位置就是head的位置向鱼的正方向移动一点。
//        //然后增加两个点，StartPoint和EndPoint。


//        GameObject[] objs = Selection.gameObjects;
//        for (int i = 0; i < objs.Length; i++)
//        {

//            float length, width;
//            FindBody(i, out length, out width);
//            Direction(i, length);
//            if (SetAIRange.FindByName(Selection.gameObjects[i].transform, "WaterTrigger") == null)
//            {
                
//                AddWaterTriger(i, width, length);
//            }
//            else
//            {
//                GameObject gameObject = SetAIRange.FindByName(Selection.gameObjects[i].transform, "WaterTrigger").gameObject;
//                gameObject.tag = "WaterTrigger";

//                Transform Head = SetAIRange.FindByName(Selection.gameObjects[i].transform, "head");
//                gameObject.transform.SetParent(Head, false);

//                SphereCollider sphere = new SphereCollider();
               
//                if (sphere != null)
//                {
//                    sphere = gameObject.AddComponent<SphereCollider>();
//                }
//                SphereCollider[] spheres = gameObject.GetComponents<SphereCollider>();
//                if (spheres.Length > 1)
//                {
//                    for (int j = 1; j < spheres.Length; j++)
//                    {
//                        DestroyImmediate(spheres[j]);
//                    }
//                }
//                sphere = gameObject.GetComponent<SphereCollider>();
//                sphere.isTrigger = true;
//                sphere.radius = width * 0.3f;
//                gameObject.transform.position = FindByName(Selection.gameObjects[i].transform, "StartPoint").position;
//            }
//            //修改鱼的startpoint 的位置到鱼类的最前方
//            GameObject startPoint = FindByName(Selection.gameObjects[i].transform, "StartPoint").gameObject;
//            float lengthStandard = Selection.gameObjects[i].transform.GetChild(0).Find("Signal").localScale.z;
//            startPoint.transform.position = FindByName(Selection.gameObjects[i].transform, "Signal").position + 0.45f * lengthStandard * Vector3.forward;
//            Debug.Log(lengthStandard);
//            GameObject waterTrigger = FindByName(objs[i].transform, "WaterTrigger").gameObject;
//            GameObject signal = FindByName(objs[i].transform, "Signal").gameObject;
//            waterTrigger.transform.position = signal.transform.position + new Vector3(0, 0, signal.transform.lossyScale.z * 0.5f - waterTrigger.transform.lossyScale.x * waterTrigger.GetComponent<SphereCollider>().radius);

//        }


//    }


//    private static void FindBody(int i, out float length, out float width)
//    {
//        SkinnedMeshRenderer body = null;
//        SkinnedMeshRenderer[] skinnedMeshRenderers = Selection.gameObjects[i].GetComponentsInChildren<SkinnedMeshRenderer>();
//        for (int j = 0; j < skinnedMeshRenderers.Length; j++)
//        {
//            if (skinnedMeshRenderers[j].gameObject.name.Contains("body"))
//            {
//                body = skinnedMeshRenderers[j];
//            }
//            else if(body == null )
//            {
//                body = skinnedMeshRenderers[0];
//            }
//        }
//        length = body.bounds.size.z;
//        width = Mathf.Pow(body.bounds.size.x * body.bounds.size.y, 0.2f);
//    }

//    private static void Direction(int i, float length)
//    {
//        GameObject StartPoint = null;
//        GameObject EndPoint = null;
//        Transform tarForm = FindByName(Selection.gameObjects[i].transform, "StartPoint");
//        if (tarForm != null)
//        {
//            StartPoint = FindByName(Selection.gameObjects[i].transform, "StartPoint").gameObject;
//        }
//        tarForm = FindByName(Selection.gameObjects[i].transform, "EndPoint");
//        if (tarForm != null) 
//        {
//            EndPoint = FindByName(Selection.gameObjects[i].transform, "EndPoint").gameObject;
//        }
//        if (StartPoint == null)
//        {
//            StartPoint = new GameObject("StartPoint");
//        }
//        if(EndPoint == null)
//        {
//            EndPoint = new GameObject("EndPoint");
//        }
//        Transform rotation = SetAIRange.FindByName(Selection.gameObjects[i].transform, "Rotation");
//        StartPoint.transform.SetParent(rotation, false);
//        EndPoint.transform.SetParent(rotation, false);
//        //Vector3 vector3 = HelperTools.ChangeVector(Vector3.one, Selection.gameObjects[i].transform,rotation);//这个向量是选择的鱼的正方向的向量
//        StartPoint.transform.position = rotation.Find("Signal").transform.position + 0.3f * length * Vector3.forward;
//        EndPoint.transform.position = rotation.Find("Signal").transform.position - 0.3f * length * Vector3.forward;
//    }

//    private static void AddWaterTriger(int i, float width, float length)
//    {
//        GameObject gameObject = new GameObject("WaterTrigger");
//        gameObject.tag = "WaterTrigger";

//        Transform Head = SetAIRange.FindByName(Selection.gameObjects[i].transform, "head");
//        gameObject.transform.SetParent(Head, false);
//        SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
//        sphere.isTrigger = true;
//        sphere.radius = width * 0.25f;
//        gameObject.transform.position = FindByName(Selection.gameObjects[i].transform, "StartPoint").position;
//    }
//    /// <summary>
//    /// 根据鱼类的signal，设置两个点位，一个代表背部，一个代表尾巴，当背部和尾巴完全进入海面之前，鱼类会一直受到向下的加速度
//    /// </summary>
//    [MenuItem("GameObject/策划/身体点", false, 1)]
//    static void SetMatJudge()
//    {
//        SkinnedMeshRenderer skinnedMeshRenderer;
//        GameObject head, back, tail;
//        //先寻找着三个点，如果没有，就去加

//            GameObject[] fish = Selection.gameObjects;
//            for (int i = 0; i < fish.Length; i++)
//            {
//                if (Exist(i) == false)
//                {
//                    head = NewSkillPoint(31);
//                    back = NewSkillPoint(32);
//                    tail = NewSkillPoint(33);
//                }
//                else
//                {
//                    head = FindSkillPoint(31, Selection.gameObjects[i]);
//                    back = FindSkillPoint(32, Selection.gameObjects[i]);
//                    tail = FindSkillPoint(32, Selection.gameObjects[i]);
//                }
//                //开始设置鱼类这三个点的坐标值。
//                SkinnedMeshRenderer[] skinnedMeshRenderers = Selection.gameObjects[i].GetComponentsInChildren<SkinnedMeshRenderer>();
//                for (int j = 0; j < skinnedMeshRenderers.Length; j++)
//                {
//                    if (skinnedMeshRenderers[j].gameObject.name.Contains("body"))
//                    {
//                        skinnedMeshRenderer = skinnedMeshRenderers[j];
//                        break;
//                    }
//                }
//                //根据鱼类的signal 设置鱼类的判定范围。
//                Transform signal = FindByName(Selection.gameObjects[i].transform, "Signal");
//                head.transform.position = signal.position + signal.lossyScale.z * 0.5f * Vector3.forward;
//                back.transform.position = signal.position - signal.lossyScale.z * 0.5f * Vector3.forward;
//                tail.transform.position = signal.position - signal.lossyScale.y * 0.5f * Vector3.down;
//                head.transform.SetParent(signal.transform, true);
//                back.transform.SetParent(signal.transform, true);
//                tail.transform.SetParent(signal.transform, true);
//            }
        
//        //
//    }

//    private static bool Exist(int i)
//    {
//        SkillPointRev[] skillPointRevs = Selection.gameObjects[i].GetComponentsInChildren<SkillPointRev>();
//        for (int j = 0; j < skillPointRevs.Length; j++)
//        {
//            if (Mathf.Abs(skillPointRevs[j].skillEffectPoint - 32) <= 1)//存在这三个点其中之一
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    /// <summary>
//    /// 生成一个编号的skillpoint
//    /// </summary>
//    /// <param name="effectPointNum"></param>
//    /// <returns></returns>
//    static GameObject NewSkillPoint(int effectPointNum)
//    {
//        GameObject skillPoint = new GameObject("SkillPoint", typeof(SkillPointRev));
//        SkillPointRev skill = skillPoint.GetComponent<SkillPointRev>();
//        skill.skillEffectPoint = effectPointNum;
//        return skillPoint;
//    }
//    static GameObject FindSkillPoint(int effectPointNum, GameObject fish)
//    {
//        SkillPointRev[] skillPointRevs = fish.GetComponentsInChildren<SkillPointRev>();
//        for (int j = 0; j < skillPointRevs.Length; j++)
//        {
//            if (skillPointRevs[j].skillEffectPoint == effectPointNum)
//            {
//                return skillPointRevs[j].gameObject;
//            }
//        }
//        return null;
//    }

//    static List<GameObject> CheckSth(string Name)
//    {
//        List<GameObject> list = new List<GameObject>();
//        if (UnSure.DoOnlyOncePro())
//        {
//            GameObject[] objs = Selection.gameObjects;
//            for (int i = 0; i < objs.Length; i++)
//            {
//                Transform water = Selection.gameObjects[i].transform;
//                Transform water2 = FindByName(water, Name);
//                SphereCollider sphereCollider = water2.GetComponentInChildren<SphereCollider>();
//            }
//        }
//        return null;
//    }
//}

