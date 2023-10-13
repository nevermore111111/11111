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
        // ��ÿ֡��ʼǰ��¼�������Priority
        int startMainCameraPriority = mainCamera.Priority;
        int startSubCameraPriority = subCamera.Priority;

        // �����������л����������������
        bool switchToSubCamera = false;

        if (mainCharacter != null)
        {
            // ����1���ܵ�����
#warning(����û��)
            if (mainCharacter.GetIsAttacked())
            {
                switchToSubCamera = true;
            }

            // ����2�����𹥻��ҷ�Χ�ڴ��ڵ���
            if (mainCharacter.GetIsAttacking() && mainCharacter.enemies.Count > 0)
            {
                switchToSubCamera = true;
            }
        }

        // ��鸱������л����������������
        bool switchToMainCamera = false;

        if (mainCharacter != null)
        {
            // ����1��������Χ�ڵ���Ϊ0
            if (mainCharacter.enemies.Count == 0)
            {
                switchToMainCamera = true;
            }
            //else if((mainCharacter.selectEnemy.transform.position - mainCharacter.transform.position).sqrMagnitude < 4)
            //{
            //    switchToMainCamera = true;
            //}

            // ����2���ƶ�ʱ�䳬��1��
            if (mainCharacter.ismoving())
            {
                switchToMainCamera = true;
            }

            // ����3���������ƶ�ʱ�����л�
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
            if (!checkCamera.ShouldSwitchToSubCamera())
            {
                Debug.Log("�ǶȲ��ϸ���ת��");
                ////�����ǰ�ǶȺܴ���ôֱ�ӷ�����ͨ�ӽ�
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

        // ���������л������
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

        // �����ʼʱ��Priority����һ֡��ʼʱ��ͬ��ִ��ChangeTransform����
        //if (startMainCameraPriority != prevMainCameraPriority || startSubCameraPriority != prevSubCameraPriority)
        //{
        //    ChangeTransform();
        //}

        // ���±����Priority
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
