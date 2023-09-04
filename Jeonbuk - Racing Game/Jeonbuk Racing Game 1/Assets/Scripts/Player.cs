using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Car
{
    [Header("Player Traction")]
    public float baseTraction;
    public float driftTraction;
    public float traction;

    [Header("Player Drift")]
    public float driftMoveSpeed;
    public float reachDriftSpeedValue;
    public float baseRotateSpeed;
    public float minDriftRotateSpeed;
    public float maxDriftRotateSpeed;
    public float driftIncreaseSpeed;
    public float driftDecreaseSpeed;
    public float rotateSpeed;
    public bool isDrifting;
    public TrailRenderer driftEffet1;
    public TrailRenderer driftEffet2;

    [Header("Sound")]
    public AudioClip engineSFX;
    public AudioClip boostSFX;
    public AudioClip driftSFX;
    public AudioClip itemSFX;

    void Start()
    {
        SoundManager.Instance.PlaySound(engineSFX, true);
    }

    private void Update()
    {
        if (!stun && InGameManager.Instance.isGameStart)
        {
            Move();
            Drift();
            Boost();
            ChargeBoost();
            UseItem();
        }

        if (stun) boostEffect.SetActive(false);
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    public override void Move()
    {
        moveForce += transform.forward * Input.GetAxisRaw("Vertical") * MaxSpeed * Time.deltaTime;

        float z = Input.GetAxisRaw("Horizontal");
        transform.Rotate(transform.up * z * rotateSpeed * moveForce.magnitude * Time.deltaTime);

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;
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

    void Drift()
    {
        float driftDir = 0;

        if (!isDrifting && Input.GetKeyDown(KeyCode.LeftControl) && Input.GetAxisRaw("Horizontal") != 0)
        {
            traction = driftTraction;
            rotateSpeed = minDriftRotateSpeed;
            driftDir = Input.GetAxisRaw("Horizontal");
            SoundManager.Instance.PlaySound(driftSFX, false);
            isDrifting = true;
        }

        if (isDrifting)
        {
            if (moveForce.normalized == transform.up || Input.GetAxisRaw("Horizontal") == -driftDir || moveForce.magnitude < driftMoveSpeed)
            {
                isDrifting = false;
            }

            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                traction = baseTraction;
            }
            else
            {
                traction = driftTraction;
                ChargeBoost();
            }

            if (Input.GetKey(KeyCode.LeftControl) && rotateSpeed < maxDriftRotateSpeed)
            {
                rotateSpeed += driftIncreaseSpeed * Time.deltaTime;
            }
            else if (!Input.GetKey(KeyCode.LeftControl) && rotateSpeed > minDriftRotateSpeed)
            {
                rotateSpeed -= driftDecreaseSpeed * Time.deltaTime;
            }

            moveSpeed = Mathf.Lerp(moveSpeed, driftMoveSpeed, reachDriftSpeedValue * Time.deltaTime);

            driftEffet1.enabled = true;
            driftEffet2.enabled = true;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            rotateSpeed = baseRotateSpeed;
            traction = baseTraction;

            driftEffet1.enabled = false;
            driftEffet2.enabled = false;
            
        }
    }

    public override void Boost()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && curBoostValue > maxBoostValue * 0.3f)
        {
            SoundManager.Instance.PlaySound(boostSFX, false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && curBoostValue > maxBoostValue * 0.3f)
        {
            curBoostSpeed = boostSpeed;
            curBoostValue -= Time.deltaTime * boostUseValue;
            InGameManager.Instance.AddScore(1);
            boostEffect.SetActive(true);
        }
        else
        {
            curBoostSpeed = Mathf.Lerp(curBoostSpeed, 0, Time.deltaTime * 5);
            boostEffect.SetActive(false);
        }
    }

    public override void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            switch (curItem)
            {
                case ItemType.Trap:
                    Vector3 spawnPos = new Vector3(transform.position.x + -transform.forward.x * 4, transform.position.y, transform.position.z + -transform.forward.z * 4);
                    Instantiate(ItemManager.Instance.TrapObj, spawnPos, transform.rotation);
                    break;
                case ItemType.Bomb:
                    if (RacingRankingManager.Instance.racingRanking[0] != this)
                    {
                        for (int i=RacingRankingManager.Instance.racingRanking.Count-1; i>0; i--)
                        {
                            if (RacingRankingManager.Instance.racingRanking[i] == this)
                            {
                                RacingRankingManager.Instance.racingRanking[i - 1].BombTargeting();
                            }
                        }
                    }
                    break;
                case ItemType.Shield:
                    OnInvc(3);
                    SetAdditionalSpeed(30, 3);
                    break;
                case ItemType.ChargeBoost:
                    curBoostValue = maxBoostValue;
                    break;
                default:
                    break;
            }
            curItem = ItemType.None;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(transform.position, Vector3.down * 1.1f);
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

            InGameManager.Instance.AddScore(2000);
            SoundManager.Instance.PlaySound(itemSFX, false);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Trap"))
        {
            Stun(2);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            InGameManager.Instance.FinishGame(CarType.Player);
        }
    }
}
