using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();

                if (instance == null)
                {
                    GameObject managerObject = new GameObject("SoundManager");
                    instance = managerObject.AddComponent<SoundManager>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if(audioSource == null)
        audioSource = FindFirstObjectByType<AudioSource>();
        DontDestroyOnLoad(gameObject);

        // 加载所有音频
        LoadSounds();
    }

    private async void LoadSounds()
    {
        AsyncOperationHandle<IList<AudioClip>> handle = Addressables.LoadAssetsAsync<AudioClip>(new List<string> { "Sound" }, null, Addressables.MergeMode.Union);
        await handle.Task;
        foreach (AudioClip clip in handle.Result)
        {
            soundDictionary.Add(clip.name, clip);
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(soundDictionary[soundName]);
        }
        else
        {
            Debug.LogError($"Sound {soundName} not found.");
        }
    }

    public void PlaySound(string[] soundNames)
    {
        foreach (string soundName in soundNames)
        {
            PlaySound(soundName);
        }
    }
}
