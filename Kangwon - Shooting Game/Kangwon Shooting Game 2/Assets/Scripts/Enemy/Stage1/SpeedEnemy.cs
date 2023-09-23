using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedEnemy : BaseEnemy
{
    public override void Attack()
    {
    }

    public override void Move()
    {
        speed = PlayerAttackManager.Instance.player.speed * 1.5f;
        Vector2 target = PlayerAttackManager.Instance.player.transform.position;
        Vector2 dir = target - (Vector2)transform.position;

        transform.up = dir.normalized;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
