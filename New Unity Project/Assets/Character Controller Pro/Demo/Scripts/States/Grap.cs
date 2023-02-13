using Lightbug.CharacterControllerPro.Implementation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// <summary>
public class Grap : CharacterState
{
   
    protected override void Awake()
    {
        base.Awake();   
    }
    protected override void Start()
    {
        base.Start();
        
    }
    public override void UpdateBehaviour(float dt)
    {
        if(CharacterActions.jump.value)
        {
            
            Grap01();
            //CharacterActor.ForceNotGrounded();
        }
        if(grappling)
        {
            CharacterActor.Velocity -= new Vector3(0,Grivaty*Time.deltaTime,0);
        }
        
    }

    public override void EnterBehaviour(float dt, CharacterState fromState)
    {
        base.EnterBehaviour(dt, fromState);
        CharacterActor.Velocity = new Vector3(0,0,0);
    }

    [Header("References")]
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrapplable;
    public float Grivaty;

    [Header("Grapping")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public LineRenderer Lr;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grappleTimer;
    [Space(10)]
    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    public bool grappling;

    private void Grap01()
    {
            StartGrapple();
        if (grappleTimer > 0)
        {
            grappleTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 这个方法用来设置线性渲染器的位置；
    /// </summary>
    private void LaterGrap()
    {
        if (grappling)
        {
            Lr.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grappleTimer > 0) return;
        grappling = true;

        RaycastHit Hit;
        if (Physics.Raycast(cam.position, cam.forward, out Hit, maxGrappleDistance, whatIsGrapplable))
        {
            grapplePoint = Hit.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        Lr.enabled = true;
        Lr.SetPosition(1, grapplePoint);
    }
    private void ExecuteGrapple()
    {
        Vector3 vecs = CalVocality(CharacterActor.Position, grapplePoint, UnityEngine.Random.Range(0f, 2f), 9.8f);
        CharacterActor.ForceNotGrounded();
        CharacterActor.Velocity = vecs;
        Debug.Log("弹射");
    }
    private void StopGrapple()
    {
        grappling = false;
        grappleTimer = grapplingCd;
        Lr.enabled = false;
    }


    private Vector3 CalVocality(Vector3 start, Vector3 target, float spHeigh, float grivaty)
    {
        grivaty = Grivaty;
        //自己填写重力
        float delY = target.y - start.y;
        float delX = target.x - start.x;
        float delZ = target.z - start.z;

        float vy = MathF.Sqrt(2f * ((delY) + spHeigh) * grivaty);
        float vXZ = Mathf.Sqrt(((Mathf.Pow(delX, 2) + Mathf.Pow(delZ, 2)) * grivaty / (2 * delY + 4 * spHeigh)));
        Vector3 med = new(delX, 0, delZ);
        Vector3 end = med.normalized * vXZ + new Vector3(0, vy, 0);
        return end;
    }
}
