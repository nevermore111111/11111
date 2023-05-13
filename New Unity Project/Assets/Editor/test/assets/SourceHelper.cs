using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SourceHelper : ScriptableObject
{
    private const string AssetPath = "Assets/Editor/test/assets/SourceHelperConfig.asset";
    [Tooltip("资源种类")]
    [Header("种类")]
    [Space(10)]
    [SerializeField]
    string[] kind;


    /// <summary>
    /// 创建
    /// </summary>
    [MenuItem("CreateAsset/Test")]
    static void Create()
    {
        ScriptableObject asset = ScriptableObject.CreateInstance<SourceHelper>();
        string savePath = AssetPath;
        AssetDatabase.CreateAsset(asset, savePath);
    }
    /// <summary>
    /// 载入asset的配置
    /// </summary>
    [MenuItem("Assets/测试")]
    static void Test()
    {
        SourceHelper sourceHelper = AssetDatabase.LoadAssetAtPath<SourceHelper>(AssetPath);
        
    }
    [CanEditMultipleObjects()]
    [CustomEditor(typeof(SourceHelper), true)]
    public class Source : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();
            if (GUILayout.Button("执行分配"))
            {
                if (EditorUtility.DisplayDialog("提示", "是否要保存修改？", "是", "否"))
                {
                    // 用户点击了“是”按钮
                }
                else
                {
                    // 用户点击了“否”按钮
                }
            }
        }
    }
}
