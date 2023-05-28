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
        // 获取Cinemachine FreeLook Camera
        freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {

        //我想实现的，在玩家和





        // 检测玩家鼠标横轴输入
        float mouseX = Input.GetAxisRaw("Mouse X");

        // 如果鼠标输入不为0，则更新上次鼠标输入的时间戳
        //if (mouseX != 0)
        //{
        //    lastMouseInputTime = Time.time;
        //}

        // 判断是否超过鼠标输入延迟时间，若是则执行自动转向
       // if (Time.time - lastMouseInputTime >= mouseInputDelay)
        {
            //这个是摄像机的方向
            cameraToTargetPosition = -(gameObject.transform.position - freeLookCamera.LookAt.position);
            Vector3 planeNormal = Vector3.up; // Y轴为平面的法线
            Vector3 projectedVector1 = Vector3.ProjectOnPlane(cameraToTargetPosition, planeNormal);
     

          
        
            if (group.m_Targets.Length > 1)
            {

              
                playToTarget = group.m_Targets[1].target.transform.position - group.m_Targets[0].target.transform.position;
                Vector3 projectedVector2 = Vector3.ProjectOnPlane(playToTarget, planeNormal);

                float angle = Vector3.Angle(projectedVector1, projectedVector2);
                Quaternion targetRotate = Quaternion.FromToRotation(projectedVector2, projectedVector1);

                


                Debug.Log(angle);
                // 判断角度是否大于30度
                if (angle > 30f)
                {
                    freeLookCamera.m_XAxis.Value = -Mathf.LerpAngle(freeLookCamera.m_XAxis.Value, targetRotate.eulerAngles.y, group.m_Targets[1].weight * rotationSpeed * Time.deltaTime);
                    Debug.Log(freeLookCamera.m_XAxis.Value);
                }
            }
        }
    }
}
