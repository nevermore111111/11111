using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReceive : MonoBehaviour
{
    public CharacterInfo CharacterInfo;

    public void Awake()
    {
        CharacterInfo = GetComponentInParent<CharacterInfo>();
    }
}
