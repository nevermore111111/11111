using UnityEngine;
using Cinemachine;

public class TestLong : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;

    private Transform firstTarget;
    private Transform secondTarget;

    private void Start()
    {
        // ��ȡCinemachine Target Group
        targetGroup = GetComponent<CinemachineTargetGroup>();

        // ��ȡ��һ���͵ڶ���Ŀ���Transform
        if (targetGroup.m_Targets.Length > 0)
            firstTarget = targetGroup.m_Targets[0].target.transform;

        if (targetGroup.m_Targets.Length > 1)
            secondTarget = targetGroup.m_Targets[1].target.transform;
    }

    private void Update()
    {
        if (targetGroup.m_Targets.Length > 1)
            secondTarget = targetGroup.m_Targets[1].target.transform;
        // ����"T"�����㲢��ӡ����
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
