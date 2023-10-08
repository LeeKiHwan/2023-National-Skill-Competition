using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseTower
{
    bool isFire = true;

    private void Awake()
    {
        StartCoroutine(FireCo());
        StartCoroutine(TickDamage());
    }

    public IEnumerator FireCo()
    {
        yield return new WaitForSeconds(atkCool);
        isFire = false;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    public IEnumerator TickDamage()
    {
        while (true)
        {
            foreach(Collider enemy in EnemysInAtkRange())
            {
                enemy.GetComponent<BaseEnemy>().TakeDamage(damage, null);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void Attack()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BaseEnemy>())
        {
            other.GetComponent<BaseEnemy>().isFire = isFire;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseEnemy>())
        {
            other.GetComponent<BaseEnemy>().isFire = false;
        }
    }
}
