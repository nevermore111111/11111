using UnityEngine;
using UnityEditor;
using System.IO;

public class ConvertGenericToHumanoid : AssetPostprocessor
{
    [MenuItem("Assets/测试转化", false, 1)] // 添加加载 JSON 的菜单项
    static public void ChangeFbx()
    {
        //ModelImporter modelImporter;
        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        string fullPath  = PathConverter.ConvertToDirectoryPath(path);
        ConvertFBXFilesToHumanoid(fullPath);
    }






    static private void ConvertFBXFilesToHumanoid(string folderPath)
    {
        string[] fbxFiles = Directory.GetFiles(folderPath, "*.fbx", SearchOption.AllDirectories);

        foreach (string fbxFilePath in fbxFiles)
        {
            ModelImporter importer = AssetImporter.GetAtPath(PathConverter.ConvertToAssetPath(fbxFilePath)) as ModelImporter;

            if (importer != null)
            {
                // 设置模型为Humanoid
                importer.animationType = ModelImporterAnimationType.Human;

                // 如果需要，可以在此处自定义Avatar
                // HumanDescription humanDescription = new HumanDescription();
                // importer.humanDescription = humanDescription;

                // 重新导入模型
                AssetDatabase.ImportAsset(fbxFilePath);
                Debug.Log("Converted FBX to Humanoid: " + fbxFilePath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Conversion of FBX files to Humanoid completed.");
    }

}