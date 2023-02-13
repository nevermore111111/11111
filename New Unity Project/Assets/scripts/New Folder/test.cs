using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Timeline.Actions;
/// <summary>
/// 
/// <summary>
public class test : MonoBehaviour
{
    [MenuItem("GameObject/²âÊÔ/test01",false,10)]
    static void test01()
    {

    }
    int num;
    private IEnumerator OnMouseDown()
    {
        //num++;
        //if (num > 3)
        //{
        //    num = 1;
        //}
        //Debug.Log($"µÚ{num}´ÎÊä³ö");
        yield return 1;
        print(1);
        yield return 1;
        print(2);
        yield return 1;
        print(3);
    }
}
