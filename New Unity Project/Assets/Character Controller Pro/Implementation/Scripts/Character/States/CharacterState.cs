using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.Utilities;
using RootMotion;
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

        protected  static string verticalSpeedParameter = "VerticalSpeed";

        protected  static string planarSpeedParameter = "PlanarSpeed";

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
            //string className = this.GetType().Name;
            ////当进入对应模式的时候，去切换对应的timeline数组
            //if (timelineManager != null)
            //    timelineManager.SwapTimelinesByAssetName(className);
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

   

        public virtual string GetInfo()
        {
            return "";
        }

        /// <summary>
        /// Checks if the Animator component associated with the character is "valid" or not.
        /// </summary>
        /// <returns>True if the Animator is valid, false otherwise.</returns>
        public bool IsAnimatorValid() => CharacterActor.IsAnimatorValid();


        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        //————————————————————————————————————————写一个通用方法—————————————handleVelocity——————调节每个状态的速度——————————————————————————————
        [Header("开启显示自动调节速度的参数，只是显示，还要去代码里调用具体的方法")]
        public bool isActiveAutoHandleVelocity = false;
        [ShowIf("isActiveAutoHandleVelocity")]
        public PlanarMovementParameters PlanarMovementParameters;
        [ShowIf("isActiveAutoHandleVelocity")]
        public VerticalMovementParameters VerticalMovementParameters;

        protected virtual void ProcessVerticalMovement(float dt)
        {
            ProcessGravity(dt);
        }
        protected virtual void ProcessPlanarMovement(float dt)
        {


            bool needToAccelerate = false;//CustomUtilities.Multiply(CharacterStateController.InputMovementReference, currentPlanarSpeedLimit).sqrMagnitude >= CharacterActor.PlanarVelocity.sqrMagnitude;

            Vector3 targetPlanarVelocity = default;


            SetMotionValues(targetPlanarVelocity);


            float acceleration = currentMotion.acceleration;
            if (needToAccelerate)
            {
                acceleration *= currentMotion.angleAccelerationMultiplier;

                // Affect acceleration based on the angle between target velocity and current velocity
                //float angleCurrentTargetVelocity = Vector3.Angle(CharacterActor.PlanarVelocity, targetPlanarVelocity);
                //float accelerationBoost = 20f * (angleCurrentTargetVelocity / 180f);
                //acceleration += accelerationBoost;
            }
            else
            {
                acceleration = currentMotion.deceleration;
            }

            CharacterActor.PlanarVelocity = Vector3.MoveTowards(
                CharacterActor.PlanarVelocity,
                targetPlanarVelocity,
                acceleration * dt
            );

            //if (CharacterActor.UpdateRootPosition == true && CharacterActor.UseRootMotion == true)
            //{
            //    //这个是归一化的xyz
            //    //这个是归一化的想要移动的方向
            //    Vector3 targetVector3 = CharacterStateController.InputMovementReference;
            //    targetVector3 = CharacterActor.transform.InverseTransformDirection(targetVector3).normalized;
            //    //2.2f是防御的动画移动速度--只有一个防御，先用常数
            //    XYZMove = Vector3.MoveTowards(XYZMove, targetVector3, acceleration * dt / currentAnimSpeed);
            //    CharacterActor.Animator.SetFloat(xMovePar, XYZMove.x);
            //    CharacterActor.Animator.SetFloat(yMovePar, XYZMove.z);
            //}
            ////更新AI的行走方向——这个是更新动画用的
            //if (!CharacterActor.isPlayer)
            //{
            //    //去更新AI动画机
            //    Vector3 characterLocalVecolity = CharacterActor.LocalPlanarVelocity;
            //    Vector3 characterLocalVecolityNormalize = characterLocalVecolity * characterLocalVecolity.magnitude / planarMovementParameters.baseSpeedLimit;
            //    CharacterActor.Animator.SetFloat(xMovePar, characterLocalVecolityNormalize.x);
            //    CharacterActor.Animator.SetFloat(yMovePar, characterLocalVecolityNormalize.z);
            //}
        }


        void SetMotionValues(Vector3 targetPlanarVelocity)
        {
            float angleCurrentTargetVelocity = Vector3.Angle(CharacterActor.PlanarVelocity, targetPlanarVelocity);

            switch (CharacterActor.CurrentState)
            {
                case CharacterActorState.StableGrounded:

                    if (isDefense)
                    {
                        currentMotion.acceleration = defenseParameters.DefendGroundedAcceleration;//  planarMovementParameters.stableGroundedAcceleration;
                        currentMotion.deceleration = defenseParameters.DefendGroundedDeceleration;// planarMovementParameters.stableGroundedDeceleration;
                        currentMotion.angleAccelerationMultiplier = defenseParameters.DefendAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);
                        //planarMovementParameters.stableGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);
                    }
                    else
                    {
                        currentMotion.acceleration = planarMovementParameters.stableGroundedAcceleration;
                        currentMotion.deceleration = planarMovementParameters.stableGroundedDeceleration;
                        currentMotion.angleAccelerationMultiplier = planarMovementParameters.stableGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);
                    }


                    break;

                case CharacterActorState.UnstableGrounded:
                    currentMotion.acceleration = planarMovementParameters.unstableGroundedAcceleration;
                    currentMotion.deceleration = planarMovementParameters.unstableGroundedDeceleration;
                    currentMotion.angleAccelerationMultiplier = planarMovementParameters.unstableGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);

                    break;

                case CharacterActorState.NotGrounded:

                    if (reducedAirControlFlag)
                    {
                        float time = Time.time - reducedAirControlInitialTime;
                        if (time <= reductionDuration)
                        {
                            currentMotion.acceleration = (planarMovementParameters.notGroundedAcceleration / reductionDuration) * time;
                            currentMotion.deceleration = (planarMovementParameters.notGroundedDeceleration / reductionDuration) * time;
                        }
                        else
                        {
                            reducedAirControlFlag = false;

                            currentMotion.acceleration = planarMovementParameters.notGroundedAcceleration;
                            currentMotion.deceleration = planarMovementParameters.notGroundedDeceleration;
                        }

                    }
                    else
                    {
                        currentMotion.acceleration = planarMovementParameters.notGroundedAcceleration;
                        currentMotion.deceleration = planarMovementParameters.notGroundedDeceleration;
                    }

                    currentMotion.angleAccelerationMultiplier = planarMovementParameters.notGroundedAngleAccelerationBoost.Evaluate(angleCurrentTargetVelocity);

                    break;

            }


            // Material values
            if (materialController != null)
            {
                if (CharacterActor.IsGrounded)
                {
                    currentMotion.acceleration *= materialController.CurrentSurface.accelerationMultiplier * materialController.CurrentVolume.accelerationMultiplier;
                    currentMotion.deceleration *= materialController.CurrentSurface.decelerationMultiplier * materialController.CurrentVolume.decelerationMultiplier;
                }
                else
                {
                    currentMotion.acceleration *= materialController.CurrentVolume.accelerationMultiplier;
                    currentMotion.deceleration *= materialController.CurrentVolume.decelerationMultiplier;
                }
            }

        }

        protected virtual void ProcessGravity(float dt)
        {
            if (!VerticalMovementParameters.useGravity)
                return;
            VerticalMovementParameters.UpdateParameters();


            float gravityMultiplier = 1f;

            float gravity = gravityMultiplier * VerticalMovementParameters.gravity;

            if (!CharacterActor.IsStable)
                CharacterActor.VerticalVelocity += CustomUtilities.Multiply(-CharacterActor.Up, gravity, dt);
        }
    }
}
