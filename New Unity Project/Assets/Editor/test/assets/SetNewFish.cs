using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.Mathematics;

public class SetNewFish : MonoBehaviour
{
    //要做：1选择一个fbx，生成在面板中2寻找到skin给材质球3给材质球设置贴图
    [MenuItem("Assets/美术/NewFish")]
     static private  void NewFish()
    {
        GameObject obj = Instantiate(Selection.objects[0]) as GameObject;
        string pathTar = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        Material mat2 = CreatMat(Selection.objects[0] as GameObject, pathTar);
        SetMat2(obj, mat2);

    }

    private static void SetMat2(GameObject obj, Material mat2)
    {
        MeshRenderer[] skin = obj.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < skin.Length; i++)
        {
            skin[i].material = mat2;
        }
    }

    static private Material CreatMat(GameObject obj,string pathTar)
    {

        string path = "Assets/SharkRes/ToonyFish/Materials/F_3001_1.mat";
        string path2 = pathTar.Substring(0,pathTar.IndexOf(obj.name+"."));
        AssetDatabase.CopyAsset(path, path2+obj.name+".mat");
        Material mat2 = AssetDatabase.LoadAssetAtPath<Material>(path2 + obj.name + ".mat");
        Texture texTar = AssetDatabase.LoadAssetAtPath<Texture>(path2 + obj.name + ".psd");
        mat2.SetTexture("_MainTex", texTar);
        return mat2;
    }
}
