using NPCBehavior;
using UnityEngine;

public class TestObject : ObjectBase
{
    public NPCFearType feartype = NPCFearType.Painting; // Fear type for this object
    
    public Animator anim;
    public override void Trigger()
    {
        base.Trigger();
        
        // 获取范围内的所有碰撞体
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distance);
        foreach (var hitCollider in hitColliders)
        {
            // 检查是否有 NpcController 组件
            NpcController npc = hitCollider.GetComponent<NpcController>();
            if (npc != null)
            {
                // 调用 NpcController 的目标函数
                npc.TriggerShock(3.0f, feartype); // 示例：触发 Shock 状态，持续 3 秒
            }
        }
        Debug.Log("TestObject Triggered");
    }
    
    private void OnDrawGizmosSelected()
    {
        // 在编辑器中可视化影响范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}