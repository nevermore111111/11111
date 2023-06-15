using Lightbug.CharacterControllerPro.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : CharacterInfo 
{
    public CharacterActor CharacterActor;
    protected override void Awake()
    {
        base.Awake();
        CharacterActor = GetComponentInParent<CharacterActor>();
    }

    internal bool GetIsAttacking()
    {
        
    }
}
