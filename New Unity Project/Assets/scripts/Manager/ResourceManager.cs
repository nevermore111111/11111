using System.Collections;
using System.Collections.Generic; // 添加此行以支持 List<string>
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    // 常驻资源的引用
    private GameObject persistentPrefab;
    private List<string> keys; // 修改此行，确保正确的 List 类型

    // 获取ResourceManager的实例
    public static ResourceManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ResourceManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ResourceManager");
                    instance = obj.AddComponent<ResourceManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 初始化 key 列表
            keys = new List<string>
            {
                "path/to/your/persistentPrefab"
                // 添加其他常驻资源的路径
            };

            // 在这里调用加载常驻资源的方法
            LoadPersistentResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadPersistentResources()
    {
        // 使用Addressable Assets加载常驻资源
        foreach (string key in keys)
        {
            LoadAsset(key);
        }
    }

    private void LoadAsset(string key)
    {
        Addressables.LoadAssetAsync<GameObject>(key).Completed +=
            (AsyncOperationHandle<GameObject> obj) =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    persistentPrefab = obj.Result;
                    // 在这里执行加载成功后的逻辑
                }
                else
                {
                    Debug.LogError($"Failed to load asset with key: {key}");
                }
            };
    }

    public GameObject GetPersistentPrefab()
    {
        // 返回常驻资源的引用
        return persistentPrefab;
    }
}
