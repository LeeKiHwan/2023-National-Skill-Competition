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
        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy"));

        if (monsters.Length > 0)
        {
            turret.LookAt(new Vector3(monsters[0].transform.position.x, 1, monsters[0].transform.position.z));
            Instantiate(bullet, muzzlePos.position, Quaternion.identity).GetComponent<Bullet>().SetTarget(monsters[0].gameObject, bulletSpeed);
            monsters[0].GetComponent<BaseEnemy>().TakeDamage(damage, this);
        }
    }
}
