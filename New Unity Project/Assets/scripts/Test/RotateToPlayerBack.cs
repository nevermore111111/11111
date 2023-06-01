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
                //并且这个物体在镜头的边界。我就给他转过来镜头
                Vector3 targetPosition = group.m_Targets[1].target.transform.position;
                // Vector3 screenPosition = freeLookCamera.istar
                //  Debug.Log(screenPosition);

                //playToTarget = group.m_Targets[1].target.transform.position - group.m_Targets[0].target.transform.position;
                //Vector3 projectedVector2 = Vector3.ProjectOnPlane(playToTarget, planeNormal);

                //float angle = Vector3.Angle(projectedVector1, projectedVector2);
                //Quaternion targetRotate = Quaternion.FromToRotation(projectedVector2, projectedVector1);


                Bounds viewSpaceBounds = group.GetViewSpaceBoundingBox(Camera.cameraToWorldMatrix);

                // 将视野空间边界框转换为屏幕空间
                Vector3 minScreenPoint = Camera.WorldToScreenPoint(viewSpaceBounds.min);
                Vector3 maxScreenPoint = Camera.WorldToScreenPoint(viewSpaceBounds.max);

                // 检查屏幕空间边界框是否在屏幕内
                bool isGroupVisible = minScreenPoint.x < Screen.width && maxScreenPoint.x > 0 &&
                                      minScreenPoint.y < Screen.height && maxScreenPoint.y > 0;

                if (isGroupVisible)
                {
                    // TargetGroup中的所有目标都在视野内的处理逻辑
                    Debug.Log("All targets in the group are visible!");
                }
                else
                {
                    // TargetGroup中的至少一个目标不在视野内的处理逻辑
                    Debug.Log("At least one target in the group is not visible!");
                }

                //Debug.Log(angle);
                //// 判断角度是否大于30度
                //if (angle > 30f)
                //{
                //    freeLookCamera.m_XAxis.Value = -Mathf.LerpAngle(freeLookCamera.m_XAxis.Value, targetRotate.eulerAngles.y, group.m_Targets[1].weight * rotationSpeed * Time.deltaTime);
                //    Debug.Log(freeLookCamera.m_XAxis.Value);
                //}
            }
        }
    }
}
