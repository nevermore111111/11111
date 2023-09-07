using UnityEngine;
using UnityEditor;

public class ConvertGenericToHumanoid : AssetPostprocessor
{
    // 当导入模型时触发
    //void OnPostprocessModel(GameObject model)
    //{
    //    // 检查模型是否已经被导入为Humanoid
    //    if (model.GetComponent<Animator>() == null)
    //    {
    //        // 获取模型导入器
    //        ModelImporter importer = assetImporter as ModelImporter;

    //        // 设置模型为Humanoid
    //        importer.animationType = ModelImporterAnimationType.Human;

    //        // 如果您希望自定义Avatar，可以创建一个HumanDescription并分配给importer
    //        // HumanDescription humanDescription = new HumanDescription();
    //        // importer.humanDescription = humanDescription;

    //        // 重新导入模型
    //        AssetDatabase.ImportAsset(importer.assetPath);
    //    }
    //}
    [MenuItem("Assets/测试转化",false,1)] // 添加加载 JSON 的菜单项
    static public void ChangeFbx()
    {
        ModelImporter modelImporter;
        string path = AssetDatabase.GUIDToAssetPath( Selection.assetGUIDs[0]);
        modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
        Debug.Log(modelImporter.name);
    }
}