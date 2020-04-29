using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 添加该脚本时,自动添加Rigidbody2D 和 Animator脚本
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField]
    private float speed;
    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField]
    private string type;

    [SerializeField]
    private int level;

    /// <summary>
    /// 角色身上的Animator组件
    /// </summary>
    public Animator MyAnimator
    {
        get;set;
    }

    //移动方向
    private Vector2 direction;
    public Vector2 Direction
    {
        get => direction;
        set => direction = value;
    }
    /// <summary>
    /// 角色身上的刚体组件
    /// </summary>
    [SerializeField]
    private Rigidbody2D myRigidbdoy;

    protected Coroutine attackRoutine;

    /// <summary>
    /// 被击中范围判定
    /// </summary>
    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public Stat MyHealth
    {
        get
        {
            return health;
        }
    }

    public Transform MyTarget
    {
        get;set;
    }

    [SerializeField]
    protected float initHealth;

    public bool IsAttacking
    {
        get; set;
    }

    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public string MyType
    {
        get => type;
        set => type = value;
    }
    public int MyLevel
    {
        get => level;
        set => level = value;
    }

    protected virtual void Awake()
    {
        MyAnimator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// 移动
    /// </summary>
    public virtual void Move()
    {
        if (IsAlive)
        {
            //transform.Translate(direction * speed * Time.deltaTime);
            myRigidbdoy.velocity = Direction.normalized * Speed;
        }
    }

    /// <summary>
    /// 根据当前状态,切换动画层级,Idle,Walk,Attack
    /// </summary>
    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {
                ActiveLayer("WalkLayer");

                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);

            }
            else if (IsAttacking)
            {
                ActiveLayer("AttackLayer");
            }
            else
            {
                //如果没有移动则切换动画层权重到Idle
                ActiveLayer("IdleLayer");
            }
        }
        else
        {
            ActiveLayer("DeathLayer");
        }
        
    }

    /// <summary>
    /// 激活某动画层
    /// </summary>
    /// <param name="layerName"></param>
    public void ActiveLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }
        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

    public virtual void TakeDamage(float damage, Transform source)
    {
        //扣血
        health.MyCurrentValue -= damage;

        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE);
        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero;
            myRigidbdoy.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
            if (this is Enemy)
            {
                Player.MyInstance.GainXP(XPManager.CalculateXP(this as Enemy));
            }
        }
    }


    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(),SCTTYPE.HEAL);
    }
}
