using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class Stage3Boss : BaseEnemy
{
    public Animator anim;
    public GameObject body;

    public float atkCool;
    public GameObject slash;
    public float slashSpeed;
    public GameObject blackFire;
    public GameObject ironShot;
    public float ironShotSpeed;
    public bool isDie;

    private void Awake()
    {
        StartCoroutine(AttackCo());
    }

    public override void Attack()
    {
    }

    public IEnumerator AttackCo()
    {
        while (!isDie)
        {
            int atkIdx = Random.Range(0, 3);

            switch (atkIdx)
            {
                case 0:
                    anim.SetTrigger("Attack");
                    for (int i=0; i<30; i++)
                    {
                        Vector2 dir = PlayerAttackManager.Instance.player.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f)) - transform.position;
                        GameObject b = Instantiate(slash, transform.position, Quaternion.identity);
                        b.transform.up = dir.normalized;
                        b.GetComponent<Bullet>().SetBulletStatus(damage, slashSpeed + Random.Range(-2,2));
                        yield return new WaitForSeconds(0.05f);
                    }
                    yield return new WaitForSeconds(0.75f);
                    Vector2 iDir = PlayerAttackManager.Instance.player.transform.position - transform.position;
                    GameObject ishot = Instantiate(ironShot, transform.position, Quaternion.identity);
                    ishot.transform.up = iDir.normalized;
                    ishot.GetComponent<Bullet>().SetBulletStatus(damage * 2, ironShotSpeed);
                    break;
                case 1:
                    for (int i=0;i<3;i++)
                    {
                        anim.SetTrigger("Attack");
                        for (int j=0; j<=360; j+=40)
                        {
                            Instantiate(ironShot, transform.position, Quaternion.Euler(new Vector3(0,0,j))).GetComponent<Bullet>().SetBulletStatus(damage*2, ironShotSpeed);
                        }
                        yield return new WaitForSeconds(0.25f);
                        for (int j = 20; j <= 320; j += 40)
                        {
                            Instantiate(ironShot, transform.position, Quaternion.Euler(new Vector3(0, 0, j))).GetComponent<Bullet>().SetBulletStatus(damage * 2, ironShotSpeed);
                        }
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
                case 2:
                    anim.SetTrigger("Attack");
                    for (int i=0; i<10; i++)
                    {
                        Vector2 firePos = PlayerAttackManager.Instance.player.transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
                        Instantiate(blackFire, firePos, Quaternion.identity);
                        yield return new WaitForSeconds(0.5f);
                        Collider2D[] player = Physics2D.OverlapCircleAll(firePos, 1);
                        foreach (Collider2D col in player)
                        {
                            if (col.GetComponent<Player>())
                            {
                                col.GetComponent<Player>().TakeDamage(damage);
                            }
                        }
                        yield return new WaitForSeconds(0.05f);
                    }
                    break;
            }
            yield return new WaitForSeconds(atkCool);
        }
    }

    public override void Move()
    {
        if (!isDie)
        {
            if (PlayerAttackManager.Instance.player.transform.position.x - transform.position.x < 0)
            {
                body.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                body.transform.rotation = Quaternion.identity;
            }

            Vector2 dir = PlayerAttackManager.Instance.player.transform.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime);
        }
    }

    public override void Die()
    {
        isDie = true;
        StartCoroutine(DieCo());
    }

    public IEnumerator DieCo()
    {
        anim.SetTrigger("IsDie");
        yield return new WaitForSeconds(10f);
        InGameManager.Instance.StageClear();

        yield break;
    }
}
