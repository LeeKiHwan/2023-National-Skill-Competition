using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : BaseTower
{
    private void Awake()
    {
        foreach(Collider enemy in EnemysInAtkRange())
        {
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage, null);
        }
    }

    public override void Attack()
    {
    }
}
