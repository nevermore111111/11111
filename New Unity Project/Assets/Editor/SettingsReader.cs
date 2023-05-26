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
    //��Ҫ1ҳ�룬2�ֶ�����3·�������ݵ�һ��ƥ�����Ϣ���͵�3�е�����ȥ����ת��
    //����֧�ֵ����ͣ�int��float �Լ� int[]��float[]
    static AssetHelper asset;

    static public void GetSetting()
    {
        List<string[]> Model = ExcelReaderHelper.ExcelReader(1, "string[]", PathConverter.ConvertToDirectoryPath(SettingPath)) as List<string[]>;
        Debug.Log(Model[3][0]);
    }
    [MenuItem("GameObject/����")]
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

        //����

        //asset.AttackInAir_fist_int = mid.ToArray();
        //Debug.Log(mid.Count);
    }

}
