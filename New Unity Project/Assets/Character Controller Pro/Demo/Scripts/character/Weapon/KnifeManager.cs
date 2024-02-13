using Unity.Mathematics;
using UnityEngine;

public class KnifeManager : MonoBehaviour
{
    public float detectionWidth;      // 盒形检测的宽度
    public float detectionHeight = 0.1f;     // 盒形检测的高度
    public float detectionDepth;      // 盒形检测的深度

    public Transform knifeTip;  // 刀尖
    public Transform knifeTail; // 刀尾

    private BoxDetection boxDetection;  // BoxDetector实例
    private Vector3 previousTipPosition;  // 上一帧刀尖的位置
    private Vector3 previousTailPosition; // 上一帧刀尾的位置
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

        // 初始化上一帧的位置
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
        // 获取当前box的中心点
        boxCenter = (previousTipPosition + previousTailPosition + knifeTip.position + knifeTail.position) / 4f;

        Vector3 dis01 = (knifeTip.position - previousTipPosition);
        Vector3 dis02 = (knifeTail.position - previousTailPosition);
        Vector3 X = (dis01 + dis02) / 2f;//近似的x
        if(X == Vector3.zero)
        {
            //第一帧还没开始动
            X = knifeTip.forward;
        }
        float disF = (dis01.magnitude+ dis02.magnitude)*0.5f;
        Vector3 DISz01 = (previousTipPosition - previousTailPosition);
        Vector3 DISz02 = (knifeTip.position - knifeTail.position);
        Vector3 z = (DISz01 + DISz02) / 2f;//近似的z
        //x方向不能太小
        disF = disF > detectionDepth ? disF : detectionDepth;
        half = 0.5f * new Vector3(disF, detectionHeight, detectionDepth);
        Vector3 Y = Vector3.Cross(z, X);//近似的y

        quaternion = Quaternion.LookRotation(z, Y);
        // 更新上一帧的位置
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