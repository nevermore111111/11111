using System.Collections.Generic;
using UnityEngine;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.Utilities;


namespace Lightbug.CharacterControllerPro.Implementation
{



    /// <summary>
    /// This class represents a state, that is, a basic element used by the character state controller (finite state machine).
    /// </summary>
    public abstract class CharacterState : MonoBehaviour, IUpdatable
    {
        [SerializeField]
        bool overrideAnimatorController = true;

        [Condition("overrideAnimatorController", ConditionAttribute.ConditionType.IsTrue)]
        [SerializeField]
        RuntimeAnimatorController runtimeAnimatorController = null;

        static public bool canPlayerControl = false;
        static public bool isTimelineAnimation = false;
        /// <summary>
        /// Gets the hash value (Animator) associated with this state, based on its name.
        /// </summary>
        public int StateNameHash { get; private set; }

        public TimelineManager timelineManager;


        /// <summary>
        /// Gets the state runtime animator controller.
        /// </summary>
        public RuntimeAnimatorController RuntimeAnimatorController => runtimeAnimatorController;


        public bool OverrideAnimatorController => overrideAnimatorController;

        /// <summary>
        /// Gets the CharacterActor component of the gameObject.
        /// </summary>
        public CharacterActor CharacterActor { get; private set; }

        /// <summary>
        /// Gets the CharacterBrain component of the gameObject.
        /// </summary>
        // public CharacterBrain CharacterBrain{ get; private set; }
        CharacterBrain CharacterBrain = null;

        /// <summary>
        /// Gets the current brain actions CharacterBrain component of the gameObject.
        /// </summary>
        public CharacterActions CharacterActions
        {
            get
            {
                return CharacterBrain == null ? new CharacterActions() : CharacterBrain.CharacterActions;
            }
        }

        /// <summary>
        /// Gets the CharacterStateController component of the gameObject.
        /// </summary>
        public CharacterStateController CharacterStateController { get; private set; }




        protected static string groundedParameter = "Grounded";

        protected static string stableParameter = "Stable";

        protected  static string verticalSpeedParameter = "VerticalSpeed";

        protected  static string planarSpeedParameter = "PlanarSpeed";

        protected static string horizontalAxisParameter = "HorizontalAxis";

        protected static string verticalAxisParameter = "VerticalAxis";

        protected static string heightParameter = "Height";

        protected static string GroundDistance = "GroundDistance";

        protected static string evadeParameter = "evade";

        protected static string spAttackParameter = "spAttack";

        public static string stopParameter = "stop";

        protected static string xMovePar = "xMove";
        protected static string yMovePar = "yMove";

        protected static string defensePar = "defense";

        protected static string startDefensePar = "startDefense";
        protected static string endDefensePar = "endDefense";

        protected static string NormalMovementPar = "NormalMovement";
        protected static string AttackOnGroundPar = "AttackOnGround";
        private static int spAttack = -1;

        //可以进入空中攻击的最低限度
        public static float HightCanAttackInAir = 0.8f;

        public int SpAttack
        {
            get
            {
                return spAttack;
            }
            set
            {
                if (CharacterActor.isPlayer)
                {
                    spAttack = value;
                    CharacterActor.Animator.SetInteger(spAttackParameter, value);
                }
            }
        }

        protected virtual void Awake()
        {
            CharacterActor = this.GetComponentInBranch<CharacterActor>();
            CharacterStateController = this.GetComponentInBranch<CharacterActor, CharacterStateController>();
            CharacterBrain = this.GetComponentInBranch<CharacterActor, CharacterBrain>();
        }

        public virtual void CanPlayerControl(bool canControl)
        {
            canPlayerControl = canControl;
        }

        /// <summary>
        /// 是否能进行空中攻击，如果可以，才会进行
        /// </summary>
        public bool canAttackInair => (!CharacterActor.IsGrounded) && (CharacterActor.PredictedGroundDistance > HightCanAttackInAir);

        protected virtual void Start()
        {
            StateNameHash = Animator.StringToHash(this.GetType().Name);
            timelineManager = this.transform.parent.GetComponentInChildren<TimelineManager>();
        }


        /// <summary>
        /// This method runs once when the state has entered the state machine.
        /// </summary>
        public virtual void EnterBehaviour(float dt, CharacterState fromState)
        {
            string className = this.GetType().Name;
            //当进入对应模式的时候，去切换对应的timeline数组
            if (timelineManager != null)
                timelineManager.SwapTimelinesByAssetName(className);
        }

        /// <summary>
        /// This methods runs before the main Update method.
        /// </summary>
        public virtual void PreUpdateBehaviour(float dt)
        {
        }

        /// <summary>
        /// This method runs frame by frame, and should be implemented by the derived state class.
        /// </summary>
        public virtual void UpdateBehaviour(float dt)
        {

        }

        /// <summary>
        /// This methods runs after the main Update method.
        /// </summary>
        public virtual void PostUpdateBehaviour(float dt)
        {
        }



