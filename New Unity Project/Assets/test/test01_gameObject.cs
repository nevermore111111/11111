using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;
using Unity.VisualScripting;

/// <summary>
/// 
/// <summary>
public class test01_gameObject : MonoBehaviour
{
    [MenuItem("GameObject/≤‚ ‘/¥Ú”°bones",false,11)]
    private static void test02()
    {
        Renderer renderer = Selection.gameObjects[0].GetComponent<Renderer>();
        renderer.material.color = Color.green;

        
    }
}
