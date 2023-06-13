using UnityEngine;
using Cinemachine;


public class RotateToPlayerBack : MonoBehaviour
{

    public enum CameraMode {AutoRotate, Rotate, LookMid}
    public CameraMode mode;

    public Transform player;
    public CinemachineFreeLook freeLookCamera;
    public float rotationSpeed = 2f;
    public CinemachineTargetGroup group;
    Vector3 playToTarget;
    Vector3 cameraToTargetPosition;
    public Camera Camera;

    private float lastMouseInputTime;
    private const float mouseInputDelay = 0.5f;

    private MainCharacter MainCharacter;
    public Vector3 vector1;
    public Vector3 vector2;

    private void Start()
    {
        // ��ȡCinemachine FreeLook Camera
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        MainCharacter = GetComponent<CameraEffects>().MainCharacter;
    }

    private void Update()
    {
        RotateToEnemy();
       
    }

    private void RotateToEnemy()
    {
        if (mode == CameraMode.Rotate || mode == CameraMode.AutoRotate)
        {
            {
                //�����������ķ���
                cameraToTargetPosition = -(gameObject.transform.position - freeLookCamera.LookAt.position);
                Vector3 planeNormal = Vector3.up; // Y��Ϊƽ��ķ���
                Vector3 projectedVector1 = Vector3.ProjectOnPlane(cameraToTargetPosition, planeNormal);
               if(mode == CameraMode.AutoRotate)
                {
                    Rotate(planeNormal, projectedVector1);
                }
                else
                {
                    if (group.m_Targets.Length > 1)
                    {
                        //�ж��Ƿ��ڵ����߳�����Ļ��
                        if (IsObscured() || OutOfView())
                        {








                        }
                    }
                }



            }
        }
    }

    private void Rotate(Vector3 planeNormal, Vector3 projectedVector1)
    {
        if (group.m_Targets.Length > 1)
        {
            RotateCameraToEnemy(planeNormal, projectedVector1);

           // IsVisable();
        }
    }

    private  bool IsObscured()
    {
        return false;
    }
    private bool OutOfView()
    {
        return false;
    }
    private void RotateCameraToEnemy(Vector3 planeNormal, Vector3 projectedVector1)
    {
        //������������ھ�ͷ�ı߽硣�Ҿ͸���ת������ͷ
        Vector3 targetPosition = group.m_Targets[1].target.transform.position;
        Vector3 screenPosition = freeLookCamera.gameObject.transform.position;
        Debug.Log(screenPosition);

        playToTarget = group.m_Targets[1].target.transform.position - group.m_Targets[0].target.transform.position;
        Vector3 projectedVector2 = Vector3.ProjectOnPlane(playToTarget, planeNormal);

        float angle = Vector3.Angle(projectedVector1, projectedVector2);
        Quaternion targetRotate = Quaternion.FromToRotation(projectedVector2, projectedVector1);




        Debug.Log(angle);
        // �жϽǶ��Ƿ����30��
        if (angle > 30f)
        {
            freeLookCamera.m_XAxis.Value = -Mathf.LerpAngle(freeLookCamera.m_XAxis.Value, targetRotate.eulerAngles.y, group.m_Targets[1].weight * rotationSpeed * Time.deltaTime);
            Debug.Log(freeLookCamera.m_XAxis.Value);
        }
    }

    private void IsVisable()
    {
        Bounds viewSpaceBounds = group.GetViewSpaceBoundingBox(Camera.cameraToWorldMatrix);

        // ����Ұ�ռ�߽��ת��Ϊ��Ļ�ռ�
        Vector3 minScreenPoint = Camera.WorldToScreenPoint(viewSpaceBounds.min);
        Vector3 maxScreenPoint = Camera.WorldToScreenPoint(viewSpaceBounds.max);

        // �����Ļ�ռ�߽���Ƿ�����Ļ��
        bool isGroupVisible = minScreenPoint.x < Screen.width && maxScreenPoint.x > 0 &&
                              minScreenPoint.y < Screen.height && maxScreenPoint.y > 0;

        if (isGroupVisible)
        {
            // TargetGroup�е�����Ŀ�궼����Ұ�ڵĴ����߼�
            Debug.Log("All targets in the group are visible!");
        }
        else
        {
            // TargetGroup�е�����һ��Ŀ�겻����Ұ�ڵĴ����߼�
            Debug.Log("At least one target in the group is not visible!");
        }
    }
}
