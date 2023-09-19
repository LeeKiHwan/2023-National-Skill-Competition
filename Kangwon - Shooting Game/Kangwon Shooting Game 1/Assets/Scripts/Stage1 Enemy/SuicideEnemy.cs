using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemy : BaseEnemy
{
    [Header("Suicide Enemy")]
    public GameObject suicideBullet;

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
        int cnt = Random.Range(7, 13);

        for (int i = 0; i < cnt; i++)
        {
            Vector2 target = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            Vector2 dir = target - (Vector2)transform.position;

            GameObject bullet = Instantiate(suicideBullet, transform.position, Quaternion.identity);
            bullet.transform.up = dir.normalized;
        }

        base.Die();
    }
}
