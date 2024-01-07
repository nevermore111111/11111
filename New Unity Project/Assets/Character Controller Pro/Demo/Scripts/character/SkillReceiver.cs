using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//标记位置的类
public class SkillReceiver : MonoBehaviour
{
    public int skillPoint;
    [HideInInspector]
    private CharacterInfo characterInfo;
    private void Awake()
    {
        characterInfo = GetComponentInParent<CharacterInfo>();
        characterInfo.allSkillReceivers.Add(this);
    }
}
