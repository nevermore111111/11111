using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using static Lightbug.CharacterControllerPro.Implementation.MovementReferenceParameters;

namespace Rusk
{
    /// <summary>
    /// 这个类现在需要一个待机状态，播完闪避切换到待机状态，要不太僵硬了并且不好掌控播放这个闪避的时间。直接切换到
    /// <summary>
    ///
    public class Evade : CharacterState
    {

        [Min(0f)]
        [SerializeField]
        protected float initialVelocity = 12f;

        [Min(0f)]
        [SerializeField]
        protected float duration = 1.0f;

        private float UnChangedDuration;

        [SerializeField]
        protected AnimationCurve movementCurve = AnimationCurve.Linear(0, 1, 1, 0);

        [Min(0f)]
        [SerializeField]
        protected int availableNotGroundedDashes = 1;

        [SerializeField]
        protected bool ignoreSpeedMultipliers = false;

        [SerializeField]
        protected bool forceNotGrounded = true;

        [Tooltip("Whether or not to allow the dash to be canceled by others rigidbodies.")]
        [SerializeField]
        protected bool cancelOnContact = true;

        [Tooltip("If the contact point velocity (magnitude) is greater than this value, the Dash will be instantly canceled.")]
        [SerializeField]
        protected float contactVelocityTolerance = 5f;


        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────


        protected MaterialController materialController = null;

        protected int airDashesLeft;
        protected float dashCursor = 0;

        protected Vector3 dashDirection = Vector2.right;

        protected bool isDone = true;

        protected float currentSpeedMultiplier = 1f;

        protected NormalMovement NormalMovement;
        private Attack attack;


        #region Events

        /// <summary>
        /// This event is called when the dash state is entered.
        /// 
        /// The direction of the dash is passed as an argument.
        /// </summary>
        public event System.Action<Vector3> OnDashStart;

        /// <summary>
        /// This event is called when the dash action has ended.
        /// 
        /// The direction of the dash is passed as an argument.
        /// </summary>
        public event System.Action<Vector3> OnDashEnd;

        #endregion

        void OnEnable()
        {
            CharacterActor.OnGroundedStateEnter += OnGroundedStateEnter;

        }

        void OnDisable()
        {
            CharacterActor.OnGroundedStateEnter -= OnGroundedStateEnter;
        }


        public override string GetInfo()
        {
            return "This state is entirely based on particular movement, the \"dash\". This movement is normally a fast impulse along " +
            "the forward direction. In this case the movement can be defined by using an animation curve (time vs velocity)";
        }

        void OnGroundedStateEnter(Vector3 localVelocity) => airDashesLeft = availableNotGroundedDashes;

        bool CheckContacts()
        {
            if (!cancelOnContact)
                return false;

            for (int i = 0; i < CharacterActor.Contacts.Count; i++)
            {
                Contact contact = CharacterActor.Contacts[i];

                if (contact.pointVelocity.magnitude > contactVelocityTolerance)
                    return true;
            }

            return false;
        }

        protected override void Awake()
        {
            base.Awake();
            attack = this.GetComponent<Attack>();
            materialController = this.GetComponentInBranch<CharacterActor, MaterialController>();
            airDashesLeft = availableNotGroundedDashes;
            NormalMovement = GetComponent<NormalMovement>();
            characterActor = this.transform.parent.GetComponentInBranch<CharacterActor>();
            body = characterActor.transform;
            UnChangedDuration = duration;
        }


        public override bool CheckEnterTransition(CharacterState fromState)
        {
            if (!CharacterActor.IsGrounded && airDashesLeft <= 0)
                return false;

            return true;
        }

        public override void CheckExitTransition()
        {
            if (isDone)
            {
                if (OnDashEnd != null)
                    OnDashEnd(dashDirection);
               // characterActor.Animator.SetTrigger("");
                CharacterStateController.EnqueueTransition<NormalMovement>();
            }
            if(characterActor.IsGrounded)
            {
                if (dashCursor >= 0.85f && CharacterActions.attack.value == true)
                {
                    if (attack.currentAttackMode == Attack.AttackMode.AttackOnGround)
                    {
                        CharacterStateController.EnqueueTransition<AttackOnGround>(); ResetDash();
                    }
                    
                    if(attack.currentAttackMode == Attack.AttackMode.AttackOnGround_fist)
                    {
                        CharacterStateController.EnqueueTransition<AttackOnGround_fist>(); ResetDash();
                    }
                }
            }

            
            if(CharacterActions.jump.value == true)
            {
                CharacterStateController.EnqueueTransition<NormalMovement>();
            }
        }


