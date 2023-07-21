using UnityEngine;
using System.Collections.Generic;

namespace Lightbug.CharacterControllerPro.Implementation
{

    /// <summary>
    /// This input handler implements the input detection following the Unity's Input Manager convention. This scheme is used for desktop games.
    /// </summary>
    public class UnityInputHandler : InputHandler
    {
        private Dictionary<string, float> pressTimes = new Dictionary<string, float>();
        struct Vector2Action
        {
            public string x;
            public string y;

            public Vector2Action(string x, string y)
            {
                this.x = x;
                this.y = y;
            }
        }

        Dictionary<string, Vector2Action> vector2Actions = new Dictionary<string, Vector2Action>();

        public override bool GetBool(string actionName)
        {
            bool output = false;
            try
            {
                output = Input.GetButton(actionName);
            }
            catch (System.Exception)
            {
                PrintInputWarning(actionName);
            }

            return output;
        }



        

        public override float GetFloat(string actionName)
        {
            float output = default(float);
            try
            {
                output = Input.GetAxis(actionName);
            }
            catch (System.Exception)
            {
                PrintInputWarning(actionName);
            }

            return output;
        }

        public override Vector2 GetVector2(string actionName)
        {
            // Not officially supported	
            // Example : "Movement"  splits into "Movement X" and "Movement Y"

            Vector2Action vector2Action;

            bool found = vector2Actions.TryGetValue(actionName, out vector2Action);

            if (!found)
            {
                vector2Action = new Vector2Action(
                    string.Concat(actionName, " X"),
                    string.Concat(actionName, " Y")
                );

                vector2Actions.Add(actionName, vector2Action);
            }

            Vector2 output = default(Vector2);

            try
            {
                output = new Vector2(Input.GetAxis(vector2Action.x), Input.GetAxis(vector2Action.y));
            }
            catch (System.Exception)
            {
                PrintInputWarning(vector2Action.x, vector2Action.y);
            }

            return output;
        }

        void PrintInputWarning(string actionName)
        {
            Debug.LogWarning($"{actionName} action not found! Please make sure this action is included in your input settings (axis). If you're only testing the demo scenes from " +
            "Character Controller Pro please load the input preset included at \"Character Controller Pro/OPEN ME/Presets/.");
        }

        void PrintInputWarning(string actionXName, string actionYName)
        {
            Debug.LogWarning($"{actionXName} and/or {actionYName} actions not found! Please make sure both of these actions are included in your input settings (axis). If you're only testing the demo scenes from " +
            "Character Controller Pro please load the input preset included at \"Character Controller Pro/OPEN ME/Presets/.");
        }

        public override bool IsShortPress(string actionName)
        {
            float pressTime = GetPressTime(actionName);
            return pressTime <= shortPressTime;
        }

        public override bool IsLongPress(string actionName)
        {
            float pressTime = GetPressTime(actionName);
            return pressTime >= longPressTime;
        }

        private float GetPressTime(string actionName)
        {
            float pressTime = 0f;
            if (Input.GetButton(actionName))
            {
                if (pressTimes.ContainsKey(actionName))
                {
                    pressTime = pressTimes[actionName] + Time.deltaTime;
                }
                else
                {
                    pressTime = Time.deltaTime;
                }
                pressTimes[actionName] = pressTime;
            }
            else
            {
                pressTimes.Remove(actionName);
            }
            return pressTime;
        }

        public override void Update()
        {
            // 在 Update 方法中更新按键状态
            foreach (var actionName in pressTimes.Keys)
            {
                if (Input.GetButtonUp(actionName))
                {
                    // 在松开按键时，根据按下时间触发对应的事件
                    float pressTime = pressTimes[actionName];
                    if (pressTime <= shortPressTime)
                    {
                        // 短按事件
                        OnShortPress(actionName);
                    }
                    else
                    {
                        // 长按事件
                        OnLongPress(actionName);
                    }
                    pressTimes.Remove(actionName);
                }
            }
        }
    }

}
