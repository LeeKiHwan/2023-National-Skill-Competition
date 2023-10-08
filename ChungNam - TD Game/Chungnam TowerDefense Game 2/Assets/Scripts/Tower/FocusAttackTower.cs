using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FocusAttackTower : BaseTower
{
    public float atkArea;
    public Transform muzzle;
    public GameObject bullet;
    public AudioClip attackSFX;

    public override void Attack()
    {
        SoundManager.Instance.PlaySFX(attackSFX, 0.3f);
        Vector3 targetPos = EnemysInAtkRange()[0].transform.position;
        Collider[] enemys = Physics.OverlapSphere(targetPos, atkArea, 1 << LayerMask.NameToLayer("Enemy"));

        Vector3 enemyPos = new Vector3(targetPos.x, 1, targetPos.z);
        GameObject b = Instantiate(bullet, muzzle.position, Quaternion.identity);
        b.transform.LookAt(enemyPos);

        foreach (Collider enemy in enemys)
        {
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage, this);
        }
    }
}
