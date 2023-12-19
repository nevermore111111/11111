using Unity.Mathematics;
using UnityEngine;

public class Bullet : Projectile
{
    private Vector3 lastFramePosition;

    protected override void Start()
    {
        base.Start();
        lastFramePosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();
        lastFramePosition = transform.position;
    }

    protected override void CheckTrigger()
    {
        // �������߷���
        Vector3 direction = transform.position - lastFramePosition;

        // ��������
        RaycastHit hit;
        if (Physics.Raycast(lastFramePosition, direction, out hit, direction.magnitude))
        {
            // �����ײ�����Ƿ��� AttackReceive ���
            AttackReceive attackReceiver;
            
            // ������� AttackReceive �����ִ������߼�
            if (hit.collider.TryGetComponent(out attackReceiver)&& attackReceiver.isNormalReceive())
            {
                // �������߼������Դ����˺�����Ϣ�� AttackReceive ���
                attackReceiver.CharacterInfo.GetDamage(damage, this.transform.forward, -1f);
            }
        }
    }

    // ... ��������������
}
