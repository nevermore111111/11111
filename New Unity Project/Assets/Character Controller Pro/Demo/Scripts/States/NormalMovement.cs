using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using Rusk;
using System;
using UnityEngine;
using static Lightbug.CharacterControllerPro.Core.PhysicsActor;
using Cinemachine;
using System.Collections;

namespace Lightbug.CharacterControllerPro.Demo
{
    [AddComponentMenu("Character Controller Pro/Demo/Character/States/Normal Movement")]
    public class NormalMovement : CharacterState
    {
        public bool IsPlayer;

        [Space(10)]

        public PlanarMovementParameters planarMovementParameters = new PlanarMovementParameters();

        public VerticalMovementParameters verticalMovementParameters = new VerticalMovementParameters();

        public CrouchParameters crouchParameters = new CrouchParameters();

        public LookingDirectionParameters lookingDirectionParameters = new LookingDirectionParameters();

        //自己加
        private Attack attack;
        private WeaponManager[] weaponManager;

       



        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────


        #region Events	

        /// <summary>
        /// Event triggered when the character jumps.
        /// </summary>
        public event System.Action OnJumpPerformed;

        /// <summary>
        /// Event triggered when the character jumps from the ground.
        /// </summary>
        public event System.Action<bool> OnGroundedJumpPerformed;

        /// <summary>
        /// Event triggered when the character jumps while.
        /// </summary>
        public event System.Action<int> OnNotGroundedJumpPerformed;

        #endregion


        protected MaterialController materialController = null;
        protected int notGroundedJumpsLeft = 0;
        protected bool isAllowedToCancelJump = false;
        protected bool wantToRun = false;
        protected float currentPlanarSpeedLimit = 0f;

        protected bool groundedJumpAvailable = true;
        protected Vector3 jumpDirection = default(Vector3);

        protected Vector3 targetLookingDirection = default(Vector3);
        protected float targetHeight = 1f;

        protected bool wantToCrouch = false;
        protected bool isCrouched = false;

        public PlanarMovementParameters.PlanarMovementProperties currentMotion = new PlanarMovementParameters.PlanarMovementProperties();
        bool reducedAirControlFlag = false;
        float reducedAirControlInitialTime = 0f;
        float reductionDuration = 0.5f;

        //上一帧的速度
        float lastVelocityMagnitude;

        protected override void Awake()
        {
            base.Awake();

            notGroundedJumpsLeft = verticalMovementParameters.availableNotGroundedJumps;

            materialController = this.GetComponentInBranch<CharacterActor, MaterialController>();

            attack = this.GetComponent<Attack>();
            weaponManager = GetComponentsInChildren<WeaponManager>();
        }

        protected virtual void OnValidate()
        {
            verticalMovementParameters.OnValidate();
        }

        protected override void Start()
        {
            base.Start();

            targetHeight = CharacterActor.DefaultBodySize.y;

            float minCrouchHeightRatio = CharacterActor.BodySize.x / CharacterActor.BodySize.y;
            crouchParameters.heightRatio = Mathf.Max(minCrouchHeightRatio, crouchParameters.heightRatio);
        }

        protected virtual void OnEnable()
        {
            CharacterActor.OnTeleport += OnTeleport;
        }

        protected virtual void OnDisable()
        {
            CharacterActor.OnTeleport -= OnTeleport;
        }

        public override string GetInfo()
        {
            return "This state serves as a multi purpose movement based state. It is responsible for handling gravity and jump, walk and run, crouch, " +
            "react to the different material properties, etc. Basically it covers all the common movements involved " +
            "in a typical game, from a 3D platformer to a first person walking simulator.";
        }

        void OnTeleport(Vector3 position, Quaternion rotation)
        {
            targetLookingDirection = CharacterActor.Forward;
            isAllowedToCancelJump = false;
        }

        /// <summary>
        /// Gets/Sets the useGravity toggle. Use this property to enable/disable the effect of gravity on the character.
        /// </summary>
        /// <value></value>
        public bool UseGravity
        {
            get => verticalMovementParameters.useGravity;
            set => verticalMovementParameters.useGravity = value;
        }

