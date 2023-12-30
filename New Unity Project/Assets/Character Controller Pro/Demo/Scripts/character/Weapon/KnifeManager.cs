using Unity.Mathematics;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    public float detectionWidth;      // ���μ��Ŀ��
    public float detectionHeight = 0.1f;     // ���μ��ĸ߶�
    public float detectionDepth;      // ���μ������

    public Transform knifeTip;  // ����
    public Transform knifeTail; // ��β

    private BoxDetection boxDetection;  // BoxDetectorʵ��
    private Vector3 previousTipPosition;  // ��һ֡�����λ��
    private Vector3 previousTailPosition; // ��һ֡��β��λ��
    Vector3 boxCenter;
    quaternion quaternion;
    Vector3 half;

    private void Awake()
    {
        boxDetection = GetComponent<BoxDetection>();
    }

    void Start()
    {
        detectionDepth = (knifeTail.transform.position - knifeTip.transform.position).magnitude;

        // ��ʼ����һ֡��λ��
        previousTipPosition = knifeTip.position;
        previousTailPosition = knifeTail.position;
    }
    public void ResetBeForeHit()
    {
        previousTipPosition = knifeTip.position;
        previousTailPosition = knifeTail.position;
    }



    public void BeforeBoxUpdate()
    {
        // ��ȡ��ǰbox�����ĵ�
        boxCenter = (previousTipPosition + previousTailPosition + knifeTip.position + knifeTail.position) / 4f;

        Vector3 dis01 = (knifeTip.position - previousTipPosition);
        Vector3 dis02 = (knifeTail.position - previousTailPosition);
        Vector3 X = (dis01 + dis02) / 2f;//���Ƶ�x
        if(X == Vector3.zero)
        {
            //��һ֡��û��ʼ��
            X = knifeTip.forward;
        }
        float disF = Mathf.Max(dis01.magnitude, dis02.magnitude);
        Vector3 DISz01 = (previousTipPosition - previousTailPosition);
        Vector3 DISz02 = (knifeTip.position - knifeTail.position);
        Vector3 z = (DISz01 + DISz02) / 2f;//���Ƶ�z
        //x������̫С
        disF = disF > detectionDepth ? disF : detectionDepth;
        half = 0.5f * new Vector3(disF, detectionHeight, detectionDepth);
        Vector3 Y = Vector3.Cross(z, X);//���Ƶ�y

        quaternion = Quaternion.LookRotation(z, Y);
        // ������һ֡��λ��
        boxDetection.boxCenter = boxCenter;
        boxDetection.halfExtents = half;
        boxDetection.orientation = quaternion;
        //Physics.OverlapBox(boxCenter, half, quaternion);

        previousTipPosition = knifeTip.position;
        previousTailPosition = knifeTail.position;
    }

//    private void OnDrawGizmos()
//    {
//#if UNITY_EDITOR
//        if (!Application.isPlaying) return;
//#endif
//        Debug.Log(half);
//        Gizmos.matrix = Matrix4x4.TRS(boxCenter, quaternion, Vector3.one);
//        Gizmos.color = Color.green;
//        Gizmos.DrawWireCube(Vector3.zero, 2 * half);
//        Gizmos.matrix = Matrix4x4.identity;
//    }


}