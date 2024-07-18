using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Car
{
    [Header("Player Move Speed")]
    [SerializeField] float driftMoveSpeed;
    [SerializeField] float dragValue;

    [Header("Player Rotate Speed")]
    [SerializeField] float baseRotateSpeed;
    [SerializeField] float driftRotateIncreaseSpeed;
    [SerializeField] float driftRotateDecreaseSpeed;
    [SerializeField] float minDriftRotateSpeed;
    [SerializeField] float maxDriftRotateSpeed;
    float rotateSpeed;
    float speedLerpTime;
    [SerializeField] float maxDriftSpeedTime;

    [Header("Player Drift")]
    [SerializeField] bool isDrifting;
    [SerializeField] float baseTraction;
    [SerializeField] float driftTraction;
    float traction;
    [SerializeField] TrailRenderer leftDriftEffect;
    [SerializeField] TrailRenderer rightDriftEffect;

    [Header("Player Boost")]
    [SerializeField] GameObject boostEffect;

    private void Update()
    {
        AddBoost(Time.deltaTime);

        if (!isTraped && GameManager.Instance.isGameStart && !isFinish)
        {
            Move();
            Drift();
            Boost();
            UseItem();
            DecreaseInvincibleTime();
        }

        if (Input.GetKeyDown(KeyCode.Space) && curItem != null)
        {
            curItem.UseItem(this);
        }
    }
    private void FixedUpdate()
    {
        moveForce *= dragValue;
    }

    protected override void Move()
    {
        moveForce += transform.up * Input.GetAxis("Vertical") * MaxSpeed * Time.deltaTime;
        transform.position += moveForce * Time.deltaTime;

        float z = -Input.GetAxisRaw("Horizontal");
        transform.Rotate(Vector3.forward * z * moveForce.magnitude * rotateSpeed * Time.deltaTime);

        moveForce = Vector3.Lerp(moveForce.normalized, transform.up, traction * Time.deltaTime) * moveForce.magnitude;
        moveForce = Vector3.ClampMagnitude(moveForce, MaxSpeed);
    }

    void Drift()
    {
        float firstDir = 0;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDrifting && Input.GetAxisRaw("Horizontal") != 0)
        {
            rotateSpeed = minDriftRotateSpeed;
            traction = driftTraction;
            firstDir = Input.GetAxisRaw("Horizontal");
            speedLerpTime = 0;
            isDrifting = true;
        }

        if (isDrifting)
        {
            // ((moveForce.normalized - transform.up).magnitude < driftCancelRotate && Input.GetAxisRaw("Horizontal") != 0)
            if (moveForce.normalized == transform.up || Input.GetAxisRaw("Horizontal") == -firstDir || moveForce.magnitude < driftMoveSpeed)
            {
                speedLerpTime = 0;
                isDrifting = false;
            }

            if (Input.GetKey(KeyCode.LeftControl) && rotateSpeed < maxDriftRotateSpeed)
            {
                rotateSpeed += Time.deltaTime * driftRotateIncreaseSpeed;
            }
            else if (!Input.GetKey(KeyCode.LeftControl) && rotateSpeed > minDriftRotateSpeed)
            {
                rotateSpeed += -Time.deltaTime * driftRotateDecreaseSpeed;
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                traction = driftTraction;
                AddBoost(Time.deltaTime * boostChargeValue);
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                traction = baseTraction;
            }

            speedLerpTime += Time.deltaTime;
            maxMoveSpeed = Mathf.Lerp(baseMoveSpeed, driftMoveSpeed, speedLerpTime / maxDriftSpeedTime);

            leftDriftEffect.enabled = true;
            rightDriftEffect.enabled = true;
        }
        else
        {
            traction = baseTraction;
            maxMoveSpeed = baseMoveSpeed;
            rotateSpeed = baseRotateSpeed;

            leftDriftEffect.enabled = false;
            rightDriftEffect.enabled = false;
        }
    }
    protected override void Boost()
    {
        if (Input.GetKey(KeyCode.LeftShift) && curBoost > maxBoost * 0.3f)
        {
            curBoostSpeed = boostSpeed;
            curBoost -= Time.deltaTime * boostUseValue;
            GameManager.Instance.AddScore(1);
            boostEffect.SetActive(true);
        }
        else
        {
            curBoostSpeed = 0;
            boostEffect.SetActive(false);
        }
    }
    protected override void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Z) && curItem != null)
        {
            curItem.UseItem(this);
            GameManager.Instance.AddScore(1000);
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