using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    public FxHelper FxHelper;
    public ParticleSystem[] particleSystem;
    private float time;
    private List<ParticleSystem> particleSystemTodestory;
    float timeMaxDestory = 5f;
    public void Awake()
    {
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
    public ParticleSystem PlayFx(string FxName, Transform pos = null,bool isAutoDestory = true,Transform parent =null,float maxTimeDestory = 5f)
    {
        ParticleSystem clone;
        if ((parent = null) && (pos != null))
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
        if(isAutoDestory)
        {
            particleSystemTodestory.Add(clone);
        }
        if (maxTimeDestory != 5)
        {
            timeMaxDestory = time + maxTimeDestory;
        }
        return clone;
    }
    /// <summary>
    /// parent和pos不能同时为null
    /// </summary>
    /// <param name="FxName"></param>
    /// <param name="scale"></param>
    /// <param name="pos"></param>
    /// <param name="isAutoDestory"></param>
    /// <param name="parent"></param>
    /// <param name="maxTimeDestory"></param>
    /// <returns></returns>
    public ParticleSystem PlayFx(string FxName,Vector3 scale, Transform pos =null,bool isAutoDestory = true, Transform parent = null, float maxTimeDestory = 5f)
    {
        ParticleSystem clone;
        if ((parent = null) && (pos!=null))
        {
             clone = Instantiate(FindFxByName(FxName), pos.position, pos.rotation);
        }
        else if(parent!=null)
        {
             clone = Instantiate(FindFxByName(FxName), parent);
        }
        else
        {
            clone = Instantiate(FindFxByName(FxName));
        }
        clone.Play(true);
        clone.transform.localScale = scale;
        if (isAutoDestory)
        {
            particleSystemTodestory.Add(clone);
        }
        if(maxTimeDestory != 5)
        {
            timeMaxDestory = time + maxTimeDestory;
        }
        return clone;
    }

    private ParticleSystem FindFxByName(string name)
    {
        for(int i = 0; i < particleSystem.Length; i++)
        {
            if(particleSystem[i].name == name)
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
