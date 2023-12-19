using Lightbug.CharacterControllerPro.Implementation;
using UnityEngine;
public class ExtremeEvadeArea : MonoBehaviour
{
    // ��Inspector��ͼ�����ü������ܵĳ���ʱ��
    public float extremeEvadeDuration = 3.0f;

    private bool isActivated = false;
    private float activationTime = 0f;

    void Update()
    {
        if (isActivated)
        {
            // ���������ܵĳ���ʱ��
            if (Time.time - activationTime >= extremeEvadeDuration)
            {
                DeactivateArea();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // �����������������ʱ������Ƿ���Դ�����������
        if (other.CompareTag("Player"))
        {

        }
    }

    void ActivateArea()
    {
        isActivated = true;
        activationTime = Time.time;

        // ��������Դ���������������߼�
    }

    void DeactivateArea()
    {
        isActivated = false;

        // ��������Դ���ֹͣ��������������߼�
    }
}
