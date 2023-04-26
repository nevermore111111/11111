using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Test000001 : MonoBehaviour
{
    public PlayableDirector PlayableDirector;
    private void OnGUI()
    {
        if(GUILayout.Button("≤‚ ‘"))
        {
            PlayableDirector.Play();
        }
    }
}
