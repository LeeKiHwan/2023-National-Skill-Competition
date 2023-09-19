using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Stage1Boss : BaseEnemy
{
    [Header("Attack")]
    public Rigidbody2D rb;
    public float attackCoolTime;
    public float attackCurTime;
    public GameObject bullet;
    public float dashForce;
    public GameObject[] spawnEnemys;

    public override void Attack()
    {
        if (attackCurTime > 0) attackCurTime -= Time.deltaTime;

        if (attackCurTime <= 0)
        {
            int attack = Random.Range(0, 3);

            switch (attack)
            {
                case 0:
                    StartCoroutine(Fire());
                    break;
                case 1:
                    StartCoroutine(Dash());
                    break;
                case 2:
                    StartCoroutine(SpawnEnemy());
                    break;
            }

            attackCurTime = attackCoolTime;
        }
    }

    public override void Move()
    {
        Vector2 target = PlayerAttackManager.Instance.player.transform.position;
        Vector2 dir = target - (Vector2)transform.position;

        transform.up = dir.normalized;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator Fire()
    {
        for (int i=0; i<5; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Vector2 target = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                Vector2 dir = target - (Vector2)transform.position;

                GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
                bullet.transform.up = dir.normalized;
            }
            yield return new WaitForSeconds(0.25f);
        }

        yield break;
    }

    public IEnumerator Dash()
    {
        Vector2 dashTarget = PlayerAttackManager.Instance.player.transform.position;
        Vector2 dashDir = dashTarget - (Vector2)transform.position;
        rb.AddForce(dashDir.normalized * dashForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(1);
        rb.velocity = Vector2.zero;
    }

    public IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < 10; i++)
        {
            int idx = Random.Range(0, spawnEnemys.Length);
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

            Instantiate(spawnEnemys[idx], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
        yield break;
    }

    public override void Die()
    {
        InGameManager.Instance.StageClear();
        Destroy(gameObject);
    }
}
