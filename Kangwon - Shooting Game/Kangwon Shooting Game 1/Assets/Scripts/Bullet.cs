using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Player,
        Enemy
    }

    public BulletType bulletType;
    public int damage;
    public float speed;
    public bool cantDestroy;

    private void Awake()
    {
        Destroy(gameObject, 5);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public void SetBulletStatus(int damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && bulletType == BulletType.Player)
        {
            collision.GetComponent<Unit>().TakeDamage(damage);
            if (!cantDestroy)
            {
                Destroy(gameObject);
            }
        }
        if (collision.CompareTag("Player") && bulletType == BulletType.Enemy)
        {
            collision.GetComponent<Unit>().TakeDamage(damage);
            if (!cantDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
