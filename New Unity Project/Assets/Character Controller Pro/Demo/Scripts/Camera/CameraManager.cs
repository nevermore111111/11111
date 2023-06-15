using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera subCamera;
    public CheckEnemy checkEnemy;
    public MainCharacter mainCharacter;

    public int mainCameraPriority = 10;
    public int subCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;

    private void Start()
    {
        mainCharacter = FindObjectOfType<MainCharacter>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
    }

    private void Update()
    {
        // 检查主摄像机切换到副摄像机的条件
        bool switchToSubCamera = false;

        if (mainCharacter != null)
        {
            // 条件1：受到攻击
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
            if (Time.time - mainCharacter.GetLastMoveTime() > 1.5f)
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
        if (cinemachineBrain != null)
        {
            cinemachineBrain.UpdateVirtualCameras(cinemachineBrain.ActiveBlend);
        }
    }
}
