using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FxManagerPro : MonoBehaviour
{
    private static FxManagerPro instance;

    // 使用字典存储特效，键是特效的名称，值是特效的预制体
    private Dictionary<string, GameObject> fxDictionary = new Dictionary<string, GameObject>();

    // 单例模式
    public static FxManagerPro Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FxManagerPro>();

                if (instance == null)
                {
                    GameObject managerObject = new GameObject("FxManagerPro");
                    instance = managerObject.AddComponent<FxManagerPro>();
                }
            }

            return instance;
        }
    }
    List<AsyncOperationHandle> Handles = new List<AsyncOperationHandle>();
    private void Awake()
    {
        AsyncOperationHandle<IList<GameObject>> handle2 = Addressables.LoadAssetsAsync<GameObject>(new List<string>() { "Fx" }, null, Addressables.MergeMode.Union);
        handle2.Completed += LoadObjectFromAddressables_Completed;
        Handles.Add(handle2);
    }
    private void LoadObjectFromAddressables_Completed(AsyncOperationHandle<IList<GameObject>> target)
    {
        if (target.Status == AsyncOperationStatus.Succeeded)
        {
            target.Result.ForEach(_ => { fxDictionary.Add(_.name, _); });
        }
        else
        {
           
                Debug.LogError($"Failed to load FX: {target.OperationException.Message}");
        }
    }
    // 加载特效
  

    // 播放特效
    public void PlayFx(string fxName, Vector3 position, Quaternion rotation)
    {
        if (fxDictionary.ContainsKey(fxName))
        {
            GameObject fxInstance = Instantiate(fxDictionary[fxName], position, rotation);
            DestroyFxAfterPlay(fxInstance);
        }
        else
        {
            Debug.LogError($"FX {fxName} not found in dictionary. Make sure to load it first.");
        }
    }

    // 停止特效
    public void StopFx(GameObject fxInstance)
    {
        Destroy(fxInstance);
    }

    // 销毁特效
    private async UniTask DestroyFxAfterPlay(GameObject fxInstance)
    {
        ParticleSystem particleSystem = fxInstance.GetComponent<ParticleSystem>();
        

        if (particleSystem != null)
        {
            await UniTask.Delay((int)((particleSystem.main.duration)*1000));
        }
        else
        {
            // 如果不是粒子系统，你可以根据实际情况进行调整
            await UniTask.Delay(1000);
        }

        StopFx(fxInstance);
    }
}
