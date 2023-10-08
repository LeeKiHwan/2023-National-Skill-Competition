using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeed : BaseTower
{
    bool isRage = true;

    private void Awake()
    {
        StartCoroutine(SlowCo());
    }

    public IEnumerator SlowCo()
    {
        yield return new WaitForSeconds(atkCool);
        isRage = false;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    public override void Attack()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BaseTower>())
        {
            other.GetComponent<BaseTower>().isAttackSpeed = isRage;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseTower>())
        {
            other.GetComponent<BaseTower>().isAttackSpeed = false;
        }
    }
}
