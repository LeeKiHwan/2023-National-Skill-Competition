using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : Unit
{
    [Header("BaseEnemy")]
    public SpriteRenderer sr;
    public int dropXp;
    public float stunTime;

    public virtual void Update()
    {
        if (Vector2.Distance(PlayerAttackManager.Instance.player.transform.position, transform.position) > 20)
        {
            Destroy(gameObject);
        }

        if (stunTime > 0) stunTime -= Time.deltaTime;
        if (stunTime <= 0 && PlayerAttackManager.Instance.player)
        {
            Move();
            Attack();
        }
    }

    public override void TakeDamage(int damage)
    {
        StartCoroutine(TakeDamageCo());
        base.TakeDamage(damage);
    }

    public IEnumerator TakeDamageCo()
    {
        sr.color = new Color(1, 0, 0);
        yield return new WaitForSeconds(0.25f);
        sr.color = new Color(1, 1, 1);

        yield break;
    }

    public void TickDamage(int damage, float durTime, float tickTime)
    {
        StartCoroutine(TickDamageCo(damage, durTime, tickTime));
    }

    public IEnumerator TickDamageCo(int damage, float durTime, float tickTime)
    {
        float t = 0;
        while (t < durTime)
        {
            TakeDamage(damage);
            t += tickTime;
            yield return new WaitForSeconds(tickTime);
        }   

        yield break;
    }

    public void SetStunTime(float stunTime)
    {
        if (this.stunTime < stunTime) this.stunTime = stunTime;
    }

    public void SlowSpeed(float slowSpeed, float slowTime)
    {
        StartCoroutine(SlowSpeedCo(slowSpeed, slowTime));
    }

    public IEnumerator SlowSpeedCo(float slowSpeed, float slowTime)
    {
        if (speed - slowSpeed > 0) speed -= slowSpeed;
        yield return new WaitForSeconds(slowTime);
        speed += slowSpeed;

        yield break;
    } 


    public override void Die()
    {
        PlayerAttackManager.Instance.GetXp(dropXp);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Unit>().TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("DestroyBlock"))
        {
            Destroy(gameObject);
        }
    }
}
