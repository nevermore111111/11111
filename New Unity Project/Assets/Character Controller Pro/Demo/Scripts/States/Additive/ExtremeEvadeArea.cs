using Lightbug.CharacterControllerPro.Implementation;
using UnityEngine;
public class ExtremeEvadeArea : MonoBehaviour
{
    // 在Inspector视图中设置极限闪避的持续时间
    public float extremeEvadeDuration = 3.0f;

    private bool isActivated = false;
    private float activationTime = 0f;

    void Update()
    {
        if (isActivated)
        {
            // 处理极限闪避的持续时间
            if (Time.time - activationTime >= extremeEvadeDuration)
            {
                DeactivateArea();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 当其他对象进入区域时，检查是否可以触发极限闪避
        if (other.CompareTag("Player"))
        {

        }
    }

    void ActivateArea()
    {
        isActivated = true;
        activationTime = Time.time;

        // 在这里可以处理激活区域的其他逻辑
    }

    void DeactivateArea()
    {
        isActivated = false;

        // 在这里可以处理停止激活区域的其他逻辑
    }
}
