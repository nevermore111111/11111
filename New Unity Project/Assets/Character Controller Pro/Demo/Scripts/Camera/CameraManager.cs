using UnityEngine;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public CinemachineVirtualCamera subCamera;
    public CheckEnemy checkEnemy;
    public MainCharacter mainCharacter;

    [Tooltip("������ڲ������������ô���������Ϊ���Ȩ��")]
    public CinemachineFreeLook mainCharacterPrefab;


    public int mainCameraPriority = 0;
    public int subCameraPriority = 20;

    private CinemachineBrain cinemachineBrain;

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
        mainCamera =  GameObject.Find("MainCamera").GetComponent<CinemachineFreeLook>();
        subCamera = GameObject.Find("subCamera").GetComponent<CinemachineVirtualCamera>();
        checkEnemy = FindObjectOfType<CheckEnemy>();
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
     
    }
}
