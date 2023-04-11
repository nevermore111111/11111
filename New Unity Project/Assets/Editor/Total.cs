using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Total : MonoBehaviour
{
    [MenuItem("Assets/≤‚ ‘",false,10)]
    static void Fun01()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath((Selection.assetGUIDs[0])));
    }
}
