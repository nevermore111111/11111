using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LoadData
{
    public AnimationEventData data;

    [MenuItem("GameObject/��������/�������б������")]
    public void LoadDataAll()
    {

    }
    public void LoadAnimationData()
    {
        data = Resources.Load<AnimationEventData>("AnimationEventData");
    }

}
