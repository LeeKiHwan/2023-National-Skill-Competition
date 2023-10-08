using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [Header("Status")]
    public float hp;
    public int dropGold;
    public NavMeshAgent agent;
    public bool isAttacked;
    public float baseSpeed;
    public bool isSlow;
    public GameObject slowEffect;
    public bool isFire;
    public GameObject fireEffect;

    [Header("Attackable")]
    public bool onAtkRange;
    public BaseTower target;
    public float damage;
    public float atkRange;
    public float atkCool;
    public float atkCur;

    [Header("Die")]
    public GameObject dieEffect;

    private void Awake()
    {
        baseSpeed = agent.speed;
        SetTarget();
    }

    private void Update()
    {
        agent.speed = isSlow ? baseSpeed * 0.5f : baseSpeed;

        agent.isStopped = CanAttack();

        if (atkCur > 0) atkCur -= Time.deltaTime;
        else if (CanAttack() && target)
        {
            Attack();
            atkCur = atkCool;
        }

        if (!target && UIManager.Instance.pf)
        {
            SetTarget();
            isAttacked = false;
        }

        slowEffect.SetActive(isSlow);
        fireEffect.SetActive(isFire);
    }

    public void Attack()
    {
        target.TakeDamage(damage);
    }

    public bool CanAttack()
    {
        Collider[] towers = Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Tower"));
        foreach (Collider tower in towers)
        {
            if (target && tower.GetComponent<BaseTower>() == target)
            {
                return true;
            }
        }
        return false;
    }

    public void SetTarget()
    {
        List<IllusionTower> illusionTowers = new List<IllusionTower>(FindObjectsOfType<IllusionTower>());
        illusionTowers = illusionTowers.OrderByDescending(i => Vector3.Distance(transform.position, i.transform.position)).ToList();
        if (illusionTowers.Count > 0)
        {
            SetTarget(illusionTowers[0]);
            return;
        }

        List<ProtectFacility> protectFacilities = new List<ProtectFacility>(FindObjectsOfType<ProtectFacility>());
        protectFacilities = protectFacilities.OrderByDescending(i => Vector3.Distance(transform.position, i.transform.position)).ToList();
        if (protectFacilities.Count > 0)
        {
            SetTarget(protectFacilities[0]);
            return;
        }

        List<FinalProtectFacility> finalProtectFacilities = new List<FinalProtectFacility>(FindObjectsOfType<FinalProtectFacility>());
        finalProtectFacilities = finalProtectFacilities.OrderByDescending(i => Vector3.Distance(transform.position, i.transform.position)).ToList();
        if (finalProtectFacilities.Count > 0)
        {
            SetTarget(finalProtectFacilities[0]);
            return;
        }
    }

    public void SetTarget(BaseTower target)
    {
        this.target = target;
        agent.SetDestination(target.transform.position);
    }

    public void TakeDamage(float damage, BaseTower atkTower)
    {
        if (hp - damage > 0)
        {
            hp -= damage;
            if (!isAttacked && target && !target.GetComponent<IllusionTower>() && atkTower)
            {
                SetTarget(atkTower);
                isAttacked = true;
            }
        }
        else Die();
    }

    public void Die()
    {
        GameManager.Instance.CheckEnemy();
        Instantiate(dieEffect, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        TowerManager.Instance.GetGold(dropGold);
        GameManager.Instance.GetScore(dropGold * 100);
        Destroy(gameObject);
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
