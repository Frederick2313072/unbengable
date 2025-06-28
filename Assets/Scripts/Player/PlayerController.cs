using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody2D _rigidbody2D;
    private Transform _transform;
    private BoxCollider2D _boxCollider;

    [SerializeField] KeyCode Controllable_key = KeyCode.E; // 附身状态的触发键
    bool isControllable = false; // 是否处于附身状态

    private PlayerState _currentState = PlayerState.Idle;
    public float moveSpeed = 5.0f; // 移动速度
    public float movementSmoothing = 0.5f;
    private Vector2 veloc;
    float h, v;
    //改变颜色来显示不同状态
    private SpriteRenderer _spriteRenderer;
    public Color idleColor = Color.white; // 空闲状态颜色
    public Color moveColor = Color.white; // 移动状态颜色
    public Color influenceColor = Color.red; // 附身状态颜色


    private GameObject currentInfluenceObject = null;


    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        // 初始化状态

        ChangeState(PlayerState.Idle);
    }



    void checkState()
    {
        // 检测附身状态,附身状态可以和其他状态重合
        if (Input.GetKeyDown(Controllable_key)) // 假设按E键进入附身状态
        {
            isControllable = !isControllable; // 切换附身状态
        }

        if (isControllable)
        {
            InfluenceState();
        }
        else if (currentInfluenceObject != null)
        {
            //释放物体,取消父物体绑定
            currentInfluenceObject.transform.SetParent(null);
        }


        // 检测输入，切换到移动状态
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        // 目标速度（不考虑重力时，直接用输入轴）
        Vector2 targetVelocity = new Vector2(h, v) * moveSpeed;

        // 平滑速度变化
        Vector2 velocity = Vector2.SmoothDamp(_rigidbody2D.velocity, targetVelocity, ref veloc, movementSmoothing);

        _rigidbody2D.velocity = velocity;

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

        _rigidbody2D.velocity = Vector2.zero;



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


    }

    public void ChangeState(PlayerState newState)
    {
        _currentState = newState;
        Debug.Log("State changed to: " + _currentState);
    }
    #region 碰撞检测,使用tag标记
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // 检测是否有可附身的物体进入触发区域
        if (isControllable && collision.CompareTag("Controllable"))
        {
            currentInfluenceObject = collision.gameObject;

            currentInfluenceObject.transform.SetParent(transform); // 将附身物体设置为玩家的子物体
            currentInfluenceObject.transform.localPosition = Vector3.zero; // 重置位置

            Debug.Log("Controllable object detected: " + currentInfluenceObject.name);
        }
    }


    #endregion
}

