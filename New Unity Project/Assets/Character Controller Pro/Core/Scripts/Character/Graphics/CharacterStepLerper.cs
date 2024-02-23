using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lightbug.Utilities;

namespace Lightbug.CharacterControllerPro.Core
{
    /// <summary>
    /// 该组件负责根据角色移动（CharacterActor）平滑处理与图形相关的元素（在根对象下）。
    /// 它允许您相应地修改位置和旋转，产生出色的最终结果。
    /// </summary>
    [AddComponentMenu("Character Controller Pro/Core/Character Graphics/Step Lerper")]
    [DefaultExecutionOrder(ExecutionOrder.CharacterGraphicsOrder)]
    public class CharacterStepLerper : CharacterGraphics
    {
        // 步伐上升插值的速度。
        [Tooltip("步伐上升插值的速度。")]
        [SerializeField]
        float positiveDisplacementSpeed = 20f;

        // 步伐下降插值的速度。
        [Tooltip("步伐下降插值的速度。")]
        [SerializeField]
        float negativeDisplacementSpeed = 40f;

        // 一个角色一直被插值不是理想的情况，特别是在行走在斜坡上，不接地，或者可能使用移动平台时。
        // 对于这些情况，应允许角色随着时间平稳地回到其原始本地位置。该字段表示此过程的持续时间（以秒为单位）。
        [Tooltip("一个角色一直被插值不是理想的情况，特别是在行走在斜坡上，不接地，或者可能使用移动平台时。" +
        "对于这些情况，应允许角色随着时间平稳地回到其原始本地位置。该字段表示此过程的持续时间（以秒为单位）。")]
        [SerializeField]
        float recoveryDuration = 1f;

        // 用于恢复过程的最大速度。
        [Tooltip("用于恢复过程的最大速度。")]
        [SerializeField]
        float maxRecoverySpeed = 200f;

        Vector3 previousPosition = default; // 角色的前一个位置
        bool teleportFlag = false; // 标志位，指示角色是否传送
        float recoveryTimer = 0f; // 恢复过程的计时器

        protected override void OnValidate()
        {
            base.OnValidate();

            // 确保 positiveDisplacementSpeed 和 negativeDisplacementSpeed 是正值
            CustomUtilities.SetPositive(ref positiveDisplacementSpeed);
            CustomUtilities.SetPositive(ref negativeDisplacementSpeed);
        }

        void Start()
        {
            // 将 previousPosition 初始化为角色的当前位置
            previousPosition = transform.position;
        }

        void OnEnable() => CharacterActor.OnTeleport += OnTeleport;
        void OnDisable() => CharacterActor.OnTeleport -= OnTeleport;

        void Update()
        {
            // 如果 CharacterActor 为空，则禁用组件
            if (CharacterActor == null)
            {
                enabled = false;
                return;
            }

            // 获取自上一帧以来的时间
            float dt = Time.deltaTime;

            // 处理角色的垂直位移
            HandleVerticalDisplacement(dt);

            // 如果传送标志已设置，则重置传送标志
            if (teleportFlag)
                teleportFlag = false;
        }

        void OnTeleport(Vector3 position, Quaternion rotation)
        {
            // 当角色传送时，将传送标志设置为 true
            teleportFlag = true;
        }

        void HandleVerticalDisplacement(float dt)
        {
            // 如果角色传送，则更新 previousPosition 并将 transform 位置设置为 CharacterActor 的位置
            if (teleportFlag)
            {
                previousPosition = transform.position;
                transform.position = CharacterActor.Position;
                return;
            }

            // 计算角色的平面位移和垂直位移
            Vector3 planarDisplacement = Vector3.ProjectOnPlane(CharacterActor.transform.position - previousPosition, CharacterActor.Up);
            Vector3 verticalDisplacement = Vector3.Project(CharacterActor.transform.position - previousPosition, CharacterActor.Up);

            // 计算地面探测位移
            float groundProbingDisplacement = CharacterActor.transform.InverseTransformVectorUnscaled(CharacterActor.GroundProbingDisplacement).y;

            // 如果角色没有探测地面，则增加恢复计时器
            if (Mathf.Abs(groundProbingDisplacement) < 0.01f)
                recoveryTimer += dt;
            else
                recoveryTimer = 0f;

            // 根据垂直位移选择正位移速度和负位移速度
            bool upwardsLerpDirection = CharacterActor.transform.InverseTransformVectorUnscaled(verticalDisplacement).y >= 0f;
            float displacementTSpeed = upwardsLerpDirection ? positiveDisplacementSpeed : negativeDisplacementSpeed;

            // 根据恢复计时器计算插值速度
            float lerpTSpeedOutput = Mathf.Min(displacementTSpeed + ((maxRecoverySpeed - displacementTSpeed) / recoveryDuration) * recoveryTimer, maxRecoverySpeed);

            // 更新 transform 位置
            transform.position = previousPosition + planarDisplacement + Vector3.Lerp(Vector3.zero, verticalDisplacement, lerpTSpeedOutput * dt);

            // 更新 previousPosition 为新位置
            previousPosition = transform.position;
        }
    }
}
