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
            turret.LookAt(new Vector3(monsters[0].transform.position.x, transform.position.y, monsters[0].transform.position.z));
            Collider[] monstersInAttackArea = Physics.OverlapSphere(monsters[0].transform.position, atkAreaSize, 1 << LayerMask.NameToLayer("Monster"));

            foreach (Collider monster in monstersInAttackArea)
            {
                monster.GetComponent<BaseEnemy>().TakeDamage(damage, this);
            }
        }
    }
}
