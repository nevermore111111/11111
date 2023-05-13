using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotweenTest : MonoBehaviour
{
    //使用dotween控制位移

    private void Start()
    {
        
    }
    private void OnGUI()
    {
        if(GUILayout.Button("测试"))
        {

        }
    }
    private void DotweenTest01()
    {
        //dotween 的常用流程 
        // 1 doxxx 2setxxx 3onxxx 添加事件相应
        
    }
    enum IDoKind
    {
        to,
        punch,
        shake,
        toalpha,
        toArray,
        toAxis
    }
}
