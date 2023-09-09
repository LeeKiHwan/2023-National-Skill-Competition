using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : BaseEnemy
{
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
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
        }
    }
}
