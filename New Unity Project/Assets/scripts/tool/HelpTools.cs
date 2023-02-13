using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class HelpTools : MonoBehaviour
{
    
    static public Transform FindChildByName(Transform parent, string child)

    {
            Transform childTF = parent.Find(child);
            if (childTF != null)
            {
                return childTF;
            }
            for (int i = 0; i < parent.childCount; i++)
            {
                childTF = FindChildByName(parent.GetChild(i), child);
            }
            return childTF;
    }    
   
}
