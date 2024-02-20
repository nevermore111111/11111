using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Lightbug.CharacterControllerPro.Core;
using Unity.VisualScripting;
using Sirenix.OdinInspector.Editor.Drawers;

namespace Lightbug.CharacterControllerPro.Demo
{

    //写一个行为树，用来设计这个人物的攻击模式
    //1追踪
    //2到了之后，等待1-3秒 ：{1徘徊，或者等待}。之后判断
    //如果对方远离就靠近，否则开始攻击{期间对方攻击，就防御}
    //攻击之后继续回到等待
    //防御成功就会反击，反击分成两种，一种直接反击，一种等待反击。反击结束回到等待
    //
    //
    //
    //
    //
    //进入这个状态后，有一定概率左右走走，然后再往前冲刺

    [AddComponentMenu("Character Controller Pro/Demo/Character/AI/Follow Behaviour")]
    public class AIFollowBehaviour : CharacterAIBehaviour
    {

        [Tooltip("The target transform used by the follow behaviour.")]
        [SerializeField]
        Transform followTarget = null;
        [SerializeField]
        int probabilityToIdle = 50;
        bool toIdle = false;
        float currentTimeToIdle;
        public float minTimeIdle = 1f;
        public float maxTimeIdle = 2f;


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

        }

        public override void UpdateBehaviour(float dt)
        {
            if (timer >= refreshTime)
            {
                timer = 0f;
                if (currentTimeToIdle > 0)
                {
                    currentTimeToIdle -= refreshTime;
                    UpdateIdleBehaviour(dt);
                }
                else
                {
                    UpdateFollowTargetBehaviour(dt);
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
                CharacterActor.brain.SetAIBehaviour<AIAttackBehaviour>();
                return;
            }
            if (navMeshPath.corners.Length > 1)
                SetMovementAction(path);
        }
        void UpdateIdleBehaviour(float dt)
        {
            //这里要左右走动
            Debug.Log("左右走");
            if (followTarget == null)
                return;

            characterActions.Reset();
            if ((CharacterActor.transform.position - CharacterActor.CharacterInfo.selectEnemy.transform.position).magnitude < reachDistance)
            {
                CharacterActor.brain.SetAIBehaviour<AIAttackBehaviour>();
            }
            SetMovementAction(CharacterActor.transform.right);
        }
    }
}
