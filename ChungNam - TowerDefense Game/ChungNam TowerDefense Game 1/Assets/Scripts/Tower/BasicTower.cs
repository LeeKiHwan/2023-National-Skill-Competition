using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : BaseTower
{
    [Header("Basic Tower Data")]
    public GameObject bullet;
    public float bulletSpeed;
    public Transform muzzlePos;

    private void Update()
    {
        AttackableTimer();
    }

    public override void Attack()
    {
        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Monster"));

        if (monsters.Length > 0)
        {
            Instantiate(bullet, muzzlePos.position, Quaternion.identity).GetComponent<BasicTowerBullet>().SetTarget(monsters[0].GetComponent<BaseEnemy>(), bulletSpeed);
            turret.LookAt(new Vector3(monsters[0].transform.position.x, 1, monsters[0].transform.position.z));
            monsters[0].GetComponent<BaseEnemy>().TakeDamage(damage, this);
        }
    }
}
