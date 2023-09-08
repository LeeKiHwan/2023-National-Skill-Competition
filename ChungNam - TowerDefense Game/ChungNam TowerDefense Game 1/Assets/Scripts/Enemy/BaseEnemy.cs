using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public int hp;
    public int dropGold;
    public float speed;
    public int damage;
    public float atkCoolTime;
    public float atkCurTime;

    public abstract void Attack();
    public void TakeDamage(int damage)
    {
        Debug.Log("Damaged");

        if (hp - damage <= 0) Die();
        else hp -= damage;
    }
    public void Die()
    {
        Debug.Log("Die");
    }
}
