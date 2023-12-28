
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class LoadTest : MonoBehaviour
{
    [SerializeField]
    private string labelToLoad = "aaaa"; // �滻������Ҫ���صı�ǩ

    void Start()
    {
        LoadObjectsByLabelAsync();
    }

    private  void LoadObjectsByLabelAsync()
    {
        // ʹ��ɸѡί�м��ر�ǩΪ labelToLoad �� GameObject
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<UnityEngine.GameObject>(
            labelToLoad,
            null, // ����ʹ��ɸѡί��ȷ��ֻ���� GameObject ���͵���Դ
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
                // ����ÿ�����ص� GameObject ��Դ
                Debug.Log("Loaded GameObject: " + obj.name);
            }
        }

        // �ͷż��ز����ľ��
        Addressables.Release(handle);
    }
}
