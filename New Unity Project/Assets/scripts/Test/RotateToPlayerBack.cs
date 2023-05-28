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
        }
    }
}
