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
        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Monster"));

        if (monsters.Length > 0)
        {
            turret.LookAt(new Vector3(monsters[0].transform.position.x, transform.position.y, monsters[0].transform.position.z));
            monsters[0].GetComponent<BaseEnemy>().TakeDamage(damage, this);
        }
    }
}
