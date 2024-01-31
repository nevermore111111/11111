using UnityEngine;
using Cinemachine;
using Lightbug.CharacterControllerPro.Demo;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCameraBase mainCamera;
    public CinemachineVirtualCameraBase subCamera;
    CheckEnemy checkEnemy;
    MainCharacter mainCharacter;
   
    public float tooCloseDis = 2f;
    public float tooFarDis = 6.5f;
    float distanceToEnemy;

    int LowCameraPriority = 0;
    int HighCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;
    CheckCamera checkCamera;

    private int prevMainCameraPriority; // 保存上一帧开始时的主摄像机Priority
    private int prevSubCameraPriority; // 保存上一帧开始时的副摄像机Priority

    Transform TrueCameraTransform;


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
        TrueCameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        CheckCamera();
        CheckMouseMovement();
    }

    private bool isMouseMoving;
    private float mouseMoveStartTime;
    private void CheckMouseMovement()
    {
        if (IsMoveMouse(0.05f))
        {
            if (!isMouseMoving)
            {
                isMouseMoving = true;
                mouseMoveStartTime = Time.time;
            }
        }
        else
        {
            isMouseMoving = false;
        }
    }
    /// <summary>
    /// 返回鼠标移动是否超过了这个时间
    /// </summary>
    /// <param name="MoveTime"></param>
    /// <returns></returns>
    private bool IsMoveTimeGreaterNum(float MoveTime)
    {
        return Time.time - mouseMoveStartTime > MoveTime;
    }

    private void CheckCamera()
    {
        // 在每帧开始前记录摄像机的Priority
        int startMainCameraPriority = mainCamera.Priority;
        int startSubCameraPriority = subCamera.Priority;

        // 检查主摄像机切换到副摄像机的条件
        bool switchToSubCamera = false;
        bool switchToMainCamera = false;



        if (mainCharacter != null && mainCharacter.enemies.Count > 0)
        {
            // 条件1：受到攻击
            if (mainCharacter.GetIsAttacked() || mainCharacter.GetIsAttacking())
            {
                switchToSubCamera = true;
                switchToMainCamera = false;
            }
        }
        // 检查副摄像机切换到主摄像机的条件
        if (mainCharacter != null)
        {
            // 条件1：攻击范围内敌人为0
            if (mainCharacter.enemies.Count == 0 || IsSuitableDistance() || mainCharacter.ismoving() || (IsMoveMouse(0.3f)&& IsMoveTimeGreaterNum(0.1f)))
            {
                switchToMainCamera = true;
                switchToSubCamera = false;
            }
        }
        if (switchToSubCamera && !checkCamera.ShouldSwitchToSubCamera())
        {
            Debug.Log("角度不合格导致转化");
            switchToMainCamera = true;
        }



        // 根据条件切换摄像机
        if (switchToSubCamera)
        {
            mainCamera.Priority = LowCameraPriority;
            subCamera.Priority = HighCameraPriority;
            //切换到sub，这时候需要根据根据当前的摄像机去设置subcamera。

        }
        else if (switchToMainCamera)
        {
            mainCharacter.CharacterStateController.ExternalReference = cinemachineBrain.transform;
            mainCamera.Priority = HighCameraPriority;
            subCamera.Priority = LowCameraPriority;
        }

        // 如果开始时的Priority与上一帧开始时不同，执行ChangeTransform方法
        if (IsChangeCameraPriority(startMainCameraPriority, startSubCameraPriority))
        {
            ChangeTransform();
        }

        // 更新保存的Priority
        prevMainCameraPriority = startMainCameraPriority;
        prevSubCameraPriority = startSubCameraPriority;

    }

    /// <summary>
    /// 是否修改了摄像机的优先级
    /// </summary>
    /// <param name="startMainCameraPriority"></param>
    /// <param name="startSubCameraPriority"></param>
    /// <returns></returns>
    private bool IsChangeCameraPriority(int startMainCameraPriority, int startSubCameraPriority)
    {
        return startMainCameraPriority != prevMainCameraPriority || startSubCameraPriority != prevSubCameraPriority;
    }

    private static bool IsMoveMouse(float targetMouseMoveNum)
    {
        return Mathf.Abs(Input.GetAxis("Mouse X")) > targetMouseMoveNum || Mathf.Abs(Input.GetAxis("Mouse Y")) > targetMouseMoveNum;
    }

    /// <summary>
    /// 检查是否在合适的距离
    /// </summary>
    /// <returns></returns>
    private bool IsSuitableDistance()
    {
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
        return distanceToEnemy < tooCloseDis || distanceToEnemy > tooFarDis;
    }
    /// <summary>
    /// 强制摄像机更新到当前主摄像机的位置
    /// </summary>
    private void ChangeTransform()
    {
        if (mainCamera.Priority == HighCameraPriority)
        {
            mainCamera.ForceCameraPosition(TrueCameraTransform.position, TrueCameraTransform.rotation);
        }
        else
        {
            subCamera.ForceCameraPosition(TrueCameraTransform.position, TrueCameraTransform.rotation);
        }
    }
}
