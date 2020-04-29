using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法类,挂载在Prefab上用于控制生成的魔法
/// </summary>
public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    /// <summary>
    /// 飞行速度
    /// </summary>
    [SerializeField]
    private float speed;

    /// <summary>
    /// 飞向的目标
    /// </summary>
    public Transform Mytarget
    {
        get; private set;
    }

    private Transform source;

    private int damage;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Transform target,int damage,Transform source)
    {
        this.Mytarget = target;
        this.damage = damage;
        this.source = source;
    }

    private void FixedUpdate()
    {
        if (Mytarget != null)
        {
            //方向向量
            Vector2 direction = Mytarget.position - transform.position;
            //给予一个力
            myRigidBody.velocity = direction.normalized * speed;
            //旋转的角度
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果碰到的tag 是HitBox 并且 是目标的话执行
        if (collision.tag == "HitBox" && collision.transform == Mytarget)
        {
            Character c = collision.GetComponentInParent<Character>();
            speed = 0;
            c.TakeDamage(damage,source);
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero;
            Mytarget = null;
        }
    }
}
