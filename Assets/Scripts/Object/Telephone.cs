using System.Collections;
using NPCBehavior;
using UnityEngine;

namespace Object
{
    public class Telephone : ObjectBase
    {
        public NPCFearType feartype = NPCFearType.Telephone; // Fear type for this object
        
        public Animator anim;

        void Start()
        {
            anim = GetComponent<Animator>();
        }
        
        public override void Trigger()
        {
            base.Trigger();
            
            // 播放动画
            StartCoroutine(PlayAnimation());
            
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
        
        IEnumerator PlayAnimation()
        {
            if (anim != null)
            {
                anim.SetBool("ring", true);
                yield return new WaitForSeconds(1.0f); // 等待动画播放完成
                anim.SetBool("ring", false); // 停止动画
            }
        }
        
    }
}