        //我现在想做的：按下和长按触发不同效果，如果是短按，那么就正常触发，如果是长按，那么就进入特殊攻击



        public override void CheckExitTransition()
        {
            if (IsPlayer)
            {
                if (CanEvade())
                {
                    CharacterStateController.EnqueueTransition<Evade>();
                }
                if (CharacterActions.attack.value)
                {
                    if (CharacterActor.IsGrounded)
                    {
                        if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround)
                        {
                            CharacterStateController.EnqueueTransition<AttackOnGround>();
                        }
                        else if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround_fist)
                        {
                            CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
                        }
                    }
                    else if (!CharacterActor.IsGrounded)
                    {
                        if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround)
                        {

                        }
                        else if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround_fist)
                        {

                        }
                    }
                }
                if (CharacterActions.spAttack.value)//特殊攻击
                {
                    if (CharacterActor.IsGrounded)
                    {
                        if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround)
                        {
                            //attack
                            SpAttack = 10;
                            CharacterStateController.EnqueueTransition<AttackOnGround>();
                        }
                        else if (Attack.currentAttackMode == Attack.AttackMode.AttackOnGround_fist)
                        {
                            SpAttack = 11;
                            CharacterStateController.EnqueueTransition<AttackOnGround_fist>();
                        }
                    }
                    else
                    {
                        //在天空使用特殊攻击
                        SpAttack = 13;
                        CharacterStateController.EnqueueTransition<AttackOffGround>();
                    }
                }
                if (CharacterActions.jetPack.value)
                {
                    CharacterStateController.EnqueueTransition<JetPack>();
                }
                else if (CharacterActions.dash.Started)
                {
                    CharacterStateController.EnqueueTransition<Dash>();
                }
                else if (CharacterActor.Triggers.Count >= 1)
                {
                    CharacterStateController.EnqueueTransition<LadderClimbing>();
                    CharacterStateController.EnqueueTransition<RopeClimbing>();
                }
                else if (!CharacterActor.IsGrounded)
                {
                    if (!CharacterActions.crouch.value)
                        CharacterStateController.EnqueueTransition<WallSlide>();

                    CharacterStateController.EnqueueTransition<LedgeHanging>();
                }
            }

        }

        public override void ExitBehaviour(float dt, CharacterState toState)
        {
            reducedAirControlFlag = false;
            moving = false;
            if (IsPlayer)
            {
                CharacterActor.Animator.SetBool("jump", false);
            }
        }



        /// <summary>
        /// Reduces the amount of acceleration and deceleration (not grounded state) until the character reaches the apex of the jump 
        /// (vertical velocity close to zero). This can be useful to prevent the character from accelerating/decelerating too quickly (e.g. right after performing a wall jump).
        /// </summary>
        public void ReduceAirControl(float reductionDuration = 0.5f)
        {
            reducedAirControlFlag = true;
            reducedAirControlInitialTime = Time.time;
            this.reductionDuration = reductionDuration;
        }

        void SetMotionValues(Vector3 targetPlanarVelocity)
        {
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

        /// <summary>
        /// Processes the lateral movement of the character (stable and unstable state), that is, walk, run, crouch, etc. 
        /// This movement is tied directly to the "movement" character action.
        /// 喵喵喵，这个方法是处理角色平面移动的喵！
        ///首先，获取当前地面行走速度的上限，并计算出速度倍数喵。如果需要加速，就根据输入方向和速度上限计算目标速度向量喵。然后根据角色状态进行不同的处理喵：
        ///如果角色在空中，就更新当前速度上限喵，并根据输入方向和速度倍数计算目标速度喵。
        ///如果角色在稳定地面上，就先处理奔跑状态的输入喵，如果要蹲下或不能奔跑，就不能奔跑喵。然后根据当前状态，设置当前速度上限，如果蹲着，速度上限乘以一个蹲着速度倍数，否则，如果要奔跑，速度上限就是奔跑速度上限，否则就是基本速度上限喵。然后，根据输入方向和速度倍数计算目标速度喵。
        /// 如果角色在不稳定地面上，就将当前速度上限设置为基本速度上限，然后根据输入方向和速度倍数计算目标速度喵。
        /// 最后，根据是否需要加速来计算角色的加速度喵，如果需要加速，根据角度差来计算加速度的增益喵；否则就使用当前动作的减速度喵。最后，使用MoveTowards方法，根据当前速度、目标速度和加速度来更新角色的平面速度喵。

        /// </summary>
        protected virtual void ProcessPlanarMovement(float dt)
        {
            //SetMotionValues();



            PlayStop();

            float speedMultiplier = materialController != null ?
            materialController.CurrentSurface.speedMultiplier * materialController.CurrentVolume.speedMultiplier : 1f;


            bool needToAccelerate = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, currentPlanarSpeedLimit).sqrMagnitude >= CharacterActor.PlanarVelocity.sqrMagnitude;

            Vector3 targetPlanarVelocity = default;
            switch (CharacterActor.CurrentState)
            {
                case CharacterActorState.NotGrounded:

                    if (CharacterActor.WasGrounded)
                        currentPlanarSpeedLimit = Mathf.Max(CharacterActor.PlanarVelocity.magnitude, planarMovementParameters.baseSpeedLimit);


                    //needToAccelerate = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, currentPlanarSpeedLimit).sqrMagnitude >= CharacterActor.PlanarVelocity.sqrMagnitude;
                    targetPlanarVelocity = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, speedMultiplier, currentPlanarSpeedLimit);

                    //GetAccelerationBoost(targetPlanarVelocity)
                    break;
                case CharacterActorState.StableGrounded:


                    // Run ------------------------------------------------------------
                    if (planarMovementParameters.runInputMode == InputMode.Toggle)
                    {
                        if (CharacterActions.run.Started)
                            wantToRun = !wantToRun;
                    }
                    else
                    {
                        wantToRun = CharacterActions.run.value;
                    }

                    if (wantToCrouch || !planarMovementParameters.canRun)
                        wantToRun = false;


                    if (isCrouched)
                    {
                        currentPlanarSpeedLimit = planarMovementParameters.baseSpeedLimit * crouchParameters.speedMultiplier;
                    }
                    else
                    {
                        currentPlanarSpeedLimit = wantToRun ? planarMovementParameters.boostSpeedLimit : planarMovementParameters.baseSpeedLimit;
                    }

                    targetPlanarVelocity = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, speedMultiplier, currentPlanarSpeedLimit);

                    //needToAccelerate = CharacterStateController.InputMovementReference != Vector3.zero;


                    break;
                case CharacterActorState.UnstableGrounded:

                    currentPlanarSpeedLimit = planarMovementParameters.baseSpeedLimit;

                    //needToAccelerate = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, currentPlanarSpeedLimit).sqrMagnitude >= CharacterActor.PlanarVelocity.sqrMagnitude;
                    targetPlanarVelocity = CustomUtilities.Multiply(CharacterStateController.InputMovementReference, speedMultiplier, currentPlanarSpeedLimit);


                    break;
            }

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
        }

        public bool moving = false; // 初始状态为未移动

        private float startTime; // 移动开始时间

        void MoveTime()
        {
            if (CharacterActions.movement.value.sqrMagnitude != 0) // 检测移动
            {
                if (startTime == 0f)
                {
                    startTime = Time.time; // 如果移动开始，则记录时间
                }

                // 如果移动时间超过1秒，则将 `moving` 设为 `true`，并打印提示信息
                if (Time.time - startTime > 1f)
                {
                    moving = true;

                }
            }
            else if (!CharacterActor.IsStable)
            {
                moving = false;
                startTime = 0f; // 如果停止移动，则将开始时间重置为0
            }
        }
        private bool movementInputIdle;
        private float movementInputIdleTime;
        /// <summary>
        /// 检测是否在0.1s内没按下
        /// </summary>
        /// <returns></returns>
        private bool CheckMovementInputIdle()
        {
            if (Input.GetButton("Movement X") || Input.GetButton("Movement Y"))
            {
                movementInputIdle = false;
                movementInputIdleTime = 0f;
            }
            else
            {
                movementInputIdleTime += Time.deltaTime;
                if (movementInputIdleTime > 0.1f)
                {
                    movementInputIdle = true;
                }
            }
            return movementInputIdle;
        }
        private void PlayStop()
        {
            if (CharacterActions.movement.value.sqrMagnitude != 0)
            {
                CharacterActor.Animator.SetBool("inputMove", true);
            }
            MoveTime();
            // 获取当前速度大小
            float currentVelocityMagnitude = CharacterActor.PlanarVelocity.magnitude;

            // 如果速度大小小于等于0，则表示已停止移动
            //把这个等于0改成都没有按下

            if (CheckMovementInputIdle() & moving & CharacterActor.IsStable)
            {
                moving = false;
                CharacterActor.Animator.SetBool("inputMove", false);
                // 如果上一帧速度大小大于10，则播放停止动画
                if (lastVelocityMagnitude > 0.8f * planarMovementParameters.boostSpeedLimit)
                {
                    CharacterActor.Animator.SetFloat("running", 1);
                    CharacterActor.Animator.SetBool("stop", true);
                }
                else if (lastVelocityMagnitude > 0.6f * planarMovementParameters.boostSpeedLimit)
                {
                    CharacterActor.Animator.SetFloat("running", 0);
                    CharacterActor.Animator.SetBool("stop", true);
                }
                else
                {

                }
                // 更新状态
                //isMoving = false;
            }
            //else
            //{
            //    // 更新状态
            //    isMoving = true;
            //}

            // 保存当前速度大小，用于下一帧判断
            lastVelocityMagnitude = currentVelocityMagnitude;
        }

        protected virtual void ProcessGravity(float dt)
        {
            if (!verticalMovementParameters.useGravity)
                return;


            verticalMovementParameters.UpdateParameters();


            float gravityMultiplier = 1f;

            if (materialController != null)
                gravityMultiplier = CharacterActor.LocalVelocity.y >= 0 ?
                    materialController.CurrentVolume.gravityAscendingMultiplier :
                    materialController.CurrentVolume.gravityDescendingMultiplier;

            float gravity = gravityMultiplier * verticalMovementParameters.gravity;


            if (!CharacterActor.IsStable)
                CharacterActor.VerticalVelocity += CustomUtilities.Multiply(-CharacterActor.Up, gravity, dt);


        }


        protected bool UnstableGroundedJumpAvailable => !verticalMovementParameters.canJumpOnUnstableGround && CharacterActor.CurrentState == CharacterActorState.UnstableGrounded;



        public enum JumpResult
        {
            Invalid,
            Grounded,
            NotGrounded
        }

        JumpResult CanJump()
        {
            JumpResult jumpResult = JumpResult.Invalid;

            if (!verticalMovementParameters.canJump)
                return jumpResult;

            if (isCrouched)
                return jumpResult;


            switch (CharacterActor.CurrentState)
            {
                case CharacterActorState.StableGrounded:

                    if (CharacterActions.jump.StartedElapsedTime <= verticalMovementParameters.preGroundedJumpTime && groundedJumpAvailable)
                        jumpResult = JumpResult.Grounded;

                    break;
                case CharacterActorState.NotGrounded:

                    if (CharacterActions.jump.Started)
                    {
                        // First check if the "grounded jump" is available. If so, execute a "coyote jump".
                        if (CharacterActor.NotGroundedTime <= verticalMovementParameters.postGroundedJumpTime && groundedJumpAvailable)
                        {
                            jumpResult = JumpResult.Grounded;
                        }
                        else if (notGroundedJumpsLeft != 0)  // Do a not grounded jump
                        {
                            jumpResult = JumpResult.NotGrounded;
                        }
                    }

                    break;
                case CharacterActorState.UnstableGrounded:

                    if (CharacterActions.jump.StartedElapsedTime <= verticalMovementParameters.preGroundedJumpTime && verticalMovementParameters.canJumpOnUnstableGround)
                        jumpResult = JumpResult.Grounded;

                    break;
            }

            return jumpResult;
        }



        protected virtual void ProcessJump(float dt)
        {
            ProcessRegularJump(dt);
            ProcessJumpDown(dt);
        }

        #region JumpDown

        protected virtual bool ProcessJumpDown(float dt)
        {

            if (!verticalMovementParameters.canJumpDown)
                return false;

            if (!CharacterActor.IsStable)
                return false;

            if (!CharacterActor.IsGroundAOneWayPlatform)
                return false;

            if (verticalMovementParameters.filterByTag)
            {
                if (!CharacterActor.GroundObject.CompareTag(verticalMovementParameters.jumpDownTag))
                    return false;
            }

            if (!ProcessJumpDownAction())
                return false;
            JumpDown(dt);
            return true;
        }


        protected virtual bool ProcessJumpDownAction()
        {
            return isCrouched && CharacterActions.jump.Started;
        }


        protected virtual void JumpDown(float dt)
        {

            float groundDisplacementExtraDistance = 0f;

            Vector3 groundDisplacement = CustomUtilities.Multiply(CharacterActor.GroundVelocity, dt);
            // bool  CharacterActor.transform.InverseTransformVectorUnscaled( Vector3.Project( groundDisplacement , CharacterActor.Up ) ).y

            if (!CharacterActor.IsGroundAscending)
                groundDisplacementExtraDistance = groundDisplacement.magnitude;

            CharacterActor.ForceNotGrounded();

            CharacterActor.Position -=
                CustomUtilities.Multiply(
                    CharacterActor.Up,
                    CharacterConstants.ColliderMinBottomOffset + verticalMovementParameters.jumpDownDistance + groundDisplacementExtraDistance
                );

            CharacterActor.VerticalVelocity -= CustomUtilities.Multiply(CharacterActor.Up, verticalMovementParameters.jumpDownVerticalVelocity);
        }

        #endregion

        #region Jump

        protected virtual void ProcessRegularJump(float dt)
        {
            if (CharacterActor.IsGrounded)
            {
                notGroundedJumpsLeft = verticalMovementParameters.availableNotGroundedJumps;

                groundedJumpAvailable = true;
            }


            if (isAllowedToCancelJump)
            {
                if (verticalMovementParameters.cancelJumpOnRelease)
                {
                    if (CharacterActions.jump.StartedElapsedTime >= verticalMovementParameters.cancelJumpMaxTime || CharacterActor.IsFalling)
                    {
                        isAllowedToCancelJump = false;
                    }
                    else if (!CharacterActions.jump.value && CharacterActions.jump.StartedElapsedTime >= verticalMovementParameters.cancelJumpMinTime)
                    {
                        // Get the velocity mapped onto the current jump direction
                        Vector3 projectedJumpVelocity = Vector3.Project(CharacterActor.Velocity, jumpDirection);

                        CharacterActor.Velocity -= CustomUtilities.Multiply(projectedJumpVelocity, 1f - verticalMovementParameters.cancelJumpMultiplier);

                        isAllowedToCancelJump = false;
                    }
                }
            }
            else
            {
                JumpResult jumpResult = CanJump();

                switch (jumpResult)
                {
                    case JumpResult.Grounded:
                        groundedJumpAvailable = false;

                        break;
                    case JumpResult.NotGrounded:
                        notGroundedJumpsLeft--;

                        break;

                    case JumpResult.Invalid:
                        return;
                }

                // Events ---------------------------------------------------
                if (CharacterActor.IsGrounded)
                {

                    if (OnGroundedJumpPerformed != null)
                        OnGroundedJumpPerformed(true);
                }
                else
                {
                    if (OnNotGroundedJumpPerformed != null)
                        OnNotGroundedJumpPerformed(notGroundedJumpsLeft);
                }

                if (OnJumpPerformed != null)
                    OnJumpPerformed();

                if (IsPlayer)
                {
                    CharacterActor.Animator.SetBool("jump", true);
                }

                // Define the jump direction ---------------------------------------------------
                jumpDirection = SetJumpDirection();

                // Force "not grounded" state.     
                if (CharacterActor.IsGrounded)
                    CharacterActor.ForceNotGrounded();

                // First remove any velocity associated with the jump direction.
                CharacterActor.Velocity -= Vector3.Project(CharacterActor.Velocity, jumpDirection);
                CharacterActor.Velocity += CustomUtilities.Multiply(jumpDirection, verticalMovementParameters.jumpSpeed);

                if (verticalMovementParameters.cancelJumpOnRelease)
                    isAllowedToCancelJump = true;

            }


        }



        /// <summary>
        /// Returns the jump direction vector whenever the jump action is started.
        /// </summary>
        protected virtual Vector3 SetJumpDirection()
        {
            return CharacterActor.Up;
        }

        #endregion


        void ProcessVerticalMovement(float dt)
        {
            ProcessGravity(dt);
            ProcessJump(dt);
        }


        public override void EnterBehaviour(float dt, CharacterState fromState)
        {
            for (int i = 0; i < weaponManager.Length; i++)
            {
                weaponManager[i].gameObject.SetActive(false);
            }
            Debug.Log(CharacterActor.CharacterCollisionInfo);

            StartCoroutine(CheckAnim());

            CharacterActor.alwaysNotGrounded = false;

            targetLookingDirection = CharacterActor.Forward;

            if (fromState == CharacterStateController.GetState<WallSlide>())
            {
                // "availableNotGroundedJumps + 1" because the update code will consume one jump!
                notGroundedJumpsLeft = verticalMovementParameters.availableNotGroundedJumps + 1;

                // Reduce the amount of air control (acceleration and deceleration) for 0.5 seconds.
                ReduceAirControl(0.5f);
            }

            currentPlanarSpeedLimit = Mathf.Max(CharacterActor.PlanarVelocity.magnitude, planarMovementParameters.baseSpeedLimit);

            CharacterActor.UseRootMotion = false;
        }

        private IEnumerator CheckAnim()
        {
            yield return null;
            bool isPlayCurrentAnim = CharacterActor.Animator.GetNextAnimatorStateInfo(0).IsTag("NormalMovement");
            if (CharacterStateController.CurrentState.GetType() == this.GetType() && (!isPlayCurrentAnim))
            {
                if (CharacterActor.IsGrounded)
                {
                    CharacterActor.Animator.CrossFade("NormalMovement.StableGrounded", 0.3f);
                }
                else
                {
                    //如果竖直方向上在前进
                    if (CharacterActions.jump.value == true)
                    {
                        CharacterActor.Animator.CrossFade("NormalMovement.Lucy_Jump_Start_Inplace", 0.05f, 0, 0.2f);
                    }
                    else if (CharacterActor.PredictedGroundDistance > 0.3f)
                    {
                        CharacterActor.Animator.CrossFade("NormalMovement.Lucy_Jump_Loop_Inplace", 0.3f);
                    }
                    else
                    {
                        CharacterActor.Animator.CrossFade("NormalMovement.Lucy_Jump_End_Inplace", 0.05f);
                    }
                }
            }
        }

        protected virtual void HandleRotation(float dt)
        {
            HandleLookingDirection(dt);
        }

        void HandleLookingDirection(float dt)
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

            Quaternion targetDeltaRotation = Quaternion.FromToRotation(CharacterActor.Forward, targetLookingDirection);
            Quaternion currentDeltaRotation = Quaternion.Slerp(Quaternion.identity, targetDeltaRotation, lookingDirectionParameters.speed * dt);

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

        public override void UpdateBehaviour(float dt)
        {
            HandleSize(dt);
            HandleVelocity(dt);
            HandleRotation(dt);
        }

        public override void PostUpdateBehaviour(float dt)
        {
            base.PostUpdateBehaviour(dt);
            if (IsPlayer)
            {
                LaterGrap();
            }
        }

        [Header("References")]
        public Transform cam;
        public Transform gunTip;
        public LayerMask whatIsGrapplable;

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

        private void Grap()
        {
            if (Input.GetKeyDown(grappleKey))
            {
                StartGrapple();
            }
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
            Vector3 vecs = CalVocality(CharacterActor.Position, grapplePoint, UnityEngine.Random.Range(0f, 2f), 19.8f);
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
            grivaty = Physics.gravity.magnitude;
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

        public override void PreCharacterSimulation(float dt)
        {
            base.PreCharacterSimulation(dt);
            // Pre/PostCharacterSimulation methods are useful to update all the Animator parameters. 
            // Why? Because the CharacterActor component will end up modifying the velocity of the actor.
           
        }

        public override void PostCharacterSimulation(float dt)
        {
            // Pre/PostCharacterSimulation methods are useful to update all the Animator parameters. 
            // Why? Because the CharacterActor component will end up modifying the velocity of the actor.
            if (!CharacterActor.IsAnimatorValid())
                return;

            // Parameters associated with velocity are sent after the simulation.
            // The PostSimulationUpdate (CharacterActor) might update velocity once more (e.g. if a "bad step" has been detected).
            CharacterStateController.Animator.SetFloat(verticalSpeedParameter, CharacterActor.LocalVelocity.y);
            CharacterStateController.Animator.SetFloat(planarSpeedParameter, CharacterActor.PlanarVelocity.magnitude);
        }

        protected virtual void HandleSize(float dt)
        {
            // Get the crouch input state 
            if (crouchParameters.enableCrouch)
            {
                if (crouchParameters.inputMode == InputMode.Toggle)
                {
                    if (CharacterActions.crouch.Started)
                        wantToCrouch = !wantToCrouch;
                }
                else
                {
                    wantToCrouch = CharacterActions.crouch.value;
                }

                if (!crouchParameters.notGroundedCrouch && !CharacterActor.IsGrounded)
                    wantToCrouch = false;

                if (CharacterActor.IsGrounded && wantToRun)
                    wantToCrouch = false;
            }
            else
            {
                wantToCrouch = false;
            }

            if (wantToCrouch)
                Crouch(dt);
            else
                StandUp(dt);
        }

        void Crouch(float dt)
        {
            CharacterActor.SizeReferenceType sizeReferenceType = CharacterActor.IsGrounded ?
                CharacterActor.SizeReferenceType.Bottom : crouchParameters.notGroundedReference;

            bool validSize = CharacterActor.CheckAndInterpolateHeight(
                CharacterActor.DefaultBodySize.y * crouchParameters.heightRatio,
                crouchParameters.sizeLerpSpeed * dt, sizeReferenceType);

            if (validSize)
                isCrouched = true;
        }

        void StandUp(float dt)
        {
            CharacterActor.SizeReferenceType sizeReferenceType = CharacterActor.IsGrounded ?
                CharacterActor.SizeReferenceType.Bottom : crouchParameters.notGroundedReference;

            bool validSize = CharacterActor.CheckAndInterpolateHeight(
                CharacterActor.DefaultBodySize.y,
                crouchParameters.sizeLerpSpeed * dt, sizeReferenceType);

            if (validSize)
                isCrouched = false;
        }

        //玩家的输入和输出都要放在update中检测，不能放在fixupdate中

        protected virtual void HandleVelocity(float dt)
        {
            ProcessVerticalMovement(dt);
            ProcessPlanarMovement(dt);
        }

        private float buttonDownTime;
        public bool preEvade;
        public Vector2 evadeVec2;

        private void Update()
        {
            CanEvade();
        }

        public bool CanEvade()
        {
            if (CharacterStateController.CurrentState is Evade)
            {
                return false;
            }
            if (preEvade)
            {
                return true;
            }
            if (Input.GetButtonDown("Run"))
            {
                // 虚拟按键Run被按下，记录按下时间
                buttonDownTime = Time.time;
                evadeVec2 = CharacterActions.movement.value;

            }
            else if (Input.GetButtonUp("Run"))
            {
                // 虚拟按键Run被抬起，检查按下时间是否小于0.1秒
                if (Time.time - buttonDownTime < 0.35f)
                {
                    // 按下时间小于0.1秒，返回true
                    preEvade = true;
                }
            }

            // 返回false
            return false;
        }
    }
}






