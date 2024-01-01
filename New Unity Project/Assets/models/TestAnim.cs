using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour
{
    Animator animator;
    public string name1;
    public string name2;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnGUI()
    {
        if(GUILayout.Button("≤‚ ‘1"))
        {
            animator.Play(name1);

        }
        if (GUILayout.Button("≤‚ ‘2"))
        {
            animator.Play(name2);

        }
    }
}
