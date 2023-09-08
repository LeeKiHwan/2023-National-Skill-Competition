using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public bool onDrawGizmos;

    [Header("Tower Data")]
    public int hp;
    public int price;

    [Header("Attackable Data")]
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
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Monster")).Length > 0;
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
