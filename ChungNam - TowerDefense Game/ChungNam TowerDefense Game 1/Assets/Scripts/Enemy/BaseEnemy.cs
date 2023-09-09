using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator anim;

    [Header("Status Data")]
    public int hp;
    public float speed;
    public int dropGold;

    [Header("Attackable Data")]
    public BaseTower targetTower;
    public int damage;
    public float atkRange;
    public float atkCoolTime;
    public float atkCurTime;
    public bool isAttacked;

    public void AttackableTimer()
    {
        if (CanAttack()) agent.isStopped = true;
        else agent.isStopped = false;
        anim.SetBool("isMove", !agent.isStopped);

        if (atkCurTime > 0) atkCurTime -= Time.deltaTime;
        else if (atkCurTime <= 0 && CanAttack())
        {
            Attack();
            atkCurTime = atkCoolTime;
        }

        if (targetTower == null)
        {
            SetTargetProtectFacility();
            if (isAttacked) isAttacked = false;
        }
    }
    public bool CanAttack()
    {
        Collider[] checkedTowers = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Tower"));

        foreach (Collider checkedTower in checkedTowers)
        {
            if (checkedTower.GetComponent<BaseTower>() == targetTower) return true;
        }

        return false;
    }
    public abstract void Attack();
    public void TakeDamage(int damage, BaseTower attackTower)
    {
        if (hp - damage <= 0) Die();
        else
        {
            hp -= damage;
            if (!isAttacked && !targetTower.GetComponent<IllusionTower>())
            {
                SetTarget(attackTower);
                isAttacked = true;
            }
        }
    }

    public void SetTargetProtectFacility()
    {
        BaseTower targetTower = null;

        if (FindObjectsOfType<IllusionTower>().Length > 0)
        {
            IllusionTower[] findIllusionTowers = FindObjectsOfType<IllusionTower>();

            float dis = 0;
            dis = Vector3.Distance(transform.position, findIllusionTowers[0].transform.position);
            targetTower = findIllusionTowers[0];

            for (int i=1; i<findIllusionTowers.Length; i++)
            {
                if (Vector3.Distance(transform.position, findIllusionTowers[i].transform.position) < dis)
                {
                    dis = Vector3.Distance(transform.position, findIllusionTowers[i].transform.position);
                    targetTower = findIllusionTowers[i];
                }
            }
        }
        else if (FindObjectsOfType<ProtectFacility>().Length > 0)
        {
            ProtectFacility[] findProtectFacility = FindObjectsOfType<ProtectFacility>();

            float dis = 0;
            dis = Vector3.Distance(transform.position, findProtectFacility[0].transform.position);
            targetTower = findProtectFacility[0];

            for (int i = 1; i < findProtectFacility.Length; i++)
            {
                if (Vector3.Distance(transform.position, findProtectFacility[i].transform.position) < dis)
                {
                    dis = Vector3.Distance(transform.position, findProtectFacility[i].transform.position);
                    targetTower = findProtectFacility[i];
                }
            }
        }
        else if (FindObjectOfType<FinalProtectFacility>())
        {
            targetTower = FindObjectOfType<FinalProtectFacility>();
        }

        SetTarget(targetTower);
    }

    public void SetTarget(BaseTower targetTower)
    {
        this.targetTower = targetTower;
        if (targetTower != null)
        {
            agent.SetDestination(targetTower.transform.position);
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, atkRange);
    }
}
