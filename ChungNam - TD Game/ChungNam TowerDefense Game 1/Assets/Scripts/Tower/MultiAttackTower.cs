using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackTower : BaseTower
{
    [Header("Basic Tower Data")]
    public float fireRate;
    public GameObject bullet;
    public float bulletSpeed;
    public Transform muzzlePos;

    private void Update()
    {
        AttackableTimer();
    }

    public override void Attack()
    {
        StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        Collider[] monsters = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy"));
        int cnt = monsters.Length > 5 ? 5 : monsters.Length;

        for (int i = 0; i < cnt; i++)
        {
            turret.LookAt(new Vector3(monsters[i].transform.position.x, transform.position.y, monsters[i].transform.position.z));
            Instantiate(bullet, muzzlePos.position, Quaternion.identity).GetComponent<Bullet>().SetTarget(monsters[i].gameObject, bulletSpeed);
            monsters[i].GetComponent<BaseEnemy>().TakeDamage(damage, this);
            yield return new WaitForSeconds(fireRate);
        }

        yield break;
    }
}
