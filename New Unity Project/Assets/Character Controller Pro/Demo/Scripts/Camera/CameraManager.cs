using UnityEngine;
using Cinemachine;
using Lightbug.CharacterControllerPro.Demo;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCameraBase mainCamera;
    public CinemachineVirtualCameraBase subCamera;
    CheckEnemy checkEnemy;
    MainCharacter mainCharacter;
    CinemachineOrbitalTransposer CinemachineOrbitalTransposer;
    public float tooCloseDis = 2f;
    public float tooFarDis = 5f;
    float distanceToEnemy;

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
        CinemachineOrbitalTransposer = (subCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineOrbitalTransposer>();
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
            //使用orbit轨道的时候立刻切换
            if (mainCharacter.CharacterStateController.CurrentState is NormalMovement && CinemachineOrbitalTransposer != null)
            {
                //是否立刻切换
                switchToMainCamera = true;
            }
            // 条件3：鼠标存在移动时立即切换
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
            if (switchToSubCamera && !checkCamera.ShouldSwitchToSubCamera())
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

        {//并且和敌人距离不能太近
            if (mainCharacter.selectEnemy != null)
                distanceToEnemy = (mainCharacter.transform.position - mainCharacter.selectEnemy.transform.position).magnitude;
            else if (mainCharacter.enemies.Count > 0)
            {
                distanceToEnemy = (mainCharacter.transform.position - mainCharacter.enemies[0].transform.position).magnitude;
            }
            else
            {
                distanceToEnemy = 0;
            }
            if (distanceToEnemy < tooCloseDis || distanceToEnemy > tooFarDis)
            {
                switchToSubCamera = false;
                switchToMainCamera = true;
            }
        }

        // 根据条件切换摄像机
        if (switchToSubCamera)
        {
            mainCamera.Priority = LowCameraPriority;
            subCamera.Priority = HighCameraPriority;
            //切换到sub，这时候需要根据根据当前的摄像机去设置subcamera。
            if (CinemachineOrbitalTransposer != null)
            {
                CinemachineOrbitalTransposer.m_FollowOffset.y = mainCamera.transform.position.y - mainCamera.Follow.position.y;
                mainCharacter.CharacterStateController.ExternalReference = mainCamera.transform;
            }
        }
        else if (switchToMainCamera)
        {
            mainCharacter.CharacterStateController.ExternalReference = cinemachineBrain.transform;
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
        if (subCamera.Priority > mainCamera.Priority && CinemachineOrbitalTransposer != null)
        {

        }
    }

    private void ChangeTransform()
    {
        mainCamera.transform.position = Camera.main.transform.position;
        mainCamera.transform.rotation = Camera.main.transform.rotation;
        subCamera.transform.position = Camera.main.transform.position;
        subCamera.transform.rotation = Camera.main.transform.rotation;
    }
}
