using System.Collections;
using NPCBehavior;
using UnityEngine;

namespace Object
{
    public class Sofa : ObjectBase
    {
        public NPCFearType feartype = NPCFearType.Chair; // Fear type for this object
        
        public Animator anim;
        
        public string musicManagerName = "MusicManager"; // Name of the GameObject containing the MusicManager
        public MusicManager musicManager; // Reference to the MusicManager for sound effects

        void Start()
        {
            anim = GetComponent<Animator>();
            
            if (musicManager == null)
            {
                musicManager = GameObject.Find(musicManagerName).GetComponent<MusicManager>();
            }
        }

        public override void TriggerAudio()
        {
            musicManager.PlayRandomWoodMovement();
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
                anim.SetBool("isChange", true);
                TriggerAudio(); 
                yield return new WaitForSeconds(1.0f); // 等待动画播放完成
                anim.SetBool("isChange", false); // 停止动画
            }
        }
        
    }
}