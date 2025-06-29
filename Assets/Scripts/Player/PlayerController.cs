using NPCBehavior;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// 幽灵状态
/// </summary>
public enum PlayerState
{
    Idle,
    Move,
    //Terrified
    //附身
    //Influence
}
/// <summary>
/// 幽灵控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Transform _transform;
    private BoxCollider _boxCollider;

    public Material outline;
    public Material originMat;

    private SpriteRenderer spriteRenderer;

    [SerializeField] KeyCode Controllable_key = KeyCode.E; // 附身状态的触发键
    [SerializeField] KeyCode Trigger_key = KeyCode.Q; // 附身状态的触发键

    [SerializeField] bool isControllable = false; // 是否处于附身状态

    private PlayerState _currentState = PlayerState.Idle;
    public float moveSpeed = 5.0f; // 移动速度
    public float movementSmoothing = 0.5f;
    private Vector3 veloc;
    float h, v;
    //改变颜色来显示不同状态
    private SpriteRenderer _spriteRenderer;
    public Color idleColor = Color.white; // 空闲状态颜色
    public Color moveColor = Color.white; // 移动状态颜色
    public Color influenceColor = Color.red; // 附身状态颜色


    public GameObject currentInfluenceObject = null;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        // 初始化状态

        ChangeState(PlayerState.Idle);
    }



    void checkState()
    {
        if ( currentInfluenceObject != null && Input.GetKeyDown(Controllable_key)) // 假设按E键进入附身状态
        {
            isControllable = !isControllable; // 切换附身状态
        }

        if (isControllable)
        {
            _rigidbody.velocity = Vector3.zero;
            InfluenceState();
        }
        


        // 检测输入，切换到移动状态
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        // 目标速度（不考虑重力时，直接用输入轴）
        Vector3 targetVelocity = new Vector3(h,0,v) * moveSpeed;

        // 平滑速度变化
        Vector3 velocity = Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref veloc, movementSmoothing);

        _rigidbody.velocity = velocity;

        if (h != 0 || v != 0) ChangeState(PlayerState.Move);
        else ChangeState(PlayerState.Idle);
    }
    private void Update()
    {

        checkState();
        switch (_currentState)
        {
            case PlayerState.Idle:
                IdleState();
                break;
            case PlayerState.Move:
                MoveState();
                break;

        }
    }

    private void IdleState()
    {
        // 空闲状态逻辑
        if (!isControllable)
            _spriteRenderer.color = idleColor;

        _rigidbody.velocity = Vector2.zero;



    }

    private void MoveState()
    {
        // 移动状态逻辑

        //附身具有最高优先级
        if (!isControllable)
            _spriteRenderer.color = moveColor;




    }

    //附身逻辑比较特殊
    private void InfluenceState()
    {
        // 附身状态逻辑
        _spriteRenderer.color = influenceColor;
        // 可以在这里添加附身状态的特殊逻辑

        if (Input.GetKeyDown(Trigger_key)&& currentInfluenceObject!=null)
        {
            ObjectBase objectBase = currentInfluenceObject.GetComponent<ObjectBase>();
            objectBase.Trigger(); // 触发附身物体的技能
            Debug.Log("Trigger_key!");
        }
        else
        {
            Debug.Log("Trigger_key but no currentInfluenceObject");
        }

    }

    public void ChangeState(PlayerState newState)
    {
        _currentState = newState;
        //Debug.Log("State changed to: " + _currentState);
    }
    #region 碰撞检测,使用tag标记
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collider Enter:"+collision.name);
        // 检测是否有可附身的物体进入触发区域
        if (collision.CompareTag("Controllable"))
        {  
            currentInfluenceObject = collision.gameObject;
            spriteRenderer = currentInfluenceObject.GetComponent<SpriteRenderer>();
            originMat = spriteRenderer.material;
            spriteRenderer.material = outline;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Collider Exit:" + collision.name);

        if (collision.CompareTag("Controllable") && spriteRenderer != null)
        {
            spriteRenderer.material = originMat;
            spriteRenderer = null;
            currentInfluenceObject = null;
        }
    }
    #endregion
}

