using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : Unit
{
    [Header("Base Enemy")]
    public bool isBoss;
    public GameObject dieEffect;
    public SpriteRenderer sr;
    public int dropXp;

    private void Update()
    {
        Move();
        Attack();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(TakeDamageCo());
    }

    public IEnumerator TakeDamageCo()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = Color.white;

        yield break;
    }

    public override void Die()
    {
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        PlayerAttackManager.Instance.GetXp(dropXp);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Unit>().TakeDamage(damage);
        }
        if (collision.CompareTag("DestroyBlock") && !isBoss)
        {
            Destroy(gameObject);
        }
    }
}
