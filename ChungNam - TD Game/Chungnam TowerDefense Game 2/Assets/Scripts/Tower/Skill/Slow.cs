using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : BaseTower
{
    bool isSlow = true;

    private void Awake()
    {
        StartCoroutine(SlowCo());
    }

    public IEnumerator SlowCo()
    {
        yield return new WaitForSeconds(atkCool);
        isSlow = false;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    public override void Attack()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BaseEnemy>())
        {
            other.GetComponent<BaseEnemy>().isSlow = isSlow;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BaseEnemy>())
        {
            other.GetComponent<BaseEnemy>().isSlow = false;
        }
    }
}
