using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : BaseEnemy
{
    public float moveCoolTime;

    public void Awake()
    {
        StartCoroutine(MoveCo());
    }

    public override void Attack()
    {
    }

    public override void Move()
    {
    }

    public IEnumerator MoveCo()
    {
        while (true)
        {
            Vector2 targetPos = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            Vector2 dir = targetPos - (Vector2)transform.position;

            GetComponent<Rigidbody2D>().AddForce(dir.normalized * speed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(moveCoolTime);
        }
    }
}
