using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NameAdd15 : MonoBehaviour
{
    [MenuItem("Assets/策划/NameAdd15",false,10)]
    static void NameAdd()
    {
        string[] tar = Selection.assetGUIDs;
        string[] tar2 = new string[tar.Length]; 
        for(int i = 0; i < tar.Length; i++)
        {
            tar2[i] = AssetDatabase.GUIDToAssetPath(tar[i]);
        }
        for(int i = 0; i < tar2.Length; i++)
        {
            AssetDatabase.RenameAsset(tar2[i], NameChange(AssetDatabase.LoadMainAssetAtPath(tar2[i]).name));
        }
    }
    static string NameChange(string NameStart)
    {
        string nameNum =  (int.Parse(NameStart)+15).ToString();
        return nameNum;
        
    }
}
