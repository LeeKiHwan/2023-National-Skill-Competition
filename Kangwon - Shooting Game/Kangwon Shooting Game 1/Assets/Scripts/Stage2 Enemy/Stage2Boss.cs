using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Boss : BaseEnemy
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
            int attack = Random.Range(0, 2);

            switch(attack)
            {
                case 0:
                    StartCoroutine(AllAttack());
                    break;
                case 1:
                    StartCoroutine(SpreadAttack());
                    break;
            }

            attackCurTime = attackCoolTime;
        }
    }

    public IEnumerator AllAttack()
    {
        for (int i=0; i<5; i++)
        {
            for (float j = 90 / 2; j <= 270 / 2; j += 10)
            {
                GameObject b = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, j)));
                b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
            }
            yield return new WaitForSeconds(0.2f);
        }

        yield break;
    }

    public IEnumerator SpreadAttack()
    {
        for (int i=0; i<30; i++)
        {
            GameObject b = Instantiate(bullet, transform.position, Quaternion.Euler(new Vector3(0, 0, Random.Range(100, 260))));
            b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed / 2);
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }

    public override void Die()
    {
        InGameManager.Instance.StageClear();
        Destroy(gameObject);
    }

    public override void Move()
    {
        if (transform.position.y > 5) transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
}
