using UnityEngine;
using UnityEditor;
using System.IO;

public class ConvertGenericToHumanoid : AssetPostprocessor
{
    // ������ģ��ʱ����
    //void OnPostprocessModel(GameObject model)
    //{
    //    // ���ģ���Ƿ��Ѿ�������ΪHumanoid
    //    if (model.GetComponent<Animator>() == null)
    //    {
    //        // ��ȡģ�͵�����
    //        ModelImporter importer = assetImporter as ModelImporter;

    //        // ����ģ��ΪHumanoid
    //        importer.animationType = ModelImporterAnimationType.Human;

    //        // �����ϣ���Զ���Avatar�����Դ���һ��HumanDescription�������importer
    //        // HumanDescription humanDescription = new HumanDescription();
    //        // importer.humanDescription = humanDescription;

    //        // ���µ���ģ��
    //        AssetDatabase.ImportAsset(importer.assetPath);
    //    }
    //}
    [MenuItem("Assets/����ת��", false, 1)] // ��Ӽ��� JSON �Ĳ˵���
    static public void ChangeFbx()
    {
        ModelImporter modelImporter;
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
                // ����ģ��ΪHumanoid
                importer.animationType = ModelImporterAnimationType.Human;

                // �����Ҫ�������ڴ˴��Զ���Avatar
                // HumanDescription humanDescription = new HumanDescription();
                // importer.humanDescription = humanDescription;

                // ���µ���ģ��
                AssetDatabase.ImportAsset(fbxFilePath);
                Debug.Log("Converted FBX to Humanoid: " + fbxFilePath);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Conversion of FBX files to Humanoid completed.");
    }

}