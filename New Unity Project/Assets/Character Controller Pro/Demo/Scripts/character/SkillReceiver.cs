using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���λ�õ���
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
