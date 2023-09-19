using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public int damage;
    public float durTime;
    public float tickTime;

    private void Awake()
    {
        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BaseEnemy>().TickDamage(damage, durTime, tickTime);
            Destroy(gameObject);
        }
    }
}
