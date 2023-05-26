using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class SettingsReader : Editor
{
    //Assets/Resources/settings.xlsx
    static string SettingPath = "Assets/Resources/settings.xlsx";
    static string Path02 = "Assets/Resources/Shark.xlsx";
    //需要1页码，2字段名，3路径。根据第一行匹配的信息，和第3行的类型去进行转化
    //现在支持的类型，int和float 以及 int[]和float[]
    static AssetHelper asset;

    static public void GetSetting()
    {
        List<string[]> Model = ExcelReaderHelper.ExcelReader(1, "string[]", PathConverter.ConvertToDirectoryPath(SettingPath)) as List<string[]>;
        Debug.Log(Model[3][0]);
    }
    [MenuItem("GameObject/测试")]
    static void Print()
    {
        GetSetting();
        //asset = AssetDatabase.LoadAssetAtPath<AssetHelper>("Assets/Resources/AssetHelper.asset");
        //Debug.Log(asset);
        //PrintAsset();
    }
    static void PrintAsset()
    {
        asset = AssetDatabase.LoadAssetAtPath<AssetHelper>("Assets/Resources/AssetHelper.asset");
        List<int> mid = ExcelReaderHelper.ExcelReader(1, "AttackInAir_fist_int", PathConverter.ConvertToDirectoryPath(SettingPath)) as List<int>;

        //这样

        //asset.AttackInAir_fist_int = mid.ToArray();
        //Debug.Log(mid.Count);
    }

}
