using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int hp;
    public int damage;
    public float speed;

    public abstract void Move();
    public abstract void Attack();
    public virtual void TakeDamage(int damage)
    {
        if (hp - damage > 0) hp -= damage;
        else Die();
    }
    public abstract void Die();
}
