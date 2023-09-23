using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Stage2Boss : BaseEnemy
{
    public GameObject bullet;
    public float bulletSpeed;
    public float atkCool;
    public bool isDie;

    private void Awake()
    {
        StartCoroutine(AttackCo());
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

    public IEnumerator AttackCo()
    {
        while (true)
        {
            int atkIdx = Random.Range(0, 4);

            switch (atkIdx)
            {
                case 0:
                    for (int i=0; i<3; i++)
                    {
                        for (int j=90; j <= 270; j+=20)
                        {
                            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                            b.transform.rotation = Quaternion.Euler(0, 0, j);
                            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                        }
                        yield return new WaitForSeconds(0.15f);
                        for (int j = 100; j <= 260; j += 20)
                        {
                            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                            b.transform.rotation = Quaternion.Euler(0, 0, j);
                            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                        }
                        yield return new WaitForSeconds(0.15f);
                    }
                    break;
                case 1:
                    for (int i=0; i<50; i++)
                    {
                        GameObject b = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, Random.Range(110, 250)));
                        b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed / 2 + Random.Range(-2f, 2f));
                    }
                    yield return new WaitForSeconds(1.5f);
                    for (int i=0; i<2; i++)
                    {
                        for (int j = 90; j <= 270; j += 30)
                        {
                            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                            b.transform.rotation = Quaternion.Euler(0, 0, j);
                            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                        }
                        yield return new WaitForSeconds(0.5f);
                    }
                    break;
                case 2:
                    for (int i=0; i<4; i++)
                    {
                        for (int j=0; j<3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                Vector3 dir = PlayerAttackManager.Instance.player.transform.position - transform.transform.position;
                                GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                                b.transform.up = dir.normalized;
                                b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                                if (k == 1) b.transform.rotation = Quaternion.Euler(0, 0, b.transform.eulerAngles.z - 15);
                                else if (k == 2) b.transform.rotation = Quaternion.Euler(0, 0, b.transform.eulerAngles.z + 15);
                            }
                            yield return new WaitForSeconds(0.1f);
                        }
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
                case 3:
                    int angle = 0;
                    for (int i=0;i<40;i++)
                    {
                        for (int j = 0 + i; j <= 360; j += 15)
                        {
                            GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                            b.transform.rotation = Quaternion.Euler(0, 0, j);
                            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed-3);
                        }
                        angle += 10;
                        yield return new WaitForSeconds(0.05f);
                    }
                    break;
            }

            yield return new WaitForSeconds(atkCool);
        }
    }

    public override void Move()
    {
        if (transform.position.y > 2)
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
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

        InGameManager.Instance.StageClear();
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
