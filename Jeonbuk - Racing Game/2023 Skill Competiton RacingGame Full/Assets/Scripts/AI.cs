using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI : Car
{
    public bool isInGame;
    public Transform leftWheel;
    public Transform rightWheel;

    [Header("AI Random")]
    public float minBaseMoveSpeed;
    public float maxBaseMoveSpeed;
    public float minBoostSpeed;
    public float maxBoostSpeed;

    [Header("Ray")]
    public float rayCount;
    public float minRayDis;
    public float maxRayDis;
    float rayDis;
    public float rayAngle;
    RaycastHit leftOutHit;
    RaycastHit leftInHit;
    RaycastHit rightOutHit;
    RaycastHit rightInHit;

    [Header("AI Boost")]
    public float minBoostCheckDis;
    public float maxBoostCheckDis;
    float boostCheckDis;

    private void Awake()
    {
        rb.transform.parent = null;

        baseMoveSpeed = Random.Range(minBaseMoveSpeed, maxBaseMoveSpeed);
        boostSpeed = Random.Range(minBoostSpeed, maxBoostSpeed);
        rayDis = Random.Range(minRayDis, maxRayDis);
        boostCheckDis = Random.Range(minBoostCheckDis, maxBoostCheckDis);
    }

    private void Update()
    {
        if (isInGame)
        {
            if (!stun && InGameManager.Instance.isGameStart)
            {
                Move();
                ShotRay();
                CheckRotate();
                Boost();
                UseItem();
            }
        }
        else
        {
            Move();
            ShotRay();
            CheckRotate();
        }

        if (stun) boostEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    public override void Move()
    {
        moveSpeed = baseMoveSpeed;

        moveForce += transform.forward * MaxSpeed * Time.deltaTime;

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, Time.deltaTime) * moveForce.magnitude;
        moveForce = Vector3.ClampMagnitude(moveForce, MaxSpeed);

        transform.position = rb.transform.position;
    }

    void MoveForward()
    {
        rb.velocity = new Vector3(moveForce.x, rb.velocity.y, moveForce.z);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {
            rb.AddForce(Vector3.down * 10000);
        }
    }

    void ShotRay()
    {
        if (rayCount == 0 && rayCount % 2 == 1) return; 

        int rayIdx = 0;

        for (float i = -rayAngle/2; i <= rayAngle/2; i += rayAngle / (rayCount - 1))
        {
            var rot = Quaternion.AngleAxis(i, transform.up);
            var dir = rot * transform.forward;

            switch (rayIdx)
            {
                case 0:
                    Physics.Raycast(transform.position, dir, out leftOutHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 1:
                    Physics.Raycast(transform.position, dir, out leftInHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 2:
                    Physics.Raycast(transform.position, dir, out rightInHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 3:
                    Physics.Raycast(transform.position, dir, out rightOutHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
            }

            rayIdx++;
        }
    }

    void CheckRotate()
    {
        float leftOutRotateValue = leftOutHit.collider ? leftOutHit.distance : rayDis;
        float leftInRotateValue = leftInHit.collider ? leftInHit.distance : rayDis;
        float rightOutRotateValue = rightOutHit.collider ? rightOutHit.distance : rayDis;
        float rightInRotateValue = rightInHit.collider ? rightInHit.distance : rayDis;

        float leftRotateValue = -leftOutRotateValue + -leftInRotateValue;
        float rightRotateValue = rightOutRotateValue + rightInRotateValue;

        float rotateValue = leftRotateValue + rightRotateValue;
        transform.Rotate(new Vector3(0, rotateValue, 0));
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    for (float i = -rayAngle / 2; i <= rayAngle / 2; i += rayAngle / (rayCount - 1))
    //    {
    //        var rot = Quaternion.AngleAxis(i, transform.up);
    //        var dir = rot * transform.forward;

    //        Gizmos.DrawRay(transform.position, dir * rayDis);
    //    }

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(transform.position, transform.forward * boostCheckDis);
    //}

    public override void Boost()
    {
        if (!Physics.Raycast(transform.position, transform.forward, boostCheckDis, 1 << LayerMask.NameToLayer("Wall")) && curBoostValue > 0)
        {
            curBoostSpeed = boostSpeed;
            boostEffect.SetActive(true);
        }
        else
        {
            curBoostSpeed = 0;
            boostEffect.SetActive(false);
        }
    }

    public override void UseItem()
    {
        switch (curItem)
        {
            case ItemType.Trap:
                Vector3 spawnPos = new Vector3(transform.position.x + -transform.forward.x * 4, transform.position.y, transform.position.z + -transform.forward.z * 4);
                Instantiate(ItemManager.Instance.TrapObj, spawnPos, transform.rotation);
                curItem = ItemType.None;
                break;
            case ItemType.Bomb:
                if (RacingRankingManager.Instance.racingRanking[0] != this)
                {
                    for (int i = RacingRankingManager.Instance.racingRanking.Count - 1; i > 0; i--)
                    {
                        if (RacingRankingManager.Instance.racingRanking[i] == this)
                        {
                            RacingRankingManager.Instance.racingRanking[i - 1].BombTargeting();
                            curItem = ItemType.None;
                        }
                    }
                }
                break;
            case ItemType.Shield:
                if (isTargeting)
                {
                    OnInvc(3);
                    SetAdditionalSpeed(30, 3);
                    curItem = ItemType.None;
                }
                break;
            case ItemType.ChargeBoost:
                curBoostValue = maxBoostValue;
                curItem = ItemType.None;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            int rand = Random.Range(0, 4);

            switch (rand)
            {
                case 0:
                    curItem = ItemType.Trap;
                    break;
                case 1:
                    curItem = ItemType.Bomb;
                    break;
                case 2:
                    curItem = ItemType.Shield;
                    break;
                case 3:
                    curItem = ItemType.ChargeBoost;
                    break;
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            Stun(2);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            InGameManager.Instance.FinishGame(CarType.AI);
        }
    }
}
