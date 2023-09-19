using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : BaseEnemy
{
    public Vector2 targetPos;
    public float moveCoolTime;
    public float moveCurTime;

    public override void Update()
    {
        if (moveCurTime > 0) moveCurTime -= Time.deltaTime;

        base.Update();
    }

    public override void Attack()
    {
    }

    public override void Move()
    {
        if (moveCurTime <= 0)
        {
            targetPos = (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            moveCurTime = moveCoolTime;
        }

        Vector2 dir = targetPos - (Vector2)transform.position;

        transform.up = dir.normalized;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
