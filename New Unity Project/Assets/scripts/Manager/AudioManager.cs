using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    private AudioSource audioSource;

    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();

                if (instance == null)
                {
                    GameObject managerObject = new GameObject("AudioManager");
                    instance = managerObject.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    List<AsyncOperationHandle> handles = new List<AsyncOperationHandle>();

    private void Awake()
    {

    }
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        AsyncOperationHandle<IList<AudioClip>> handle = Addressables.LoadAssetsAsync<AudioClip>(new List<string>() { "sound" }, null, Addressables.MergeMode.Union);
        handle.Completed += LoadAudioClipFromAddressables_Completed;
        handles.Add(handle);
    }

    private void LoadAudioClipFromAddressables_Completed(AsyncOperationHandle<IList<AudioClip>> target)
    {
        if (target.Status == AsyncOperationStatus.Succeeded)
        {
            target.Result.ForEach(audioClip => audioClips.Add(audioClip.name, audioClip));
        }
        else
        {
            Debug.LogError($"Failed to load audio clips: {target.OperationException.Message}");
        }
    }

    public void PlaySound(string soundName, float volumeScale)
    {
        if (audioClips.ContainsKey(soundName))
        {
            audioSource.PlayOneShot(audioClips[soundName],volumeScale);
        }
        else
        {
            Debug.LogError($"Sound {soundName}没找到，请确认是否加入addressable并且载入字典");
        }
    }
}
