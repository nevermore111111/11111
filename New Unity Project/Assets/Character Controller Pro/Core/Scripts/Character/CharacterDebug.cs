using UnityEngine;
using UnityEngine.UI;
using Lightbug.Utilities;

namespace Lightbug.CharacterControllerPro.Core
{
    /// <summary>
    /// This class is used for debug purposes, mainly to print information on screen about the collision flags, certain values and/or triggering events.
    /// </summary>
    [DefaultExecutionOrder(ExecutionOrder.CharacterActorOrder)]
    [AddComponentMenu("Character Controller Pro/Core/Character Debug")]
    public class CharacterDebug : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        CharacterActor characterActor = null;

        [Header("GUI")]
        [SerializeField]
        bool debugCollisionFlags = true;

        [Condition("debugCollisionFlags", ConditionAttribute.ConditionType.IsTrue, ConditionAttribute.VisibilityType.NotEditable)]
        [SerializeField]
        Text text = null;

        [Header("Events")]
        [SerializeField]
        bool printEvents = false;

        [Header("Stability")]

        [SerializeField]
        Renderer stabilityIndicator;

        [Condition("stabilityIndicator", ConditionAttribute.ConditionType.IsNotNull, ConditionAttribute.VisibilityType.NotEditable)]
        [SerializeField]
        Color stableColor = new Color(0f, 1f, 0f, 0.5f);

        [Condition("stabilityIndicator", ConditionAttribute.ConditionType.IsNotNull, ConditionAttribute.VisibilityType.NotEditable)]
        [SerializeField]
        Color unstableColor = new Color(1f, 0f, 0f, 0.5f);


        float time = 0f;

        void Awake()
        {
            if (characterActor == null)
            {
                this.enabled = false;
                return;
            }

            if (debugCollisionFlags)
            {
                if (text == null)
                    this.enabled = false;
            }

        }

        void FixedUpdate()
        {
            if (characterActor == null)
            {
                enabled = false;
                return;
            }

            if (debugCollisionFlags)
            {
                if (time > 0.2f)
                {
                    text.text = characterActor.ToString();
                    time = 0f;
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }

        void Update()
        {
            if (stabilityIndicator != null)
            {
                stabilityIndicator.material.SetColor("_Color", characterActor.IsStable ? stableColor : unstableColor);
            }
        }

        void OnEnable()
        {
            if (!printEvents)
                return;

            characterActor.OnWallHit += OnWallHit;
            characterActor.OnGroundedStateEnter += OnGroundedStateEnter;
            characterActor.OnGroundedStateExit += OnGroundedStateExit;
            characterActor.OnHeadHit += OnHeadHit;
            characterActor.OnTeleport += OnTeleportation;
        }

        void OnDisable()
        {
            if (!printEvents)
                return;

            characterActor.OnWallHit -= OnWallHit;
            characterActor.OnGroundedStateEnter -= OnGroundedStateEnter;
            characterActor.OnGroundedStateExit -= OnGroundedStateExit;
            characterActor.OnHeadHit -= OnHeadHit;
            characterActor.OnTeleport -= OnTeleportation;
        }

        void OnWallHit(Contact contact)
        {
            Debug.Log("OnWallHit");
        }

        void OnGroundedStateEnter(Vector3 localVelocity)
        {
            Debug.Log("OnEnterGroundedState, localVelocity : " + localVelocity.ToString("F3"));
        }

        void OnGroundedStateExit()
        {
            Debug.Log("OnExitGroundedState");
        }

        void OnHeadHit(Contact contact)
        {
            Debug.Log("OnHeadHit");
        }

        void OnTeleportation(Vector3 position, Quaternion rotation)
        {
            Debug.Log("OnTeleportation, position : " + position.ToString("F3") + " and rotation : " + rotation.ToString("F3"));
        }



    }

}
