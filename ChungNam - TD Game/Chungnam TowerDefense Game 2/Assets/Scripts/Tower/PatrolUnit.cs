using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolUnit : MonoBehaviour
{
    [Header("Movable")]
    public NavMeshAgent agent;
    public float moveCool;
    public float moveCur;

    [Header("Attackable")]
    public Transform muzzle;
    public GameObject bullet;
    public BaseEnemy targetEnemy;
    public bool onAtkRange;
    public float damage;
    public float atkRange;
    public float atkCool;
    public float atkCur;
    public AudioClip attackSFX;

    private void Update()
    {
        agent.isStopped = CanAttack();

        if (moveCur > 0) moveCur -= Time.deltaTime;
        else if (moveCur <= 0 && FindObjectOfType<BaseEnemy>())
        {
            targetEnemy = FindObjectOfType<BaseEnemy>();
            agent.SetDestination(targetEnemy.transform.position);
            moveCur = moveCool;
        }

        if (atkCur > 0) atkCur -= Time.deltaTime;
        else if (atkCur <= 0 && CanAttack() && EnemysInAtkRange().Length > 0 && EnemysInAtkRange()[0])
        {
            Attack();
            atkCur = atkCool;
        }
    }

    public void Attack()
    {
        SoundManager.Instance.PlaySFX(attackSFX, 0.3f);

        BaseEnemy enemy = EnemysInAtkRange()[0].GetComponent<BaseEnemy>();
        Vector3 enemyPos = new Vector3(enemy.transform.position.x, 1, enemy.transform.position.z);

        GameObject b = Instantiate(bullet, muzzle.position, Quaternion.identity);
        b.transform.LookAt(enemyPos);

        enemy.TakeDamage(damage, null);
    }

    public bool CanAttack()
    {
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy")).Length > 0;
    }

    public Collider[] EnemysInAtkRange()
    {
        return Physics.OverlapSphere(transform.position, atkRange, 1 << LayerMask.NameToLayer("Enemy"));
    }

    private void OnDrawGizmos()
    {
        if (onAtkRange)
        {
            Gizmos.color = new Color(1,0,0,0.1f);
            Gizmos.DrawSphere(transform.position, atkRange);
        }
    }
}
