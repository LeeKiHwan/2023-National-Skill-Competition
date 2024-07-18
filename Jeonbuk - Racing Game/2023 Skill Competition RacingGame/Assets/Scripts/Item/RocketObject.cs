using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketObject : MonoBehaviour
{
    [SerializeField] GameObject RocketEffect;
    [SerializeField] float readyTime;
    [SerializeField] float stunTime;

    public void LockOn()
    {
        StartCoroutine(LockOnCoroutine());
    }

    IEnumerator LockOnCoroutine()
    {
        yield return new WaitForSeconds(readyTime);
        gameObject.GetComponentInParent<Car>().Traped(stunTime);
        gameObject.GetComponentInParent<Car>().isLockOn = false;
        Instantiate(RocketEffect, transform.position, Quaternion.identity).GetComponent<EffectObject>().SetDestroyTime(1);
        Destroy(gameObject);

        yield break;
    }
}
