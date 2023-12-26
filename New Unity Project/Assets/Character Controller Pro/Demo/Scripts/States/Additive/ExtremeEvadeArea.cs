using Lightbug.CharacterControllerPro.Implementation;
using Rusk;
using UnityEngine;
public class ExtremeEvadeArea : MonoBehaviour
{
    // ��Inspector��ͼ�����ü������ܵĳ���ʱ��
    public float extremeEvadeDuration = 3.0f;

    private bool isActivated = false;
    private float activationTime = 0f;
    //���ܷ���
    private Vector3 evadeDir;

    public ExtremeEvadeArea(Evade evade)
    {
        ReSetEvadeArea(evade);
    }

    private void ReSetEvadeArea(Evade evade)
    {
        evade.OnEvadeStart += Evade_OnEvadeStart;
        evade.OnEvadeEnd += Evade_OnEvadeEnd;
        evadeDir = evade.evadeDirection;
    }

    private void Evade_OnEvadeEnd(Vector3 obj)
    {
        throw new System.NotImplementedException();
    }

    private void Evade_OnEvadeStart(Vector3 obj)
    {
        throw new System.NotImplementedException();
    }

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
