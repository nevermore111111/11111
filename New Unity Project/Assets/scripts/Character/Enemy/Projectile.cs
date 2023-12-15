using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public int damage = 10;

    protected virtual void Start()
    {
        // 在生命周期结束后销毁发射物
        Destroy(gameObject, lifetime);
    }

    protected virtual void Update()
    {
        // 在前方移动
        Move();

        // 检查触发条件
        CheckTrigger();
    }

    protected virtual void Move()
    {
        // 根据速度向前移动
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    protected virtual void HandleCollision(Collider other)
    {
        // 处理碰撞逻辑，例如造成伤害、播放效果等
        // 这里只是简单地销毁发射物，你可以根据具体需求修改
        Destroy(gameObject);
    }

    protected virtual void CheckTrigger()
    {
        // 在这里添加触发条件的逻辑
        // 例如，你可以检查与特定标签的物体发生碰撞，然后执行相应的操作
    }
}
