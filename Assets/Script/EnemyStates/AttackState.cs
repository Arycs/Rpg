using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AttackState : IState
{
    private Enemy parent;
    private float attackCooldown = 3;

    private float extraRange = 0.1f;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        if (parent.MyAttackTime >= attackCooldown && !parent.IsAttacking)
        {
            parent.MyAttackTime = 0;

            parent.StartCoroutine(Attack());
        }

        if (parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.position, parent.transform.position);

            if (distance >= parent.MyAttackRange + extraRange && ! parent.IsAttacking)
            {
                parent.ChangeState(new FollowState());
            }
            //We need to check range and attack

        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }

    public IEnumerator Attack()
    {
        parent.IsAttacking = true;

        parent.MyAnimator.SetTrigger("attack");

        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length);
        parent.MyTarget.GetComponent<Character>().MyHealth.MyCurrentValue -= 5;

        parent.IsAttacking = false;
    }
}