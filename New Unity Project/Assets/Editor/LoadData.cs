using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoadData
{
    public AnimationEventData data;

    [MenuItem("GameObject/载入数据/载入所有表格数据")]
    public void LoadDataAll()
    {

    }
    public void LoadAnimationData()
    {
        data = Resources.Load<AnimationEventData>("AnimationEventData");
    }

}
