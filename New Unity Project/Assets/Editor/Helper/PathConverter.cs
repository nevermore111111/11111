using UnityEngine;
using UnityEditor;
using System.IO;

public static class PathConverter
{
    [MenuItem("Assets/GetPath",false)]
    public static string GetAndPrintPath()
    {
        string UnityPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        Debug.Log(UnityPath);
        return UnityPath;
    }
    public static string GetUnityPath()
    {
        string UnityPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        return UnityPath;
    }
    public static string GetDirectoryPath()
    {
        return (ConvertToDirectoryPath(GetUnityPath()));
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
}
