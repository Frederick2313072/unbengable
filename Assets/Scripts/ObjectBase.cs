using UnityEngine;
public class ObjectBase : MonoBehaviour
{
    //0:未激活，1:激活,2特殊
    public int state;
    //具体的技能配置
    public SkillBase skill;
    //物品生效的有效距离
    public float distance = 10.0f;
    //冷却时间
    public float cd;
    //每秒恢复的冷却时间
    private float cdPerSecond;
    //物体是否可以使用
    
    void Update()
    {
        // 按E触发Trigger
        if (Input.GetKeyDown(KeyCode.E))
        {
            Trigger();
        }
    }
    public bool IsReady
    {
        get
        {
            //上次使用后经过的时间
            float pass_time = Time.time - last_time;

            return pass_time >= cd;
        }
    }

    //上次使用的时间
    float last_time;

    //物体内部更新逻辑
    public virtual void InProcess()
    {

    }
    //触发技能
    public virtual void Trigger()
    {
        if (!IsReady)
        {
            return;
        }
        last_time = Time.time;
        //触发技能逻辑
        Debug.Log("Trigger skill: " + this.GetType().Name);
        InProcess();

    }
}
