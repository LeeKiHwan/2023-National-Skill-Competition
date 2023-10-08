using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public bool onDrawGizmos;

    [Header("Tower Data")]
    public int hp;
    public int price;
    public Vector2 size;

    [Header("Attackable Data")]
    public Transform turret;
    public int damage;
    public float atkCoolTime;
    public float atkCurTime;
    public float atkRange;

    public abstract void Attack();
    public void AttackableTimer()
    {
        if (atkCurTime > 0) atkCurTime -= Time.deltaTime;
        else if (atkCurTime <= 0 && CanAttack())
        {
            Attack();
            atkCurTime = atkCoolTime;
        }
    }
    public bool CanAttack()
    {
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy")).Length > 0;
    }

    public void TakeDamage(int damage)
    {
        if (hp - damage <= 0) Die();
        else hp -= damage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (onDrawGizmos)
        {
            Gizmos.color = new Color(1, 0, 0, 0.1f);
            Gizmos.DrawSphere(transform.position, atkRange);
        }
    }
}
