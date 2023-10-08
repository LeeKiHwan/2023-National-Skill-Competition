using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackTower : BaseTower
{
    public Transform turret;
    public Transform muzzle;
    public GameObject bullet;

    public override void Attack()
    {
        BaseEnemy enemy = EnemysInAtkRange()[0].GetComponent<BaseEnemy>();
        Vector3 enemyPos = new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z);

        turret.LookAt(enemyPos);
        GameObject b = Instantiate(bullet, muzzle.position, Quaternion.identity);
        b.transform.LookAt(enemyPos);

        enemy.TakeDamage(damage, this);
    }
}
