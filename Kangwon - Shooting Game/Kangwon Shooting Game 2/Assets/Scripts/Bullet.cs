using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    public BulletType bulletType;

    public int damage;
    public float speed;
    public GameObject destroyEffect;

    private void Awake()
    {
        Destroy(gameObject,5);
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
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player") && bulletType == BulletType.Enemy)
        {
            collision.GetComponent<Unit>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
