using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttackTower : BaseTower
{
    [Header("FocusAttackTower Data")]
    public float atkAreaSize;

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
            Collider[] monstersInAttackArea = Physics.OverlapSphere(monsters[0].transform.position, atkAreaSize, 1 << LayerMask.NameToLayer("Monster"));

            int cnt = monstersInAttackArea.Length > 3 ? 3 : monstersInAttackArea.Length;

            for (int i = 0; i < cnt; i++)
            {
                monstersInAttackArea[i].GetComponent<BaseEnemy>().TakeDamage(damage, this);
            }
        }
    }
}
