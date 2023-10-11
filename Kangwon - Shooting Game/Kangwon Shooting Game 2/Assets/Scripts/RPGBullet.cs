using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBullet : Bullet
{
    public Vector2 atkArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && bulletType == BulletType.Player)
        {
            Collider2D[] enemys = Physics2D.OverlapBoxAll(transform.position, atkArea, 0);

            foreach(Collider2D enemy in enemys)
            {
                if (enemy.GetComponent<BaseEnemy>())
                {
                    enemy.GetComponent<BaseEnemy>().TakeDamage(damage);
                }
            }

            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, atkArea);
    }
}
