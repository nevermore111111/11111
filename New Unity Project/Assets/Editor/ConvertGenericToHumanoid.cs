using UnityEngine;
using UnityEditor;

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
    [MenuItem("Assets/����ת��",false,1)] // ��Ӽ��� JSON �Ĳ˵���
    static public void ChangeFbx()
    {
        ModelImporter modelImporter;
        string path = AssetDatabase.GUIDToAssetPath( Selection.assetGUIDs[0]);
        modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
        Debug.Log(modelImporter.name);
    }
}