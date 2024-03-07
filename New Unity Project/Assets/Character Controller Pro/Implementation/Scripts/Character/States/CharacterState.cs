using JetBrains.Annotations;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.Utilities;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;


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

        protected static string verticalSpeedParameter = "VerticalSpeed";

        protected static string planarSpeedParameter = "PlanarSpeed";

        protected static string horizontalAxisParameter = "HorizontalAxis";

        protected static string verticalAxisParameter = "VerticalAxis";

        protected static string heightParameter = "Height";

        protected static string GroundDistance = "GroundDistance";

        protected static string evadeParameter = "evade";

        protected static string spAttackParameter = "spAttack";
        protected static string inputMovePar = "inputMove";

        public static string stopParameter = "stop";

        protected static string xMovePar = "xMove";
        protected static string yMovePar = "yMove";

        protected static string defensePar = "defense";

       

        protected static string NormalMovementPar = "NormalMovement";
        protected static string AttackOnGroundPar = "AttackOnGround";
        protected static string perfectDefendKind = "perfectDefendKind";//完美防御所用的防御状态
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
                if (CharacterActor.IsPlayer)
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
            if (isActiveBaseAutoHandleVelocity)
            {
                Debug.Log($"{CharacterActor.name}的这个状态{GetType().Name}启用了自动调整位置");
            }
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
            if (CharacterActor.IsPlayer)
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
        /// 一个基础的设置速度的方法，如果需要就调用一下，需要开启isActiveAutoHandleVelocity。NormalMovement不需要调用这个
        /// </summary>
        /// <param name="dt"></param>
        public void BaseProcessVelocity(float dt)
        {
            if (isActiveBaseAutoHandleVelocity)
            {
                ProcessVerticalMovement(dt);
                ProcessPlanarMovement(dt);
            }
        }


        public void BaseProcessRotation(float dt)
        {
            if (isActiveAutoBaseHandleRotation)
            {
                HandleLookingDirection(dt);
            }
        }
        [ShowIf("IsShowBaseToggle")]
        public bool isActiveAutoBaseHandleRotation = false;
        [ShowIf("IsShowRotatePara")]
        public LookingDirectionParameters lookingDirectionParameters;

        protected Vector3 targetLookingDirection;
        public bool IsShowBaseToggle()
        {
            return this is not NormalMovement;
        }
        public bool IsShowRotatePara()
        {
            return isActiveAutoBaseHandleRotation || (this is NormalMovement);
        }

        protected virtual void HandleLookingDirection(float dt)
        {
            /*这段代码实现了角色的朝向控制功能，包括三种模式：Movement、ExternalReference、Target。

              在Movement模式下，根据角色的状态（NotGrounded、StableGrounded、UnstableGrounded）设置目标朝向。在ExternalReference模式下，将角色的目标朝向设置为MovementReferenceForward，即角色应朝向的参考方向。在Target模式下，将角色的目标朝向设置为目标位置与角色位置的向量。

              在代码中，使用SetTargetLookingDirection()函数设置目标朝向，并通过Quaternion计算出角色当前帧应该旋转的角度。最后，根据角色是否为2D游戏，使用不同的方式设置角色的朝向。如果是2D游戏，则直接设置角色的Yaw值为目标朝向的X值；如果是3D游戏，则将当前帧旋转的角度应用到角色的Forward向量上。*/
            if (!lookingDirectionParameters.changeLookingDirection)
                return;

            switch (lookingDirectionParameters.lookingDirectionMode)
            {
                case LookingDirectionParameters.LookingDirectionMode.Movement:

                    switch (CharacterActor.CurrentState)
                    {
                        case CharacterActorState.NotGrounded:

                            SetTargetLookingDirection(lookingDirectionParameters.notGroundedLookingDirectionMode);
                            break;
                        case CharacterActorState.StableGrounded:

                            SetTargetLookingDirection(lookingDirectionParameters.stableGroundedLookingDirectionMode);

                            break;
                        case CharacterActorState.UnstableGrounded:

                            SetTargetLookingDirection(lookingDirectionParameters.unstableGroundedLookingDirectionMode);

                            break;
                    }

                    break;

                case LookingDirectionParameters.LookingDirectionMode.ExternalReference:

                    if (!CharacterActor.CharacterBody.Is2D)
                        targetLookingDirection = CharacterStateController.MovementReferenceForward;

                    break;

                case LookingDirectionParameters.LookingDirectionMode.Target:

                    targetLookingDirection = (lookingDirectionParameters.target.position - CharacterActor.Position);
                    targetLookingDirection.Normalize();

                    break;
            }

            float targetRoteteSpeed = lookingDirectionParameters.speed;
            //if (IsDefense)
            //{
            //    targetRoteteSpeed *= defenseParameters.DefendLookDirecionLerpSpeed;
            //}

            Quaternion targetDeltaRotation = Quaternion.FromToRotation(CharacterActor.Forward, targetLookingDirection);
            Quaternion currentDeltaRotation = Quaternion.Slerp(Quaternion.identity, targetDeltaRotation, targetRoteteSpeed * dt);

            if (CharacterActor.CharacterBody.Is2D)
                CharacterActor.SetYaw(targetLookingDirection);
            else
                CharacterActor.SetYaw(currentDeltaRotation * CharacterActor.Forward);
        }

        void SetTargetLookingDirection(LookingDirectionParameters.LookingDirectionMovementSource lookingDirectionMode)
        {
            if (lookingDirectionMode == LookingDirectionParameters.LookingDirectionMovementSource.Input)
            {
                if (CharacterStateController.InputMovementReference != Vector3.zero)
                    targetLookingDirection = CharacterStateController.InputMovementReference;
                else
                    targetLookingDirection = CharacterActor.Forward;
            }
            else
            {
                if (CharacterActor.PlanarVelocity != Vector3.zero)
                    targetLookingDirection = Vector3.ProjectOnPlane(CharacterActor.PlanarVelocity, CharacterActor.Up);
                else
                    targetLookingDirection = CharacterActor.Forward;
            }
        }

        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        [ShowIf("IsShowBaseToggle")]
        public bool isActiveBaseAutoHandleVelocity = false;
        [ShowIf("IsShowMovePara")]
        public PlanarMovementParameters planarMovementParameters;
        [HideInInspector]
        protected PlanarMovementParameters.PlanarMovementProperties currentMotion;
        [ShowIf("IsShowMovePara")]
        public VerticalMovementParameters verticalMovementParameters;

        public bool IsShowMovePara()
        {
            return isActiveBaseAutoHandleVelocity||(this is NormalMovement);
        }

        protected virtual void ProcessVerticalMovement(float dt)
        {
            ProcessGravity(dt);
        }
        protected virtual void ProcessPlanarMovement(float dt)
        {
            Vector3 targetPlanarVelocity = default;

            SetMotionValues(targetPlanarVelocity);


            float acceleration;
            acceleration = currentMotion.deceleration;

            CharacterActor.PlanarVelocity = Vector3.MoveTowards(
                CharacterActor.PlanarVelocity,
                targetPlanarVelocity,
                acceleration * dt
            );
        }


        void SetMotionValues(Vector3 targetPlanarVelocity)
        {
            //返回当前的移动速度和想要加速的方向，当前这个方法会返回0度
            float angleCurrentTargetVelocity = Vector3.Angle(CharacterActor.PlanarVelocity, targetPlanarVelocity);

            switch (CharacterActor.CurrentState)
            {
                case CharacterActorState.StableGrounded:
                    currentMotion.acceleration = planarMovementParameters.stableGroundedAcceleration;
                    currentMotion.deceleration = planarMovementParameters.stableGroundedDeceleration;
                    currentMotion.angleAccelerationMultiplier = planarMovementParameters.stableGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);

                    break;
                case CharacterActorState.UnstableGrounded:
                    currentMotion.acceleration = planarMovementParameters.unstableGroundedAcceleration;
                    currentMotion.deceleration = planarMovementParameters.unstableGroundedDeceleration;
                    currentMotion.angleAccelerationMultiplier = planarMovementParameters.unstableGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);
                    break;
                case CharacterActorState.NotGrounded:
                    currentMotion.acceleration = planarMovementParameters.notGroundedAcceleration;
                    currentMotion.deceleration = planarMovementParameters.notGroundedDeceleration;
                    currentMotion.angleAccelerationMultiplier = planarMovementParameters.notGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);
                    break;

            }
        }

        protected virtual void ProcessGravity(float dt)
        {
            if (!verticalMovementParameters.useGravity)
                return;
            verticalMovementParameters.UpdateParameters();


            float gravityMultiplier = 1f;

            float gravity = gravityMultiplier * verticalMovementParameters.gravity;

            if (!CharacterActor.IsStable)
                CharacterActor.VerticalVelocity += CustomUtilities.Multiply(-CharacterActor.Up, gravity, dt);
        }
    }
}
