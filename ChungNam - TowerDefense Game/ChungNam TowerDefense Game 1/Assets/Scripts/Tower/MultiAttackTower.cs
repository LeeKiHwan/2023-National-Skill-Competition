using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackTower : BaseTower
{
    private void Update()
    {
        AttackableTimer();
    }

    public override void Attack()
    {
        Debug.Log("Attack");

        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Monster"));
        int cnt = monsters.Length > 5 ? 5 : monsters.Length;

        for (int i = 0; i < cnt; i++)
        {
            monsters[i].GetComponent<BaseEnemy>().TakeDamage(damage);
        }
    }
}
