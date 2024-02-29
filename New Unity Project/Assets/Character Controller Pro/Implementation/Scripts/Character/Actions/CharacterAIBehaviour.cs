using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.Utilities;
using Lightbug.CharacterControllerPro.Demo;

namespace Lightbug.CharacterControllerPro.Implementation
{

    public abstract class CharacterAIBehaviour : MonoBehaviour
    {
        public CharacterActions characterActions = new CharacterActions();


        public virtual void EnterBehaviour(float dt) { }
        public abstract void UpdateBehaviour(float dt);
        public virtual void ExitBehaviour(float dt) { }

        public CharacterActor CharacterActor { get; private set; }


        protected virtual void Awake()
        {
            CharacterActor = this.GetComponentInBranch<CharacterActor>();
        }
        protected virtual void Start()
        {
            normalMovement = CharacterActor.stateController.GetState<NormalMovement>() as NormalMovement;
        }

        protected void SetMovementAction(Vector3 direction)
        {
            Vector3 inputXZ = Vector3.ProjectOnPlane(direction, CharacterActor.Up);
            inputXZ.Normalize();
            inputXZ.y = inputXZ.z;
            inputXZ.z = 0f;
            characterActions.movement.value = inputXZ;
        }
        protected void SetDefendAction(bool defendValue)
        {
            characterActions.defend.value = defendValue;
        }
        NormalMovement normalMovement;
        protected void SetEvade(Vector3 direction)
        {
            SetMovementAction(direction);
            characterActions.evade.value = true;
            if (normalMovement != null)
            {
                normalMovement.evadeVec2 = characterActions.movement.value;
            }
            else
            {
                Debug.LogError("没有移动状态");
            }
        }
    }

}