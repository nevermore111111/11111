using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lightbug.CharacterControllerPro.Core 
{
    /// <summary>
    /// 
    /// <summary>
    public class rigidTest : MonoBehaviour
    {
        CharacterBody characterBody;
        private void Start()
        {
            characterBody = GetComponent<CharacterBody>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Debug.Log("shijiali");
                //characterBody.RigidbodyComponent.AddForce(Vector3.forward, false, false);
                
            }
        }
    }
}
