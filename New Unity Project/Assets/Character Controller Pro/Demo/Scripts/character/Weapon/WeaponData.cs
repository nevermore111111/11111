using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SaveDuringPlay]
public class WeaponData : MonoBehaviour
{
    [Header("是否打印攻击类型")]
    public bool PrintHit = true;

    [Header("这个脚本会运行时保存")]
    [SerializeField]
    [Range(0f,1f)] // 使用Range属性来定义可编辑范围
    public float impulseValue0 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)] // 使用Range属性来定义可编辑范围
    public float impulseValue1 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)] // 使用Range属性来定义可编辑范围
    public float impulseValue2 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)] // 使用Range属性来定义可编辑范围
    public float impulseValue3 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)] // 使用Range属性来定义可编辑范围
    public float impulseValue4 = 1.0f; // 初始值

    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue0 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue1 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue2 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue3 = 1.0f; // 初始值
    [SerializeField]
    [Range(0f, 1f)]
    public float durationValue4 = 1.0f; // 初始值


}
