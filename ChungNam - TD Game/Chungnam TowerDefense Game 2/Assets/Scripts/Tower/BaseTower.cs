using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTower : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public int price;

    [Header("Attackable")]
    public bool onAtkRange;
    public float damage;
    public float atkRange;
    public float atkCool;
    public float atkCur;
    public bool isAttackSpeed;
    public GameObject attackSpeedEffect;


    private void Update()
    {
        if (atkCur > 0) atkCur -= Time.deltaTime;
        else if (atkCur <= 0 && CanAttack() && EnemysInAtkRange().Length > 0 && EnemysInAtkRange()[0])
        {
            Attack();
            atkCur = isAttackSpeed ? atkCool * 0.5f : atkCool;
        }

        if (attackSpeedEffect)
        {
            attackSpeedEffect.SetActive(isAttackSpeed);
        }
    }

    public abstract void Attack();

    public bool CanAttack()
    {
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy")).Length > 0;
    }

    public void TakeDamage(float damage)
    {
        if (hp - damage > 0) hp -= damage;
        else Die();
    }

    public Collider[] EnemysInAtkRange()
    {
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy"));
    }

    public virtual void Die()
    {
        hp = 0;
        Destroy(gameObject);
    }

    public void TakeHeal(float heal)
    {
        if (hp + heal < maxHp) hp += heal;
        else hp = maxHp;
    }

    private void OnDrawGizmos()
    {
        if (onAtkRange)
        {
            Gizmos.color = new Color(0, 1, 0, 0.1f);
            Gizmos.DrawSphere(transform.position, atkRange);
        }
    }
}
