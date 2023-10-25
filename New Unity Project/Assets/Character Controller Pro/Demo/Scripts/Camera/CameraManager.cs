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
        CinemachineOrbitalTransposer = (subCamera as CinemachineVirtualCamera).GetCinemachineComponent<CinemachineOrbitalTransposer>();
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
            //ʹ��orbit�����ʱ�������л�
            if (mainCharacter.CharacterStateController.CurrentState is NormalMovement && CinemachineOrbitalTransposer != null)
            {
                //�Ƿ������л�
                switchToMainCamera = true;
            }
            // ����3���������ƶ�ʱ�����л�
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
            if (switchToSubCamera && !checkCamera.ShouldSwitchToSubCamera())
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

        {//���Һ͵��˾��벻��̫��
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

        // ���������л������
        if (switchToSubCamera)
        {
            mainCamera.Priority = LowCameraPriority;
            subCamera.Priority = HighCameraPriority;
            //�л���sub����ʱ����Ҫ���ݸ��ݵ�ǰ�������ȥ����subcamera��
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

        // �����ʼʱ��Priority����һ֡��ʼʱ��ͬ��ִ��ChangeTransform����
        //if (startMainCameraPriority != prevMainCameraPriority || startSubCameraPriority != prevSubCameraPriority)
        //{
        //    ChangeTransform();
        //}

        // ���±����Priority
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
