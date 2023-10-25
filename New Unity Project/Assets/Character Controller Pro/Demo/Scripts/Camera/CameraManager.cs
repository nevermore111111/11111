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
    public float tooFarDis = 5f;
    float distanceToEnemy;

    int LowCameraPriority = 0;
    int HighCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;
    CheckCamera checkCamera;

    private int prevMainCameraPriority; // ������һ֡��ʼʱ���������Priority
    private int prevSubCameraPriority; // ������һ֡��ʼʱ�ĸ������Priority

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
        CheckCamera();
    }

    private void CheckCamera()
    {
        // ��ÿ֡��ʼǰ��¼�������Priority
        int startMainCameraPriority = mainCamera.Priority;
        int startSubCameraPriority = subCamera.Priority;

        // �����������л����������������
        bool switchToSubCamera = false;
        bool switchToMainCamera = false;



        if (mainCharacter != null && mainCharacter.enemies.Count > 0)
        {
            // ����1���ܵ�����
            if (mainCharacter.GetIsAttacked()|| mainCharacter.GetIsAttacking())
            {
                switchToSubCamera = true;
                switchToMainCamera = false;
            }
        }
        // ��鸱������л����������������
        if (mainCharacter != null)
        {
            // ����1��������Χ�ڵ���Ϊ0
            if (mainCharacter.enemies.Count == 0 || IsSuitableDistance() || mainCharacter.ismoving()|| IsMoveMouse())
            {
                switchToMainCamera = true;
                switchToSubCamera = false;
            }
        }
        if (switchToSubCamera && !checkCamera.ShouldSwitchToSubCamera())
        {
            Debug.Log("�ǶȲ��ϸ���ת��");
            switchToMainCamera = true;
        }

       

        // ���������л������
        if (switchToSubCamera)
        {
            mainCamera.Priority = LowCameraPriority;
            subCamera.Priority = HighCameraPriority;
            //�л���sub����ʱ����Ҫ���ݸ��ݵ�ǰ�������ȥ����subcamera��
           
        }
        else if (switchToMainCamera)
        {
            mainCharacter.CharacterStateController.ExternalReference = cinemachineBrain.transform;
            mainCamera.Priority = HighCameraPriority;
            subCamera.Priority = LowCameraPriority;
        }

        // �����ʼʱ��Priority����һ֡��ʼʱ��ͬ��ִ��ChangeTransform����
        //if (startMainCameraPriority != prevMainCameraPriority || startSubCameraPriority != prevSubCameraPriority)
        //{
        //    ChangeTransform();
        //}

        // ���±����Priority
        prevMainCameraPriority = startMainCameraPriority;
        prevSubCameraPriority = startSubCameraPriority;
       
    }

    private static bool IsMoveMouse()
    {
        return Input.GetAxis("Mouse X")! > 0.1f || Input.GetAxis("Mouse Y") > 0.1f;
    }

    /// <summary>
    /// ����Ƿ��ں��ʵľ���
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

    private void ChangeTransform()
    {
        mainCamera.transform.position = Camera.main.transform.position;
        mainCamera.transform.rotation = Camera.main.transform.rotation;
        subCamera.transform.position = Camera.main.transform.position;
        subCamera.transform.rotation = Camera.main.transform.rotation;
    }
}
