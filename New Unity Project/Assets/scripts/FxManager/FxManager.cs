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

    public ParticleSystem PlayFx(string FxName,bool isAutoDestory = true,Transform parent =null,float maxTimeDestory = 5f)
    {
        ParticleSystem clone;
        if (parent = null)
        {
            clone = Instantiate(FindFxByName(FxName), transform.position, transform.rotation, parent);
        }
        else
        {
            clone = Instantiate(FindFxByName(FxName), parent);
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
    public ParticleSystem PlayFx(string FxName,Vector3 scale, bool isAutoDestory = true, Transform parent = null, float maxTimeDestory = 5f)
    {
        ParticleSystem clone;
        if (parent = null)
        {
             clone = Instantiate(FindFxByName(FxName), transform.position, transform.rotation, parent);
        }
        else
        {
             clone = Instantiate(FindFxByName(FxName), parent);
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
