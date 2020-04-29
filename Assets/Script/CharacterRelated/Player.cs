using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    private static Player instance;

    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }


    //血量和蓝量
    [SerializeField]
    private Stat mana;
    //初始化血量,蓝量值
    private float initMana = 50;
    [SerializeField]
    private Stat xpStat;

    [SerializeField]
    private Text levelText;

    //挡板,判断怪物是否在视野
    [SerializeField]
    private Block[] blocks;

    //魔法发射出的位置
    [SerializeField]
    private Transform[] exitPoints;

    [SerializeField]
    private Animator ding;

    private IInteractable interactable;

    // 魔法发射出去位置索引 0-up,1-right,2-down,3-left
    private int exitIndex = 2;

    #region PathFinding
    private Stack<Vector3> path;

    private Vector3 destination;

    private Vector3 current;

    private Vector3 goal;

    [SerializeField]
    private AStar astar;
    #endregion
    private Vector3 min, max;

    /// <summary>
    /// 金币数量
    /// </summary>
    public int MyGold
    {
        get; set;
    }
    public IInteractable MyInteractable
    {
        get => interactable;
        set => interactable = value;
    }
    public Stat MyXp
    {
        get => xpStat;
        set => xpStat = value;
    }
    public Stat MyMana
    {
        get => mana;
        set => mana = value;
    }

    protected void FixedUpdate()
    {
        Move();
    }

    public void SetDefaultValues()
    {
        MyGold = 100;
        MyMana.Initialize(initMana, initMana);
        //初始化经验条
        health.Initialize(initHealth, initHealth);
        MyXp.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f)));
        levelText.text = MyLevel.ToString();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y),transform.position.z);

        GetInput();
        //ClickToMove();
    }
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            MyHealth.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            GainXP(600);
        }

        //根据W,A,S,D来控制方向
        Direction = Vector2.zero;

        if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["UP"]))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["LEFT"]))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["DOWN"]))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeybindManager.MyInstance.KeyBinds["RIGHT"]))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }
        if (IsMoving)
        {
            StopAttack();
        }
        foreach (string action in KeybindManager.MyInstance.ActionBinds.Keys)
        {
            if (Input.GetKeyDown(KeybindManager.MyInstance.ActionBinds[action]))
            {
                UIManager.MyInstance.ClickActionButton(action);
            }
        }
    }

    /// <summary>
    /// 施法,Button中调用,用来攻击
    /// </summary>
    public void CastSpell(string spellName)
    {
        Block();

        if (MyTarget != null && MyTarget.GetComponentInParent<Character>().IsAlive &&!IsAttacking && !IsMoving && InLineOfSight())
        {
            attackRoutine = StartCoroutine(Attack(spellName));
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    //攻击
    public IEnumerator Attack(string spellName)
    {
        Transform currentTarget = MyTarget;

        Spell newSpell = SpellBook.MyInstance.CastSpell(spellName);

        IsAttacking = true;

        MyAnimator.SetBool("attack", IsAttacking);

        //等待三秒 为施法时间
        yield return new WaitForSecondsRealtime(newSpell.MyCastTime);
        //中途切换目标和目标丢失 判断
        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(currentTarget,newSpell.MyDamage,transform);
        }
        StopAttack();
    }

    /// <summary>
    /// 判断可视范围是否有可攻击敌人.
    /// </summary>
    /// <returns></returns>
    private bool InLineOfSight()
    {
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
            //绘制一条射线
            //Debug.DrawRay(transform.position, targetDirection, Color.red);

            //一条 与第256层Layer(Block层)相交的射线
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            //如果 没有碰到,说明可以攻击,不处在事业盲区
            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 显示范围的挡板
    /// </summary>
    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }
        blocks[exitIndex].Activate();
    }

    /// <summary>
    /// 停止攻击
    /// </summary>
    public void StopAttack()
    {
        SpellBook.MyInstance.StopCasting();
        IsAttacking = false;
        MyAnimator.SetBool("attack", IsAttacking);
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }

    public void Interact()
    {
        if (MyInteractable != null)
        {
            MyInteractable.Interact();
        }
    }

    public void GainXP(int xp)
    {
        MyXp.MyCurrentValue += xp;
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), SCTTYPE.XP);
        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    private IEnumerator Ding()
    {
        while (!MyXp.IsFull)
        {
            yield return null;
        }
        MyLevel++;
        ding.SetTrigger("ding");
        levelText.text = MyLevel.ToString();
        MyXp.MyMaxValue = Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f));
        MyXp.MyCurrentValue = MyXp.MyOverflow;
        MyXp.Reset();
        if (MyXp.MyCurrentValue >= MyXp.MyMaxValue)
        {
            StartCoroutine(Ding());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void GetPath(Vector3 goal)
    {
        Debug.Log(transform.position + "," + goal);
        path = astar.Algorithm(transform.position,goal);
        current = path.Pop();
        destination = path.Pop();
        this.goal = goal;
    }

    private void ClickToMove()
    {
        if (path !=null)
        {
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination,Speed * Time.deltaTime);

            Vector3Int dest = astar.MyTilemap.WorldToCell(destination);
            Vector3Int cur = astar.MyTilemap.WorldToCell(current);

            float distance = Vector2.Distance(destination, transform.parent.position);

            if (cur.y > dest.y)
            {
                Direction = Vector2.down;
            }
            else if (cur.y < dest.y)
            {
                Direction = Vector2.up;
            }
            if (cur.y == dest.y)
            {
                if (cur.x > dest.x)
                {
                    Direction = Vector2.left;
                }
                else if (cur.x < dest.x)
                {
                    Direction = Vector2.right;
                }
            }

            if (distance <= 0f)
            {
                if (path.Count > 0)
                {
                    current = destination;
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                }
            }
        }
    }

    public override void Move()
    {
        base.Move();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            MyInteractable = collision.GetComponent<IInteractable>();
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Interactable")
        {
            if (MyInteractable != null)
            {
                MyInteractable.StopInteract();
                MyInteractable = null;
            }
        }
    }
}