        public override void EnterBehaviour(float dt, CharacterState fromState)
        {
            duration = UnChangedDuration;
            NormalMovement.preEvade = false;
            if (forceNotGrounded)
                CharacterActor.alwaysNotGrounded = true;
            characterActor.Animator.Play("Evade");
            CharacterActor.UseRootMotion = false;

            if (CharacterActor.IsGrounded)
            {

                if (!ignoreSpeedMultipliers)
                {
                    currentSpeedMultiplier = materialController != null ? materialController.CurrentSurface.speedMultiplier * materialController.CurrentVolume.speedMultiplier : 1f;
                }

            }
            else
            {

                if (!ignoreSpeedMultipliers)
                {
                    currentSpeedMultiplier = materialController != null ? materialController.CurrentVolume.speedMultiplier : 1f;
                }

                airDashesLeft--;


            }
            UpdateData(NormalMovement.evadeVec2);


            //这里修改冲刺的方向
            //设置冲刺动画的参数
            if (NormalMovement.evadeVec2 == Vector2.zero)
            {
                dashDirection = - CharacterActor.Forward;
            }
            else
            {
                dashDirection = InputMovementReference;
            }


            //需要根据闪避的方向去增加evade 的时间 

            //我需要 1根据当前闪避的方向和人物后方向归一化的的点乘 的 数值去计算.直接使用input。y就行了

            SetAnimatorPar();

            ResetDash();

            //Execute the event
            if (OnDashStart != null)
                OnDashStart(dashDirection);

        }

        private void SetAnimatorPar()
        {
            Vector2 input = NormalMovement.evadeVec2;
            
            
            {
                input.Normalize();
            }
            Vector3 camera = new Vector3(input.x, 0, input.y);
            Vector3 mid = externalReference.TransformDirection(camera);
            mid = body.InverseTransformDirection(mid);

            input = new Vector2(mid.x, mid.z);
            
            input = input.normalized;

            if (input == Vector2.zero)
            {
                characterActor.Animator.SetFloat("xInput", 0);
                characterActor.Animator.SetFloat("yInput", -1);
            }
            else
            {
                characterActor.Animator.SetFloat("xInput", input.x);
                characterActor.Animator.SetFloat("yInput", input.y);
            }
            
            //就在这里修改 闪避的时间 
            float addTime = 0f;
            if(Mathf.Abs(input.y)>0.3)
            {
                if(input.y>0)
                {
                    addTime = 0.1f*input.y;
                }
                else
                {
                    addTime = 0.3f * Mathf.Abs(input.y);
                }
            }

            duration += addTime;

        }

        public override void ExitBehaviour(float dt, CharacterState toState)
        {
            if (forceNotGrounded)
                CharacterActor.alwaysNotGrounded = false;
        }


        public override void UpdateBehaviour(float dt)
        {
            Vector3 dashVelocity = initialVelocity * currentSpeedMultiplier * movementCurve.Evaluate(dashCursor) * dashDirection;

            CharacterActor.Velocity = dashVelocity;

            float animationDt = dt / duration;
            dashCursor += animationDt;

            if (dashCursor >= 1)
            {
                isDone = true;
                dashCursor = 0;
            }

        }

        public override void PostUpdateBehaviour(float dt)
        {
            isDone |= CheckContacts();
        }

        public virtual void ResetDash()
        {

            CharacterActor.Velocity = Vector3.zero;
            isDone = false;
            dashCursor = 0;
        }



        /// <summary>
        /// Gets a vector that is the product of the input axes (taken from the character actions) and the movement reference. 
        /// The magnitude of this vector is always less than or equal to 1.
        /// </summary>
        public Vector3 InputMovementReference { get; private set; }

        /// <summary>
        /// Forward vector used by the movement reference.
        /// </summary>
        public Vector3 MovementReferenceForward { get; private set; }


        /// <summary>
        /// Right vector used by the movement reference.
        /// </summary>
        public Vector3 MovementReferenceRight { get; private set; }

        public Transform externalReference;

        private CharacterActor characterActor;

        Transform body;

        public void UpdateData(Vector2 movementInput)
        {
            UpdateMovementReferenceData();
            //if (movementInput == Vector2.zero)
            //{
            //    movementInput = new Vector2(0, -1);
            //}

            {

                Vector3 inputMovementReference = CustomUtilities.Multiply(MovementReferenceRight, movementInput.x) +
                    CustomUtilities.Multiply(MovementReferenceForward, movementInput.y);

                InputMovementReference = Vector3.ClampMagnitude(inputMovementReference, 1f);
            }

            // Debug ---------------------------------------------
            // Debug.DrawRay( characterActor.Position , MovementReferenceForward * 2f , Color.blue );
            // Debug.DrawRay( characterActor.Position , MovementReferenceRight * 2f , Color.red );
        }

        void UpdateMovementReferenceData()
        {
            // Forward




            if (externalReference != null)
            {
                // MovementReferenceForward = CustomUtilities.ProjectOnTangent( externalReference.forward , characterActor.GroundStableNormal , characterActor.Up );
                // MovementReferenceRight = CustomUtilities.ProjectOnTangent( externalReference.right , characterActor.GroundStableNormal , characterActor.Up );
                MovementReferenceForward = Vector3.Normalize(Vector3.ProjectOnPlane(externalReference.forward, characterActor.Up));
                MovementReferenceRight = Vector3.Normalize(Vector3.ProjectOnPlane(externalReference.right, characterActor.Up));
            }
            else
                Debug.Log("CharacterStateController: the external reference is null! assign a Transform.");



        }
    }
}