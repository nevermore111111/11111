using UnityEngine;
using Cinemachine;

public class TestLong : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    private Transform firstTarget;
    private Transform secondTarget;

    private void Start()
    {
        // 获取Cinemachine Target Group
        targetGroup = GetComponent<CinemachineTargetGroup>();

        // 获取第一个和第二个目标的Transform
        if (targetGroup.m_Targets.Length > 0)
            firstTarget = targetGroup.m_Targets[0].target.transform;

        if (targetGroup.m_Targets.Length > 1)
            secondTarget = targetGroup.m_Targets[1].target.transform;
    }

    private void Update()
    {
        if (targetGroup.m_Targets.Length > 1)
            secondTarget = targetGroup.m_Targets[1].target.transform;
        // 按下"T"键计算并打印距离
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (firstTarget != null && secondTarget != null)
            {
                float distance = Vector3.Distance(firstTarget.position, secondTarget.position);
                Debug.Log("Distance between targets: " + distance.ToString("F2"));
            }
            else
            {
                Debug.LogWarning("Not enough targets in the Target Group.");
            }
        }
    }
}
