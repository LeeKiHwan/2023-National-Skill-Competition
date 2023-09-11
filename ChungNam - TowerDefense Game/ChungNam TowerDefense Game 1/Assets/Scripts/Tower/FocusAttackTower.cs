using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusAttackTower : BaseTower
{
    [Header("FocusAttackTower Data")]
    public float atkAreaSize;
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
            turret.LookAt(new Vector3(monsters[0].transform.position.x, transform.position.y, monsters[0].transform.position.z));
            Instantiate(bullet, muzzlePos.position, Quaternion.identity).GetComponent<Bullet>().SetTarget(monsters[0].gameObject, bulletSpeed);
            Collider[] monstersInAttackArea = Physics.OverlapSphere(monsters[0].transform.position, atkAreaSize, 1 << LayerMask.NameToLayer("Enemy"));

            foreach (Collider monster in monstersInAttackArea)
            {
                monster.GetComponent<BaseEnemy>().TakeDamage(damage, this);
            }
        }
    }
}
