using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackTower : BaseTower
{
    public Transform turret;
    public Transform muzzle;
    public GameObject bullet;

    public override void Attack()
    {
        int cnt = 0;
        foreach(Collider enemy in EnemysInAtkRange())
        {
            Vector3 enemyPos = new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z);

            if (cnt < 4)
            {
                if (cnt==0) turret.LookAt(enemyPos);

                GameObject b = Instantiate(bullet, muzzle.position, Quaternion.identity);
                b.transform.LookAt(enemyPos);

                enemy.GetComponent<BaseEnemy>().TakeDamage(damage, this);
                cnt++;
            }
        }
    }
}
