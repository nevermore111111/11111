using UnityEngine;
using UnityEditor;
using System.IO;

public static class PathConverter
{

    [MenuItem("Assets/打印Path(Unity)")]
    public static string GetPath()
    {
        Debug.Log(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        string str = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        return str;
    }
    [MenuItem("Assets/打印Path(directory)")]
    public static string GetDirectoryPath()
    {
        string str = PathConverter.ConvertToDirectoryPath(AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]));
        Debug.Log(str);
        return str;
    }
    public static string ConvertToDirectoryPath(string assetPath)
    {
        string absolutePath = Application.dataPath + Path.Combine(Application.dataPath, assetPath.Substring("Assets".Length));

        ////if (!AssetDatabase.IsValidFolder(assetPath))
        //{
        //    Debug.LogError("The provided asset path is not a valid folder path!");
        //    return null;
        //}

        ////if (!Directory.Exists(absolutePath))
        //{
        //    Debug.LogError("The directory corresponding to the provided asset path does not exist!");
        //    return null;
        //}

        return absolutePath;
    }
    public static string ConvertToAssetPath(string directoryPath)
    {
        string absolutePath = (directoryPath);

        //if (!absolutePath.StartsWith(Application.dataPath))
        //{
        //    Debug.LogError("The provided directory path is not located within the Assets folder!");
        //    return null;
        //}

        string relativePath = "Assets" + absolutePath.Substring(Application.dataPath.Length);

        return relativePath;
    }
    /// <summary>
    /// 鱼吃鱼项目的配置表路径
    /// </summary>
    /// <returns></returns>
    public static string GetSettingPathOfRusk()
    {
        return PathConverter.ConvertToDirectoryPath("Assets/Editor/Data.xlsx");
    }
}
