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
        // �����������л����������������
        bool switchToSubCamera = false;

        if (mainCharacter != null)
        {
            // ����1���ܵ�����
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
            if (Time.time - mainCharacter.GetLastMoveTime() > 1.5f)
            {
                switchToMainCamera = true;
            }

            // ����3���������ƶ�ʱ�����л�
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                switchToMainCamera = true;
            }
        }

        // ���������л������
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

        // �������״̬
        if (cinemachineBrain != null)
        {
            cinemachineBrain.UpdateVirtualCameras(cinemachineBrain.ActiveBlend);
        }
    }
}
