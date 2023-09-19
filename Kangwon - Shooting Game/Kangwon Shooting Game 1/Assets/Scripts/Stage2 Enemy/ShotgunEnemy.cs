using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEnemy : BaseEnemy
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
            StartCoroutine(ShotgunFire());
            attackCurTime = attackCoolTime;
        }
    }

    public IEnumerator ShotgunFire()
    {
        for (int i=0; i<3; i++)
        {
            for (int j=0; j<5; j++)
            {
                Vector2 target = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
                Vector2 dir = target - (Vector2)transform.position;

                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                b.transform.up = dir.normalized;
                b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);

                yield return new WaitForSeconds(0.5f);
            }
        }

        yield break;
    }

    public override void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
