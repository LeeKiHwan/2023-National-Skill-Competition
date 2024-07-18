using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum RiderType
{
    Player,
    Enemy
}

public abstract class Car : MonoBehaviour
{
    protected float MaxSpeed { get => maxMoveSpeed + curBoostSpeed + additionalSpeed; }
    public RiderType riderType;

    [Header("Move Speed")]
    [SerializeField] protected float baseMoveSpeed;
    protected float maxMoveSpeed;
    protected float additionalSpeed;

    [Header("Boost")]
    public float maxBoost;
    public float curBoost;
    [SerializeField] protected float boostSpeed;
    protected float curBoostSpeed;
    [SerializeField] protected float boostUseValue;
    [SerializeField] protected float boostChargeValue;

    protected bool isTraped;
    public bool isLockOn;
    protected Vector3 moveForce;
    public IUsableItem curItem;
    float invincibleTime;
    public bool isFinish;

    protected abstract void Move();
    protected abstract void Boost();
    protected abstract void UseItem();

    protected void AddBoost(float addBoost)
    {
        if (curBoost < 10)
        {
            curBoost += addBoost;
        }
    }

    public void Traped(float trapedTime)
    {
        StartCoroutine(TrapedCo(trapedTime));
    }

    IEnumerator TrapedCo(float trapedTime)
    {
        if (invincibleTime > 0) yield break;
            
        isTraped = true;
        moveForce = Vector3.zero;
        yield return new WaitForSeconds(trapedTime);
        SetInvincibleTime(2);
        isTraped = false;

        yield break;
    }

    public void SetInvincibleTime(float setTime)
    {
        if (invincibleTime < setTime)
        {
            invincibleTime = setTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    protected void DecreaseInvincibleTime()
    {
        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
