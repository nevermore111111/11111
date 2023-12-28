
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class LoadTest : MonoBehaviour
{
    [SerializeField]
    private string labelToLoad = "aaaa"; // 替换成你想要加载的标签

    void Start()
    {
        LoadObjectsByLabelAsync();
    }

    private  void LoadObjectsByLabelAsync()
    {
        // 使用筛选委托加载标签为 labelToLoad 的 GameObject
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<UnityEngine.GameObject>(
            labelToLoad,
            null, // 这里使用筛选委托确保只加载 GameObject 类型的资源
            Addressables.MergeMode.Union
        );

        handle.Completed += OnLoadObjectsCompleted;
    }

     private void OnLoadObjectsCompleted(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> loadedObjects = handle.Result;

            foreach (var obj in loadedObjects)
            {
                // 处理每个加载的 GameObject 资源
                Debug.Log("Loaded GameObject: " + obj.name);
            }
        }

        // 释放加载操作的句柄
        Addressables.Release(handle);
    }
}
