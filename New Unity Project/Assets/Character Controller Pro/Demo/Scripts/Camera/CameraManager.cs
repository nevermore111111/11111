using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera subCamera;
    CheckEnemy checkEnemy;
    MainCharacter mainCharacter;

    [Tooltip("������ڲ������������ô���������Ϊ���Ȩ��")]
    public CinemachineFreeLook mainCharacterPrefab;


     int LowCameraPriority = 0;
     int HighCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;

    CheckCamera checkCamera;

    /// <summary>
    ///������л��߼�
    ///�����﹥���ҷ�Χ�ڴ��ڵ��˵�ʱ��
    ///�������ܵ�������ʱ��
    ///���������ӽǵ�ʱ��
    ///�����������ƶ����߷�Χ��û�е��˵�ʱ�򣬳���1.5s�󣬷��������ӽǡ�
    ///ת����Ļ��ʱ�����̷����Զ����
    /// </summary>
    private void Start()
    {
        mainCharacter = FindObjectOfType<MainCharacter>();
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<CinemachineFreeLook>();
        subCamera = GameObject.Find("subCamera").GetComponent<CinemachineVirtualCamera>();
        checkEnemy = FindObjectOfType<CheckEnemy>();
        checkCamera = subCamera.GetComponent<CheckCamera>();
    }

    private void Update()
    {
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

            // ����2���ƶ�ʱ�䳬��1.5��
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
                //Debug.Log("�ǶȲ��ϸ���ת��");
                ////�����ǰ�ǶȺܴ���ôֱ�ӷ�����ͨ�ӽ�
                switchToMainCamera = true;
            }
        }
        if (subCamera.Priority == HighCameraPriority)
        {
            if (switchToMainCamera == true)
            {
                Debug.Log("");
            }
        }

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
        // �������״̬
    }
}
