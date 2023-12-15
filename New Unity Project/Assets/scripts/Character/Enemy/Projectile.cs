using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 10;

    protected virtual void Start()
    {
        // ���������ڽ��������ٷ�����
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        // ��ǰ���ƶ�
        Move();

        // ��鴥������
        CheckTrigger();
    }

    protected virtual void Move()
    {
        // �����ٶ���ǰ�ƶ�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void HandleCollision(Collider other)
    {
        // ������ײ�߼�����������˺�������Ч����
        // ����ֻ�Ǽ򵥵����ٷ��������Ը��ݾ��������޸�
        Destroy(gameObject);
    }

    protected virtual void CheckTrigger()
    {
        // ��������Ӵ����������߼�
        // ���磬����Լ�����ض���ǩ�����巢����ײ��Ȼ��ִ����Ӧ�Ĳ���
    }
}