        /// <summary>
        /// This methods runs just before the character physics simulation.
        /// </summary>
        public virtual void PreCharacterSimulation(float dt)
        {
            if (!CharacterActor.IsAnimatorValid())
                return;

            CharacterStateController.Animator.SetBool(groundedParameter, CharacterActor.IsGrounded);
            CharacterStateController.Animator.SetBool(stableParameter, CharacterActor.IsStable);
            CharacterStateController.Animator.SetFloat(horizontalAxisParameter, CharacterActions.movement.value.x);
            CharacterStateController.Animator.SetFloat(verticalAxisParameter, CharacterActions.movement.value.y);
            CharacterStateController.Animator.SetFloat(heightParameter, CharacterActor.BodySize.y);
            if (CharacterActor.isPlayer)
                CharacterStateController.Animator.SetFloat(GroundDistance, CharacterActor.PredictedGroundDistance);
        }

        /// <summary>
        /// This methods runs after the character physics simulation.
        /// </summary>
        public virtual void PostCharacterSimulation(float dt)
        {
        }

        /// <summary>
        /// This method runs once when the state has exited the state machine.
        /// </summary>
        public virtual void ExitBehaviour(float dt, CharacterState toState)
        {
        }

        /// <summary>
        /// Checks if the required conditions to exit this state are true. If so it returns the desired state (null otherwise). After this the state machine will
        /// proceed to evaluate the "enter transition" condition on the target state.
        /// </summary>
        public virtual void CheckExitTransition()
        {

        }

        /// <summary>
        /// Checks if the required conditions to enter this state are true. If so the state machine will automatically change the current state to the desired one.
        /// </summary>  
        public virtual bool CheckEnterTransition(CharacterState fromState)
        {
            return true;
        }

        /// <summary>
        /// This methods runs after getting all the ik positions, rotations and their respective weights. Use it to modify the ik data of the humanoid rig.
        /// </summary>
        public virtual void UpdateIK(int layerIndex)
        {
        }

        public void SetIKPos(AvatarIKGoal targetAvatarIK, Transform targetTransform, float targetWeight)
        {
            CharacterActor.Animator.SetIKPosition(targetAvatarIK, targetTransform.position);
            CharacterActor.Animator.SetIKPositionWeight(targetAvatarIK, targetWeight);
        }
        public void SetIKRotate(AvatarIKGoal targetAvatarIK, Transform targetTransform, float targetWeight)
        {
            CharacterActor.Animator.SetIKRotation(targetAvatarIK, transform.rotation);
            CharacterActor.Animator.SetIKRotationWeight(targetAvatarIK, targetWeight);
        }
        public void SetIKbyTransform(AvatarIKGoal targetAvatarIK, Transform targetTransform, float tarPosWeight, float tarRotateWeight)
        {
            SetIKPos(targetAvatarIK, targetTransform, tarPosWeight);
            SetIKRotate(targetAvatarIK, targetTransform, tarRotateWeight);
        }
        public void SetIKbyIKPar(IKPar iKPar)
        {
            switch (iKPar.targetIKPos)
            {
                case AvatarIKGoal.LeftHand:
                    SetIKbyTransform(AvatarIKGoal.LeftHand, iKPar.targetTransform, iKPar.ikPosWeight, iKPar.ikRotateWeight);
                    break;
                case AvatarIKGoal.RightHand:
                    SetIKbyTransform(AvatarIKGoal.RightHand, iKPar.targetTransform, iKPar.ikPosWeight, iKPar.ikRotateWeight);
                    break;
                case AvatarIKGoal.LeftFoot:
                    SetIKbyTransform(AvatarIKGoal.LeftFoot, iKPar.targetTransform, iKPar.ikPosWeight, iKPar.ikRotateWeight);
                    break;
                case AvatarIKGoal.RightFoot:
                    SetIKbyTransform(AvatarIKGoal.RightFoot, iKPar.targetTransform, iKPar.ikPosWeight, iKPar.ikRotateWeight);
                    break;
            }
        }

        public virtual string GetInfo()
        {
            return "";
        }

        /// <summary>
        /// Checks if the Animator component associated with the character is "valid" or not.
        /// </summary>
        /// <returns>True if the Animator is valid, false otherwise.</returns>
        public bool IsAnimatorValid() => CharacterActor.IsAnimatorValid();


        /// <summary>
        /// 这个方法让使用timeline播放的动画实现rootMovtion的效果
        /// 必须先开启characteractor的rootmotion
        /// 如果有多个动画，只会播放最后一个动画的rootmovtion
        /// 第二行个人情况开启或者关闭
        /// </summary>
        //public void SetTimeLineAnimationRootMotion(bool startRootMovtion)
        //{
        //    isTimelineAnimation = startRootMovtion;
        //    CharacterActor.SetUpRootMotion(startRootMovtion, false);
        //}
    }
    [System.Serializable]
    public class IKPar
    {
        public AvatarIKGoal targetIKPos;
        //这个类会在timeline中变化值，来满足一些动画的需要，比如ik权重，或者是技能挂点位置等，暂时没有想好
        public float ikPosWeight;
        public float ikRotateWeight;
        public Transform targetTransform;

        public void Initialize()
        {
            ikPosWeight = 0f;
            ikRotateWeight = 0f;
            targetTransform = null;
        }
    }
}
