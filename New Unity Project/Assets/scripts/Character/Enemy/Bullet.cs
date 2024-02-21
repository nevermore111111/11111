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
        // 计算射线方向
        Vector3 direction = transform.position - lastFramePosition;

        // 发射射线
        RaycastHit hit;
        if (Physics.Raycast(lastFramePosition, direction, out hit, direction.magnitude))
        {
            // 检查碰撞体上是否有 AttackReceive 组件
            AttackReceive attackReceiver;
            
            // 如果存在 AttackReceive 组件，执行相关逻辑
            if (hit.collider.TryGetComponent(out attackReceiver)&& attackReceiver.isNormalReceive())
            {
                // 处理攻击逻辑，可以传递伤害等信息给 AttackReceive 组件
                //attackReceiver.CharacterInfo.GetDamage(damage, this.transform.forward, -1f);
            }
        }
    }

    // ... 其他方法和属性
}
