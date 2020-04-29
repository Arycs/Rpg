using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character,IInteractable
{
    /// <summary>
    /// 血量变化事件
    /// </summary>
    public event HealthChanged healthChanged;
    /// <summary>
    /// 移除人物事件
    /// </summary>
    public event CharacterRemoved characterRemoved;

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private AStar astar;

    private IState currentState;

    [SerializeField]
    private LootTable lootTable;

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float MyAttackRange
    {
        get;set;
    }

    public float MyAttackTime
    {
        get;set;
    }

    public float MyAggroRange
    {
        get;set;
    }

    public Vector3 MyStartPosition
    {
        get;set;
    }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait
    {
        get
        {
            return portrait;
        }
    }



    [SerializeField]
    private float initAggroRange;

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }

    public AStar MyAstar
    {
        get => astar;
    }

    private void Awake()
    {
        MyAnimator = GetComponent<Animator>();
        health.Initialize(initHealth, initHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 1;
        ChangeState(new IdleState());
    }


    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();
        }

        base.Update();
    }

    /// <summary>
    /// 选中Enemy
    /// </summary>
    /// <returns></returns>
    public Transform Select()
    {
        healthGroup.alpha = 1;

        return hitBox;
    }

    /// <summary>
    /// 没选中Enemy
    /// </summary>
    /// <returns></returns>
    public void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UIManager.MyInstance.HideTargetFrame);
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage,Transform source)
    {
        if (!(currentState is EvadeState))
        {
            SetTarget(source);
            base.TakeDamage(damage, source);
            OnHealthChanged(health.MyCurrentValue);
        }

    }
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newState">要进入的状态</param>
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Transform target)
    {
        if (MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, target.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
        }
    }

    /// <summary>
    /// 初始化 Enemy
    /// </summary>
    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public  void Interact()
    {
        if (!IsAlive)
        {
            lootTable.ShowLoot();
        }
    }

    public  void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnHealthChanged(float health)
    {
        healthChanged?.Invoke(health);
    }

    public void OnCharacterRemoved()
    {
        characterRemoved?.Invoke();
        Destroy(gameObject);
    }

}
