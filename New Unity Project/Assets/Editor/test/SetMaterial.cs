using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetMaterial : MonoBehaviour
{

    /// <summary>
    /// 根据鱼类的名字去Assets/SharkRes/ToonyFish/Materials/这个路径下去找材质球给鱼类上材质。复制了3001的材质球，必须有3001。
    /// </summary>
    [MenuItem("GameObject/策划/SetMaterial",false,60)]
    static void SetMat()
    {
        string AAA = MatName(Selection.activeGameObject.name);
        Material mat = FindMat(AAA);
        SkinnedMeshRenderer[] seleObj = Selection.activeGameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        setMatFinal(seleObj, mat);
    }
    
     private static string MatName(string ObjectName)
    {
        string matName;
        char[] nameArray = ObjectName.ToCharArray();
        if (nameArray[1] == 'C')
        {
            //是小鱼
            matName = ObjectName.Replace("FC","F");
            matName = matName + "_1";
        }
        else
        {
            //大鱼
            matName = ObjectName + "_2";
        }
        return matName;
    }
    private static Material FindMat(string matName)
    {
        //Assets/SharkRes/ToonyFish/Materials/F_3001_1.mat
        //Assets/SharkRes/ToonyFish/Materials/F_3026_2.mat
        string sss = "Assets/SharkRes/ToonyFish/Materials/" + matName + ".mat";
        Material tarMat = AssetDatabase.LoadAssetAtPath<Material>(sss);
        string matName1 = matName.Remove(matName.Length - 1) + "1";
        string matName2 = matName.Remove(matName.Length - 1) + "2";
        string matName3 = matName.Remove(matName.Length - 1) + "3";
        if (tarMat == null)
        {//如果是个空，那么检查一下所有的，123，都给创建一遍
            tarMat = CreatMat(matName, sss);

            

            string sss1 = "Assets/SharkRes/ToonyFish/Materials/" + matName1 + ".mat";
            Material mat1 = AssetDatabase.LoadAssetAtPath<Material>(sss1);
            if(mat1 == null)
            {
                CreatMat(matName1, sss1);
            }
            string sss2 = "Assets/SharkRes/ToonyFish/Materials/" + matName2 + ".mat";
            Material mat2 = AssetDatabase.LoadAssetAtPath<Material>(sss2);
            if (mat2 == null)
            {
                CreatMat(matName2, sss2);
            }
            string sss3 = "Assets/SharkRes/ToonyFish/Materials/" + matName3 + ".mat";
            Material mat3 = AssetDatabase.LoadAssetAtPath<Material>(sss3);
            if (mat3 == null)
            {
                CreatMat(matName3, sss3);
            }
        }
        if(tarMat.GetTexture("_MainTex")==null)
        {
            //设置贴图
            setTexture(matName, tarMat);
            
        }

        return tarMat;
    }

    private static void setTexture(string matName, Material tarMat)
    {
        string texName = matName.Replace("F_3", "4");
        string texPath = "Assets/SharkRes/ToonyFish/Textures/" + texName + ".png";
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        if(tex == null)
        {
            Debug.Log($"找不到{texPath}");
        }
        tarMat.SetTexture("_MainTex", tex);
    }

    private static Material CreatMat(string matName, string sss)
    {
        Material tarMat;
        string path = "Assets/SharkRes/ToonyFish/Materials/F_3001_1.mat";
        AssetDatabase.CopyAsset(path, sss);
        tarMat = AssetDatabase.LoadAssetAtPath<Material>(sss);
        string texName = matName.Replace("F_3", "4");
        string texPath = "Assets/SharkRes/ToonyFish/Textures/" + texName + ".png";
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
        if(tex == null)
        {
            Debug.Log($"没找到，{texPath}");
        }
        tarMat.SetTexture("_MainTex", tex);
        return tarMat;
    }

    private static void setMatFinal(SkinnedMeshRenderer[] seleObj,Material mat)
    {
        for(int i = 0; i < seleObj.Length; i++)
        {
            seleObj[i].material = mat;
        }
    }
}

