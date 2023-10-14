using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FrameNum : MonoBehaviour
{
    public int frameNum = 60;
    private void Awake()
    {
        Application.targetFrameRate = frameNum;
    }
}
