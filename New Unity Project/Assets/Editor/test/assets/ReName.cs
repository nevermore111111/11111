using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReName : MonoBehaviour
{
    [MenuItem("Assets/策划/贴图重命名",false,50)]
    static void ReNameTex()
    {
        string[] names = Selection.assetGUIDs;
        ReNameAll(names);
    }
    static void ReNameAll(string[] guids)
    {
        Texture2D[] texs = new Texture2D[guids.Length];
        for(int i = 0; i < guids.Length; i++)
        {
            texs[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guids[i]));
        }
        if (isFc(texs) == true) 
        { 
            for(int i = 0; i < texs.Length; i++)
            {
                if (texs[i].name.ToLower().Contains("fc"))
                {
                    Debug.Log(texs[i].name);
                }
            }
        }
    }
    /// <summary>
    /// 判定是否包含Fc
    /// </summary>
    /// <param name="texs"></param>
    /// <returns></returns>
    static bool isFc(Texture2D[] texs)
    {
        for(int i = 0; i< texs.Length; i++)
        {
            string lowName =  texs[i].name.ToLower();
            if(lowName.Contains("fc"))
            {
                return true;
            }
        }
        return false;
    }
}
