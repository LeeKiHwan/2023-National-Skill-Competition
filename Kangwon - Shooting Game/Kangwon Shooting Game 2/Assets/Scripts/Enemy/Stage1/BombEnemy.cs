using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemy : BaseEnemy
{
    [Header("Bomb Enemy")]
    public GameObject bombBullet;

    public override void Attack()
    {
    }

    public override void Move()
    {
        Vector2 target = PlayerAttackManager.Instance.player.transform.position;
        Vector2 dir = target - (Vector2)transform.position;

        transform.up = dir.normalized;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public override void Die()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 target = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            Vector2 dir = target - (Vector2)transform.position;

            GameObject bullet = Instantiate(bombBullet, transform.position, Quaternion.identity);
            bullet.transform.up = dir.normalized;
            bullet.GetComponent<Bullet>().SetBulletStatus(damage, 10);
        }

        base.Die();
    }
}
