using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [Header("Player")]
    public Rigidbody2D rb;
    public int maxHp;
    public int maxMp;
    public int mp;
    public float invcTime;
    public GameObject invcShield;

    private void Awake()
    {
        StartCoroutine(GetMpCo());
    }

    private void Update()
    {
        if (invcTime > 0)
        {
            invcTime -= Time.deltaTime;
            invcShield.SetActive(true);
        }
        else
        {
            invcShield.SetActive(false);
        }

        Move();
    }

    public override void Attack()
    {
    }

    public override void TakeDamage(int damage)
    {
        if (invcTime <= 0)
        {
            base.TakeDamage(damage);
        }
    }

    public override void Die()
    {
    }

    public override void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(x, y);
    }

    public void SetInvcTime(float setTime)
    {
        if (invcTime < setTime) invcTime = setTime;
    }
    public void TakeHeal(int heal)
    {
        if (hp + heal > maxHp) hp = maxHp;
        else hp += heal;
    }

    public void SpeedUp(float speedUpValue, float time)
    {
        StartCoroutine(SpeedUpCo(speedUpValue, time));
    }
    public IEnumerator SpeedUpCo(float speedUpValue, float time)
    {
        speed += speedUpValue;
        yield return new WaitForSeconds(time);
        speed -= speedUpValue;

        yield break;
    }

    public void GetMp(int getMp)
    {
        if (mp + getMp >= maxMp) mp = maxMp;
        else mp += getMp;
    }

    public IEnumerator GetMpCo()
    {
        while (true)
        {
            GetMp(1);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
