using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BlazeAISpace;

public class AIanimatorFunction_King : MonoBehaviour
{
    //public BlazeAI BlazeAI;
    public Animator animator;
    public void Awake()
    {
        //BlazeAI = GetComponent<BlazeAI>();
        animator = GetComponent<Animator>();
    }
    //
}
