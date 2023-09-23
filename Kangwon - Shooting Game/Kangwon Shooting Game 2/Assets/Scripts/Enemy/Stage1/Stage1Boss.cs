using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Stage1Boss : BaseEnemy
{
    [Header("Atk")]
    public float atkCool;
    public GameObject bullet;
    public float bulletSpeed;
    public GameObject dashIdc;
    public bool isDash;
    public float dashForce;
    public GameObject enemy;

    [Header("Die")]
    public bool isDie;
    public GameObject spaceShip;

    private void Awake()
    {
        StartCoroutine(AttackCO());
    }

    public override void Move()
    {
        if (!isDash)
        {
            Vector2 target = PlayerAttackManager.Instance.player.transform.position;
            Vector2 dir = target - (Vector2)transform.position;

            transform.up = dir.normalized;
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!isDie)
        {
            base.TakeDamage(damage);
        }
    }

    public override void Attack()
    {
    }

    public IEnumerator AttackCO()
    {
        while (!isDie)
        {
            int atk = Random.Range(0, 4);

            switch (atk)
            {
                case 0:
                    for (int i=0; i<5; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            Vector2 target = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
                            Vector2 dir = target - (Vector2)transform.position;

                            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                            b.transform.up = dir.normalized;
                            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                        }
                        yield return new WaitForSeconds(0.2f);
                    }
                    break;
                case 1:
                    isDash = true;
                    Vector2 dashTarget = PlayerAttackManager.Instance.player.transform.position;
                    Vector2 dashDir = dashTarget - (Vector2)transform.position;

                    dashIdc.SetActive(true);
                    dashIdc.transform.up = dashDir.normalized;
                    yield return new WaitForSeconds(0.5f);
                    GetComponent<Rigidbody2D>().AddForce(dashDir.normalized * dashForce, ForceMode2D.Impulse);
                    dashIdc.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    isDash = false;
                    break;
                case 2:
                    for (int i=0; i<5; i++)
                    {
                        Vector2 spawnPos = new Vector2(Random.Range(-5,5), Random.Range(-5,5)) + (Vector2)transform.position;
                        Instantiate(enemy, spawnPos, Quaternion.identity);
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
            }

            yield return new WaitForSeconds(atkCool);
        }
    }

    public override void Die()
    {
        isDie = true;
        StartCoroutine(DieCo());
        StartCoroutine(DieMoveCo());
        StartCoroutine(DieEffectCo());
    }

    public IEnumerator DieCo()
    {
        while (sr.color.a > 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime / 5));
            yield return null;
        }

        Instantiate(spaceShip, transform.position, Quaternion.identity);
        Destroy(gameObject);

        yield break;
    }

    public IEnumerator DieMoveCo()
    {
        while (true)
        {
            Vector2 pos = transform.position;
            transform.position = pos + new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            yield return new WaitForSeconds(0.1f);
            transform.position = pos;
            yield return null;
        }
    }

    public IEnumerator DieEffectCo()
    {
        while (true)
        {
            Instantiate(dieEffect, transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2)), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
