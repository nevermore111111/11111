using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera subCamera;
    CheckEnemy checkEnemy;
    MainCharacter mainCharacter;

    int LowCameraPriority = 0;
    int HighCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;
    CheckCamera checkCamera;

    private int prevMainCameraPriority; // 保存上一帧开始时的主摄像机Priority
    private int prevSubCameraPriority; // 保存上一帧开始时的副摄像机Priority

    private void Start()
    {
        mainCharacter = FindObjectOfType<MainCharacter>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (mainCamera == null)
            mainCamera = GameObject.Find("FreeLook Camera_Main").GetComponent<CinemachineFreeLook>();
        if (subCamera == null)
            subCamera = GameObject.Find("subCamera").GetComponent<CinemachineVirtualCamera>();
        checkEnemy = FindObjectOfType<CheckEnemy>();
        checkCamera = subCamera.GetComponent<CheckCamera>();
    }

    private void Update()
    {
        // 在每帧开始前记录摄像机的Priority
        int startMainCameraPriority = mainCamera.Priority;
        int startSubCameraPriority = subCamera.Priority;

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
            //else if((mainCharacter.selectEnemy.transform.position - mainCharacter.transform.position).sqrMagnitude < 4)
            //{
            //    switchToMainCamera = true;
            //}

            // 条件2：移动时间超过1秒
            if (mainCharacter.ismoving())
            {
                switchToMainCamera = true;
            }

            // 条件3：鼠标存在移动时立即切换
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
            if (!checkCamera.ShouldSwitchToSubCamera())
            {
                Debug.Log("角度不合格导致转化");
                ////如果当前角度很大，那么直接返回普通视角
                switchToMainCamera = true;
            }
        }
        //if (subCamera.Priority == HighCameraPriority)
        //{
        //    if (switchToMainCamera == true)
        //    {
        //        Debug.Log("");
        //    }
        //}

        // 根据条件切换摄像机
        if (switchToSubCamera)
        {
            if (checkCamera != null)
            {
                mainCamera.Priority = LowCameraPriority;
                subCamera.Priority = HighCameraPriority;
            }
        }
        else if (switchToMainCamera)
        {
            mainCamera.Priority = HighCameraPriority;
            subCamera.Priority = LowCameraPriority;
        }

        // 如果开始时的Priority与上一帧开始时不同，执行ChangeTransform方法
        //if (startMainCameraPriority != prevMainCameraPriority || startSubCameraPriority != prevSubCameraPriority)
        //{
        //    ChangeTransform();
        //}

        // 更新保存的Priority
        prevMainCameraPriority = startMainCameraPriority;
        prevSubCameraPriority = startSubCameraPriority;
    }

    private void ChangeTransform()
    {
        mainCamera.transform.position = Camera.main.transform.position;
        mainCamera.transform.rotation = Camera.main.transform.rotation;
        subCamera.transform.position = Camera.main.transform.position;
        subCamera.transform.rotation = Camera.main.transform.rotation;
    }
}
