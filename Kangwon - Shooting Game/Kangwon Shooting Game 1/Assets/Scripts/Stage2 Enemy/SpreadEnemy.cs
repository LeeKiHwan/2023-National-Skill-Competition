using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEnemy : BaseEnemy
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
            StartCoroutine(SpreadFire());
            attackCurTime = attackCoolTime;
        }
    }

    public IEnumerator SpreadFire()
    {
        for (int j = 0; j < 10; j++)
        {
            Vector2 target = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            Vector2 dir = target - (Vector2)transform.position;

            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
            b.transform.up = dir.normalized;
            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);

            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }

    public override void Move()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
