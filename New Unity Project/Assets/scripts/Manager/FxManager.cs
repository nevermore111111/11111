using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    private static FxManager _instance; // Singleton instance

    public static FxManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FxManager>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("FxManager");
                    _instance = singletonObject.AddComponent<FxManager>();
                }
            }

            return _instance;
        }
    }

    public FxHelper FxHelper;
    public ParticleSystem[] particleSystem;
    private float time;
    private List<ParticleSystem> particleSystemTodestory;
    float timeMaxDestory = 5f;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (FxHelper == null)
        {

        }
        FxHelper = Resources.Load<FxHelper>("FxHelper");
        particleSystem = FxHelper.AllFx;
        particleSystemTodestory = new List<ParticleSystem>();
    }

    public void Start()
    {

    }
    /// <summary>
    /// parent和pos不能同时是null
    /// </summary>
    /// <param name="FxName"></param>
    /// <param name="pos"></param>
    /// <param name="isAutoDestory"></param>
    /// <param name="parent"></param>
    /// <param name="maxTimeDestory"></param>
    /// <returns></returns>
    public ParticleSystem PlayFx(string FxName, Transform pos = null, bool isAutoDestory = true, Transform parent = null, float maxTimeDestory = 5f)
    {
        ParticleSystem clone;
        if ((parent == null) && (pos != null))
        {
            clone = Instantiate(FindFxByName(FxName), pos.position, pos.rotation);
        }
        else if (parent != null)
        {
            clone = Instantiate(FindFxByName(FxName), parent);
        }
        else
        {
            clone = Instantiate(FindFxByName(FxName));
        }
        clone.Play(true);
        if (isAutoDestory)
        {
            particleSystemTodestory.Add(clone);
        }
        if (maxTimeDestory != 5)
        {
            timeMaxDestory = time + maxTimeDestory;
        }
        return clone;
    }
    public ParticleSystem PlayFx<T>(T FxName, Transform parent = null, float maxTimeDestory = 5f)
    {
        if (FxName is string)
        {
            // 如果是单个字符串，创建单个特效
            return PlaySingleFx((string)(object)FxName, parent, maxTimeDestory);
        }
        else if (FxName is string[])
        {
            // 如果是字符串数组，创建多个特效
            return PlayMultipleFx((string[])(object)FxName, parent, maxTimeDestory);
        }
        else
        {
            Debug.LogError("FxName类型不支持");
            return null;
        }
    }

    private ParticleSystem PlaySingleFx(string FxName, Transform parent = null, float maxTimeDestory = 5f)
    {
        if( FindFxByName(FxName) ==null) 
        {
            return null;
        }
        ParticleSystem clone;
        if (parent != null)
        {
            clone = Instantiate(FindFxByName(FxName), parent);
        }
        else
        {
            clone = Instantiate(FindFxByName(FxName));
        }

        clone.Play(true);
        return clone;
    }

    private ParticleSystem PlayMultipleFx(string[] FxNames, Transform parent = null, float maxTimeDestory = 5f)
    {
        ParticleSystem clone = null;
        foreach (var FxName in FxNames)
        {
            clone = PlaySingleFx(FxName, parent, maxTimeDestory);
        }
        return clone;
    }
    ///// <summary>
    ///// parent和pos不能同时为null
    ///// </summary>
    ///// <param name="FxName"></param>
    ///// <param name="scale"></param>
    ///// <param name="pos"></param>
    ///// <param name="isAutoDestory"></param>
    ///// <param name="parent"></param>
    ///// <param name="maxTimeDestory"></param>
    ///// <returns></returns>
    //public ParticleSystem PlayFx(string FxName, Vector3 scale, Transform pos = null, bool isAutoDestory = true, Transform parent = null, float maxTimeDestory = 5f)
    //{
    //    ParticleSystem clone;
    //    if ((parent == null) && (pos != null))
    //    {
    //        clone = Instantiate(FindFxByName(FxName), pos.position, pos.rotation);
    //    }
    //    else if (parent != null)
    //    {
    //        clone = Instantiate(FindFxByName(FxName), parent);
    //    }
    //    else
    //    {
    //        clone = Instantiate(FindFxByName(FxName));
    //    }
    //    clone.Play(true);
    //    clone.transform.localScale = scale;
    //    if (isAutoDestory)
    //    {
    //        particleSystemTodestory.Add(clone);
    //    }
    //    if (maxTimeDestory != 5)
    //    {
    //        timeMaxDestory = time + maxTimeDestory;
    //    }
    //    return clone;
    //}

    private ParticleSystem FindFxByName(string name)
    {
        if (particleSystem ==null)
        {
            return null;
        }
        for (int i = 0; i < particleSystem.Length; i++)
        {
            if (particleSystem[i].name == name)
            {
                return particleSystem[i];
            }
        }
        Debug.LogError("没找到对应的特效");
        return null;
    }
    public void Update()
    {
        time += Time.deltaTime;
        if (time >= timeMaxDestory)
        {
            particleSystemTodestory.RemoveAll(_ => _ == null);
            time = 0;
            for (int i = 0; i < particleSystemTodestory.Count; i++)
            {
                if (particleSystemTodestory[i] != null)
                {
                    if (!particleSystemTodestory[i].isPlaying)
                    {
                        Destroy(particleSystemTodestory[i].gameObject);
                    }
                }
            }
            timeMaxDestory = 5f;
        }
    }
}
