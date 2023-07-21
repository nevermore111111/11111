﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Lightbug.CharacterControllerPro.Implementation
{

    /// <summary>
    /// Defines the nature of the inputs obtained by the associated input handler.
    /// </summary>
    public enum HumanInputType
    {
        InputManager,
        UIMobile,
        Custom
    }


    /// <summary>
    /// This abstract class contains all the input methods that are used by the character brain. This is the base class for all the input detection methods available.
    /// </summary>
    public abstract class InputHandler : MonoBehaviour
    {

        public static float shortPressTime = 0.2f;
        public static float longPressTime = 0.2f;
        public static InputHandler CreateInputHandler(GameObject gameObject, HumanInputType inputType)
        {
            InputHandler inputHandler = null;

            switch (inputType)
            {
                case HumanInputType.InputManager:

                    inputHandler = gameObject.AddComponent<UnityInputHandler>();
                    inputHandler.hideFlags = HideFlags.HideInInspector;

                    break;
                case HumanInputType.UIMobile:

                    inputHandler = gameObject.AddComponent<UIInputHandler>();
                    inputHandler.hideFlags = HideFlags.HideInInspector;

                    break;
            }

            return inputHandler;
        }

        //public abstract void Update();
        public abstract bool GetBool(string actionName);
        public abstract float GetFloat(string actionName);
        public abstract Vector2 GetVector2(string actionName);
        //public abstract bool IsShortPress(string actionName);
        //public abstract bool IsLongPress(string actionName);

    }

}
