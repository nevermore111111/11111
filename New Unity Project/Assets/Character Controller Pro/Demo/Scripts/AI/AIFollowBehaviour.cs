using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Lightbug.CharacterControllerPro.Core;


namespace Lightbug.CharacterControllerPro.Demo
{

    //这是一个AI行为模式，当进入这个AI后，等待1-2s后，如果存在目标，就去追，如果目标和自身距离小于reachDistance，就进入新的AI行为模式

    [AddComponentMenu("Character Controller Pro/Demo/Character/AI/Follow Behaviour")]
    public class AIFollowBehaviour : CharacterAIBehaviour
    {

        [Tooltip("The target transform used by the follow behaviour.")]
        [SerializeField]
        Transform followTarget = null;
        [SerializeField]
        int probabilityToIdle = 0;
        bool toIdle = false;
        float currentTimeToIdle;
        public float minTimeIdle = 1f;
        public float maxTimeIdle = 2f;
        public bool  isEnterAttack = true;


        [Tooltip("Desired distance to the target. if the distance to the target is less than this value the character will not move.")]
        [SerializeField]
        float reachDistance = 3f;

        [Tooltip("The wait time between actions updates.")]
        [Min(0f)]
        [SerializeField]
        float refreshTime = 0.65f;

        float timer = 0f;

        NavMeshPath navMeshPath = null;

        protected CharacterStateController stateController = null;

        protected override void Awake()
        {
            base.Awake();

            stateController = this.GetComponentInBranch<CharacterActor, CharacterStateController>();
            stateController.MovementReferenceMode = MovementReferenceParameters.MovementReferenceMode.World;
        }

        void OnEnable()
        {
            navMeshPath = new NavMeshPath();
        }

        public override void EnterBehaviour(float dt)
        {
            timer = refreshTime;
            if (probabilityToIdle > Random.Range(0, 100))
            {
                toIdle = true;
                currentTimeToIdle = Random.Range(minTimeIdle, maxTimeIdle);
            }
            else
            {
                currentTimeToIdle = 0f;
            }

        }

        public override void UpdateBehaviour(float dt)
        {
            if (timer >= refreshTime)
            {
                timer = 0f;
                if (currentTimeToIdle > 0)
                {
                    currentTimeToIdle -= refreshTime;
                    UpdateIdleBehaviour(refreshTime);
                }
                else
                {
                    UpdateFollowTargetBehaviour(refreshTime);
                }
            }
            else
            {
                timer += dt;
            }
        }

        // Follow Behaviour --------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the target to follow (only for the follow behaviour).
        /// </summary>
        public void SetFollowTarget(Transform followTarget, bool forceUpdate = true)
        {
            this.followTarget = followTarget;

            if (forceUpdate)
                timer = refreshTime + Mathf.Epsilon;
        }
        void UpdateFollowTargetBehaviour(float dt)
        {
            if (followTarget == null)
                return;

            characterActions.Reset();

            NavMesh.CalculatePath(transform.position, followTarget.position, NavMesh.AllAreas, navMeshPath);

            if (navMeshPath.corners.Length < 2)
                return;

            Vector3 path = navMeshPath.corners[1] - navMeshPath.corners[0];

            bool isDirectPath = navMeshPath.corners.Length == 2;
            if (isDirectPath && path.magnitude <= reachDistance)
            {
                //进入攻击状态
                if(isEnterAttack)
                CharacterActor.brain.SetAIBehaviour<AIAttackBehaviour>();
                return;
            }
            if (navMeshPath.corners.Length > 1)
            {
                SetMovementAction(path);
                //Debug.DrawLine(CharacterActor.transform.position, CharacterActor.transform.position + path,Color.red,0.5f);
            }
               
        }
        void UpdateIdleBehaviour(float dt)
        {
            //这里要左右走动
            if (followTarget == null)
                return;
            Debug.Log("待机时间");
            characterActions.Reset();
            if (CharacterActor.CharacterInfo.selectEnemy != null)
            {
                if ((CharacterActor.transform.position - CharacterActor.CharacterInfo.selectEnemy.transform.position).magnitude < reachDistance)
                {
                    CharacterActor.brain.SetAIBehaviour<AIAttackBehaviour>();
                }
                Vector3 MoveDirection;
                SetMovementAction(CharacterActor.transform.right);
            }

        }
    }
}
