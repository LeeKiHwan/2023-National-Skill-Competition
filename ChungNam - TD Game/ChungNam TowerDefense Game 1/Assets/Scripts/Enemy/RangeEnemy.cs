using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemy : BaseEnemy
{
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AttackableTimer();
    }

    public override void Attack()
    {
        if (targetTower != null)
        {
            targetTower.TakeDamage(damage);
            anim.SetTrigger("Attack");
        }
    }
}
