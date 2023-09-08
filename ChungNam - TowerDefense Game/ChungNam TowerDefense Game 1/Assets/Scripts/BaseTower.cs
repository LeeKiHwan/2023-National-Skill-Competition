using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    [Header("Tower Data")]
    public int hp;
    public int price;

    [Header("Attackable Data")]
    public int damage;
    public float atkCoolTime;
    public float atkCurTime;

    public abstract void Attack();
    public void AttackableTimer()
    {
        if (atkCurTime > 0) atkCurTime = 0;
        else if (atkCurTime <= 0 && CanAttack())
        {
            Attack();
            atkCurTime = atkCoolTime;
        }
    }
    public abstract bool CanAttack();
}
