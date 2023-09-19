using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AssualtEnemy : BaseEnemy
{
    public GameObject bullet;
    public float attackCoolTime;
    public float attackCurTime;
    public float bulletSpeed;

    public override void Attack()
    {
        if (attackCurTime > 0) attackCurTime -= Time.deltaTime;

        if (attackCurTime <= 0)
        {
            StartCoroutine(AssualtFire());
            attackCurTime = attackCoolTime;
        }
    }

    public IEnumerator AssualtFire()
    {
        for (int i = 0; i<3; i++)
        {
            Vector2 target = PlayerAttackManager.Instance.player.transform.position;
            Vector2 dir = target - (Vector2)transform.position;

            GameObject b =  Instantiate(bullet, transform.position, Quaternion.identity);
            b.transform.up = dir.normalized;
            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }

    public override void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
