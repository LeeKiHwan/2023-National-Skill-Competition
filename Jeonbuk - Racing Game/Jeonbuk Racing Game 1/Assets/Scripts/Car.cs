using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Car : MonoBehaviour
{
    public enum CarType
    {
        Player,
        AI
    }
    
    protected Vector3 moveForce;
    public ItemType curItem;
    public float MaxSpeed { get => moveSpeed + curBoostSpeed + additionalSpeed; }

    protected bool stun;
    public bool isInvc;
    protected bool isTargeting;

    [Header("Move")]
    public Rigidbody rb;
    public float baseMoveSpeed;
    protected float moveSpeed;
    protected float additionalSpeed;

    [Header("Boost")]
    public float boostSpeed;
    protected float curBoostSpeed;
    public float maxBoostValue;
    public float curBoostValue;
    public float boostUseValue;
    public float boostChargeValue;
    public GameObject boostEffect;

    [Header("RankInfo")]
    public int curCheckPoint;
    public int finalRacingRanking;

    void Awake()
    {
        rb.transform.parent = null;

        curItem = ItemType.None;
    }

    public abstract void Move();
    public abstract void Boost();
    public abstract void UseItem();

    public void ChargeBoost()
    {
        if (curBoostValue < maxBoostValue)
        {
            curBoostValue += Time.deltaTime * boostChargeValue;
        }
    }

    public void Stun(float stunTime)
    {
        StartCoroutine(StunCo(stunTime));
    }

    IEnumerator StunCo(float stunTime)
    {
        if (!isInvc) stun = true;
        else yield break;
        moveForce = Vector3.zero;
        yield return new WaitForSeconds(stunTime);
        stun = false;
        OnInvc(2);

        yield break;
    }

    public void OnInvc(float invcTime)
    {
        StartCoroutine(OnInvcCo(invcTime));
    }

    IEnumerator OnInvcCo(float invcTime)
    {
        isInvc = true;
        GameObject shieldEffectObj = Instantiate(ItemManager.Instance.ShieldEffectObj, transform);
        shieldEffectObj.transform.position = transform.position;
        shieldEffectObj.GetComponent<EffectObj>().SetDestroy(invcTime);
        yield return new WaitForSeconds(invcTime);
        isInvc = false;

        yield break;
    }

    public void SetAdditionalSpeed(float additionalSpeed, float time)
    {
        StartCoroutine(SetAdditionalSpeedCo(additionalSpeed, time));
    }

    IEnumerator SetAdditionalSpeedCo(float additionalSpeed, float time)
    {
        this.additionalSpeed += additionalSpeed;
        yield return new WaitForSeconds(time);
        this.additionalSpeed -= additionalSpeed;

        yield break;
    }

    public void BombTargeting()
    {
        StartCoroutine(BombTargetingCo());
    }

    IEnumerator BombTargetingCo()
    {
        isTargeting = true;
        GameObject bombTargetedObj = Instantiate(ItemManager.Instance.BombTargetedObj, transform);
        bombTargetedObj.transform.position = transform.position;
        bombTargetedObj.GetComponent<EffectObj>().SetDestroy(2);

        yield return new WaitForSeconds(2);

        GameObject bombEffectObj = Instantiate(ItemManager.Instance.BombEffectObj, transform);
        bombEffectObj.transform.position = transform.position;
        bombEffectObj.GetComponent<EffectObj>().SetDestroy(0.25f);

        Stun(2);
        isTargeting = false;

        yield break;
    }
}
