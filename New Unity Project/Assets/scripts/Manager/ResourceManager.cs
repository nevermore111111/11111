using System.Collections;
using System.Collections.Generic; // ��Ӵ�����֧�� List<string>
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance;

    // ��פ��Դ������
    private GameObject persistentPrefab;
    private List<string> keys; // �޸Ĵ��У�ȷ����ȷ�� List ����

    // ��ȡResourceManager��ʵ��
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

            // ��ʼ�� key �б�
            keys = new List<string>
            {
                "path/to/your/persistentPrefab"
                // ���������פ��Դ��·��
            };

            // ��������ü��س�פ��Դ�ķ���
            LoadPersistentResources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadPersistentResources()
    {
        // ʹ��Addressable Assets���س�פ��Դ
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
                    // ������ִ�м��سɹ�����߼�
                }
                else
                {
                    Debug.LogError($"Failed to load asset with key: {key}");
                }
            };
    }

    public GameObject GetPersistentPrefab()
    {
        // ���س�פ��Դ������
        return persistentPrefab;
    }
}
