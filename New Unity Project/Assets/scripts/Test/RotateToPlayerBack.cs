using UnityEngine;
using Cinemachine;


public class RotateToPlayerBack : MonoBehaviour
{
    public Transform player;
    public CinemachineFreeLook freeLookCamera;
    public float rotationSpeed = 2f;
    public CinemachineTargetGroup group;
    Vector3 playToTarget;
    Vector3 cameraToTargetPosition;
    public Camera Camera;

    private float lastMouseInputTime;
    private const float mouseInputDelay = 0.5f;


    public Vector3 vector1;
    public Vector3 vector2;

    private void Start()
    {
        // ��ȡCinemachine FreeLook Camera
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {

        //����ʵ�ֵģ�����Һ�





        // ����������������
        float mouseX = Input.GetAxisRaw("Mouse X");

        // ���������벻Ϊ0��������ϴ���������ʱ���
        //if (mouseX != 0)
        //{
        //    lastMouseInputTime = Time.time;
        //}

        // �ж��Ƿ񳬹���������ӳ�ʱ�䣬������ִ���Զ�ת��
       // if (Time.time - lastMouseInputTime >= mouseInputDelay)
        {
            //�����������ķ���
            cameraToTargetPosition = -(gameObject.transform.position - freeLookCamera.LookAt.position);
            Vector3 planeNormal = Vector3.up; // Y��Ϊƽ��ķ���
            Vector3 projectedVector1 = Vector3.ProjectOnPlane(cameraToTargetPosition, planeNormal);
     

          
        
            if (group.m_Targets.Length > 1)
            {
                //������������ھ�ͷ�ı߽硣�Ҿ͸���ת������ͷ
                Vector3 targetPosition = group.m_Targets[1].target.transform.position;
                // Vector3 screenPosition = freeLookCamera.istar
                //  Debug.Log(screenPosition);

                //playToTarget = group.m_Targets[1].target.transform.position - group.m_Targets[0].target.transform.position;
                //Vector3 projectedVector2 = Vector3.ProjectOnPlane(playToTarget, planeNormal);

                //float angle = Vector3.Angle(projectedVector1, projectedVector2);
                //Quaternion targetRotate = Quaternion.FromToRotation(projectedVector2, projectedVector1);


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

                //Debug.Log(angle);
                //// �жϽǶ��Ƿ����30��
                //if (angle > 30f)
                //{
                //    freeLookCamera.m_XAxis.Value = -Mathf.LerpAngle(freeLookCamera.m_XAxis.Value, targetRotate.eulerAngles.y, group.m_Targets[1].weight * rotationSpeed * Time.deltaTime);
                //    Debug.Log(freeLookCamera.m_XAxis.Value);
                //}
            }
        }
    }
}
