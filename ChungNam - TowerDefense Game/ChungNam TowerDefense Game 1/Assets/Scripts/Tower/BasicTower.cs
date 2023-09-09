using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : BaseTower
{
    private void Update()
    {
        AttackableTimer();
    }

    public override void Attack()
    {
        Debug.Log("Attack");

        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Monster"));

        if (monsters.Length > 0)
        {
            monsters[0].GetComponent<BaseEnemy>().TakeDamage(damage, this);
        }
    }
}
