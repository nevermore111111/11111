using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera subCamera;
    public CheckEnemy checkEnemy;
    public MainCharacter mainCharacter;

    [Tooltip("如果存在测试摄像机，那么测试摄像机为最大权重")]
    public CinemachineFreeLook mainCharacterPrefab;


    public int mainCameraPriority = 0;
    public int subCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;

    /// <summary>
    ///摄像机切换逻辑
    ///在人物攻击且范围内存在敌人的时候
    ///在人物受到攻击的时候
    ///返回正常视角的时间
    ///当人物自由移动或者范围内没有敌人的时候，持续1.5s后，返回自由视角。
    ///转动屏幕的时候，立刻返回自动相机
    /// </summary>
    private void Start()
    {
        mainCharacter = FindObjectOfType<MainCharacter>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        mainCamera =  GameObject.Find("MainCamera").GetComponent<CinemachineFreeLook>();
        subCamera = GameObject.Find("subCamera").GetComponent<CinemachineVirtualCamera>();
        checkEnemy = FindObjectOfType<CheckEnemy>();
    }

    private void Update()
    {
        // 检查主摄像机切换到副摄像机的条件
        bool switchToSubCamera = false;

        if (mainCharacter != null)
        {
            // 条件1：受到攻击
#warning(这里没做)
            if (mainCharacter.GetIsAttacked())
            {
                switchToSubCamera = true;
            }

            // 条件2：发起攻击且范围内存在敌人
            if (mainCharacter.GetIsAttacking() && mainCharacter.enemies.Count > 0)
            {
                switchToSubCamera = true;
            }
        }

        // 检查副摄像机切换到主摄像机的条件
        bool switchToMainCamera = false;

        if (mainCharacter != null)
        {
            // 条件1：攻击范围内敌人为0
            if (mainCharacter.enemies.Count == 0)
            {
                switchToMainCamera = true;
            }

            // 条件2：移动时间超过1.5秒
            if (mainCharacter.ismoving())
            {
                switchToMainCamera = true;
            }

            // 条件3：鼠标存在移动时立即切换
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
        }

        // 根据条件切换摄像机
        if (switchToSubCamera)
        {
            mainCamera.Priority = mainCameraPriority;
            subCamera.Priority = subCameraPriority;
        }
        else if (switchToMainCamera)
        {
            mainCamera.Priority = subCameraPriority;
            subCamera.Priority = mainCameraPriority;
        }

        // 更新相机状态
     
    }
}
