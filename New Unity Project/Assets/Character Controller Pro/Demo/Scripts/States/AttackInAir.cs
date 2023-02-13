using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class AttackInAir : Attack
{
    public override void UpdateBehaviour(float dt)
    {
        base.UpdateBehaviour(dt);
        
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(" ‰»Îl");
            //this.transform.parent.GetComponent<CharacterBody>().RigidbodyComponent.AddForce(Vector3.one, false, false);
            
        }
    }
}
