using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunEnemy : BaseEnemy
{
    public GameObject bullet;
    public float bulletSpeed;
    public float atkCool;

    private void Awake()
    {
        StartCoroutine(FirstMoveCo());
        StartCoroutine(AttackCo());
    }

    public override void Attack()
    {
    }

    public IEnumerator AttackCo()
    {
        while (true)
        {
            for (int i=0; i<2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Vector2 dir = PlayerAttackManager.Instance.player.transform.position + new Vector3(Random.Range(-3,3), Random.Range(-3,3)) - transform.position;
                    GameObject b = Instantiate(bullet, transform.position, Quaternion.identity);
                    b.transform.up = dir.normalized;
                    b.GetComponent<Bullet>().SetBulletStatus(damage, bulletSpeed);
                }
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(atkCool);
        }
    }

    public override void Move()
    {

    }

    public IEnumerator FirstMoveCo()
    {
        while (transform.position.y > 3.5f)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(MoveCo());

        yield break;
    }

    public IEnumerator MoveCo()
    {
        int yDir = -1;

        while (true)
        {
            float r = Random.Range(-3f, 3f);

            if (transform.position.x + r < -8 || transform.position.x + r > 8) yield return null;
            else
            {
                Vector3 targetPos = new Vector2(transform.position.x + r, transform.position.y + yDir);
                while (transform.position != targetPos)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                    yield return null;
                }
                yDir *= -1;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
