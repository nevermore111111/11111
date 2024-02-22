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

    /// <summary>
    /// 使用字典存储特效，键是特效的名称，值是特效的预制体
    /// </summary>
    private Dictionary<string, GameObject> fxDictionary = new Dictionary<string, GameObject>();

    /// <summary>
    /// 单例模式
    /// </summary>
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
    /// <summary>
    /// 加载特效
    /// </summary>
    /// <param name="target"></param>
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
    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="fxName"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void PlayFx(string fxName, Vector3 position, Quaternion rotation)
    {
        Debug.Log("暂时不播放特效");
        return;
        if (fxDictionary.ContainsKey(fxName))
        {
            GameObject fxInstance = Instantiate(fxDictionary[fxName], position, rotation);
            DestroyFxAfterPlay(fxInstance);
        }
        else
        {
            Debug.LogError($"FX {fxName} 没找到");
        }
    }
    /// <summary>
    /// 播放特效
    /// </summary>
    /// <param name="fxName"></param>
    /// <param name="targetTransform"></param>
    public void PlayFx(string fxName, Transform targetTransform)=>PlayFx(fxName,targetTransform.position,targetTransform.rotation);
    public void PlayFx(string[] fxName, Transform targetTransform)
    {
        for(int i = 0; i < fxName.Length; i++) 
        {
            PlayFx(fxName[i], targetTransform.position, targetTransform.rotation);
        }
    }
  

    /// <summary>
    /// 停止特效
    /// </summary>
    /// <param name="fxInstance"></param>
    public void StopFx(GameObject fxInstance)
    {
        Destroy(fxInstance);
    }

    /// <summary>
    /// 销毁特效
    /// </summary>
    /// <param name="fxInstance"></param>
    /// <returns></returns>
    private async UniTask DestroyFxAfterPlay(GameObject fxInstance)
    {
        ParticleSystem particleSystem = fxInstance.GetComponent<ParticleSystem>();


        if (particleSystem != null)
        {
            await UniTask.Delay((int)((particleSystem.main.duration) * 1000+5000));
        }
        else
        {
            // 如果不是粒子系统，你可以根据实际情况进行调整
            await UniTask.Delay(1000);
        }

        StopFx(fxInstance);
    }
}
