using UnityEngine;

public class CheckCamera : MonoBehaviour
{
    /// ���������н��Ƿ������л����� <summary>
    /// ���������н��Ƿ������л�����
    /// </summary>
    /// <returns></returns>
    public bool ShouldSwitchToSubCamera()
    {
        // ����н�sinֵ>��0.5������true�����򷵻�false.<0.5�Ļ����ǲ���ת����
        float tarAngele = Vector3.Angle(transform.forward, Vector3.up);
        return Mathf.Sin(Mathf.Deg2Rad * tarAngele) > 0.5f;//���ֵԽ�����̷�ΧԽС(ȡֵ��Χ ��0,1��)
    }
}
