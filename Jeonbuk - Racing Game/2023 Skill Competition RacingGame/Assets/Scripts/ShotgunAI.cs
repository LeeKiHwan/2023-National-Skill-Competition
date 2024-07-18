using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShotgunAI : Car
{

    [Header("AI Ray")]
    [SerializeField] float minRayDis;
    [SerializeField] float maxRayDis;
    float RayDis;
    [SerializeField] int rayCount;
    [SerializeField] float minRayAngle;
    [SerializeField] float maxRayAngle;
    float rayAngle;
    RaycastHit2D leftOutRay;
    RaycastHit2D leftInRay;
    RaycastHit2D rightOutRay;
    RaycastHit2D rightInRay;

    [Header("AI Move Speed")]
    [SerializeField] float minBaseMoveSpeed;
    [SerializeField] float maxBaseMoveSpeed;

    [Header("AI Boost")]
    [SerializeField] float minBoostSpeed;
    [SerializeField] float maxBoostSpeed;
    [SerializeField] float minBoostCheckDis;
    [SerializeField] float maxBoostCheckDis;
    float boostCheckDis;
    [SerializeField] float minBoostGetRotate;
    [SerializeField] float maxBoostGetRotate;
    float boostGetRotate;

    private void Awake()
    {
        RayDis = Random.Range(minRayDis, maxRayDis);
        rayAngle = Random.Range(minRayAngle, maxRayAngle);
        baseMoveSpeed = Random.Range(minBaseMoveSpeed, maxBaseMoveSpeed);
        boostSpeed = Random.Range(minBoostSpeed, maxBoostSpeed);
        boostCheckDis = Random.Range(minBoostCheckDis, maxBoostCheckDis);
        boostGetRotate = Random.Range(minBoostGetRotate, maxBoostGetRotate);
    }

    void Update()
    {
        if (!isTraped && GameManager.Instance.isGameStart && !isFinish)
        {
            Move();
            ShotRay();
            CheckRotate();
            Boost();
            UseItem();
            DecreaseInvincibleTime();
        }
    }

    protected override void Move()
    {
        maxMoveSpeed = baseMoveSpeed;

        moveForce += transform.up * 1 * MaxSpeed * Time.deltaTime;
        transform.position += moveForce * Time.deltaTime;

        moveForce = Vector3.ClampMagnitude(moveForce, MaxSpeed);
        moveForce = Vector3.Lerp(moveForce.normalized, transform.up, 2 * Time.deltaTime) * moveForce.magnitude;
    }

    void ShotRay()
    {
        int rayIdx = 0;
        for (float i = -rayAngle / 2; i <= rayAngle / 2; i += rayAngle / (rayCount - 1))
        {
            var rot = Quaternion.AngleAxis(i, transform.forward);
            var dir = rot * transform.up;

            switch (rayIdx)
            {
                case 0:
                    leftOutRay = Physics2D.Raycast(transform.position, dir, RayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 1:
                    leftInRay = Physics2D.Raycast(transform.position, dir, RayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 2:
                    rightInRay = Physics2D.Raycast(transform.position, dir, RayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 3:
                    rightOutRay = Physics2D.Raycast(transform.position, dir, RayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
            }
            rayIdx++;
        }
    }

    void CheckRotate()
    {
        float leftOutDis = leftOutRay.collider != null ? leftOutRay.distance : RayDis;
        float leftInDis  = leftInRay.collider != null ? leftInRay.distance : RayDis;
        float rightOutDis = rightOutRay.collider != null ? rightOutRay.distance : RayDis;
        float rightInDis = rightInRay.collider != null ? rightInRay.distance : RayDis;

        float leftRotate = -leftOutDis + -leftInDis;
        float rightRotate = rightOutDis + rightInDis;

        float rotate = leftRotate + rightRotate;

        if (Mathf.Abs(rotate) > boostGetRotate) AddBoost(Time.deltaTime * boostChargeValue); 

        transform.Rotate(new Vector3(0, 0, rotate));
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 1, 0, 0.25f);

    //    for (float i = -rayAngle / 2; i <= rayAngle / 2; i += rayAngle / (rayCount - 1))
    //    {
    //        var rot = Quaternion.AngleAxis(i, transform.forward);
    //        var dir = rot * transform.up;

    //        Gizmos.DrawRay(transform.position, dir * RayDis);
    //    }

    //    Gizmos.color = new Color(1, 0, 0, 0.25f);
    //    Gizmos.DrawRay(transform.position, transform.up * boostCheckDis);
    //}

    protected override void Boost()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, boostCheckDis, 1 << LayerMask.NameToLayer("Wall"));

        if (hit.distance == 0)
        {
            curBoostSpeed = boostSpeed;
            curBoost -= Time.deltaTime * boostUseValue;
        }
        else
        {
            curBoostSpeed = 0;
        }
    }

    protected override void UseItem()
    {
        switch (curItem)
        {
            case TrapItem:
                curItem.UseItem(this);
                break;
            case RocketItem:
                curItem.UseItem(this);
                break;
            case ShieldItem:
                if (isLockOn)
                {
                    curItem.UseItem(this);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IGetableItem>() != null)
        {
            collision.GetComponent<IGetableItem>().GetItem(this);
        }
    }
}
