using System.Collections.Generic;
using UnityEngine;

public class Player : Car
{
    [Header("Ground Check Ray")]
    [SerializeField] Transform groundCheckRayStartPos;
    [SerializeField] float groundCheckRayDis;

    [Header("Player Move")]
    [SerializeField] private float gravityForce;
    [SerializeField] private float driftMoveSpeed;
    [SerializeField] private float reachDriftSpeedTime;

    [Header("Player Rotate Speed")]
    [SerializeField] float baseRotateSpeed;
    [SerializeField] float minDriftRotateSpeed;
    [SerializeField] float maxDriftRotateSpeed;
    [SerializeField] float driftRotateIncreaseSpeed;
    [SerializeField] float driftRotateDecreaseSpeed;
    float rotateSpeed;

    [Header("Player Drift")]
    [SerializeField] private bool isDrifting;
    [SerializeField] private float baseTraction;
    [SerializeField] private float driftTraction;
    private float traction;
    [SerializeField] private List<TrailRenderer> driftEffects;


    private void Update()
    {
        Move();
        Drift();
        Boost();
    }
    private void FixedUpdate()
    {
        MoveForward();
    }

    protected override void Move()
    {
        moveForce += transform.forward * Input.GetAxisRaw("Vertical") * MaxSpeed * Time.deltaTime;

        float z = Input.GetAxisRaw("Horizontal");
        transform.Rotate(Vector3.up * z * moveForce.magnitude * rotateSpeed * Time.deltaTime);

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, traction * Time.deltaTime) * moveForce.magnitude;
        moveForce = Vector3.ClampMagnitude(moveForce, MaxSpeed);

        transform.position = rb.transform.position;
    }

    private void MoveForward()
    {
        rb.velocity = new Vector3(moveForce.x, rb.velocity.y, moveForce.z);
        RaycastHit hit;

        if (Physics.Raycast(groundCheckRayStartPos.position, -transform.up, out hit, groundCheckRayDis, 1 << LayerMask.NameToLayer("Ground")))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }
        else
        {
            rb.AddForce(Vector3.up * -9.81f * gravityForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(groundCheckRayStartPos.position, -transform.up * groundCheckRayDis);
    }

    private void Drift()
    {
        float driftDir = 0;

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDrifting && Input.GetAxisRaw("Horizontal") != 0)
        {
            rotateSpeed = minDriftRotateSpeed;
            traction = driftTraction;
            driftDir = Input.GetAxisRaw("Horizontal");
            isDrifting = true;
        }

        if (isDrifting)
        {
            if (moveForce.normalized == transform.up || Input.GetAxisRaw("Horizontal") == -driftDir || moveForce.magnitude < driftMoveSpeed)
            {
                isDrifting = false;
            }

            if (Input.GetKey(KeyCode.LeftControl) && rotateSpeed < maxDriftRotateSpeed)
            {
                rotateSpeed += driftRotateIncreaseSpeed * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.LeftControl) && rotateSpeed > minDriftRotateSpeed)
            {
                rotateSpeed += driftRotateDecreaseSpeed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                traction = driftTraction;
            }
            else if (Input.GetAxisRaw("Horizontal") == 0)
            {
                traction = baseTraction;
            }

            maxMoveSpeed = Mathf.Lerp(maxMoveSpeed, driftMoveSpeed, reachDriftSpeedTime * Time.deltaTime);

            for (int i=0; i<driftEffects.Count; i++)
            {
                driftEffects[i].enabled = true;
            }
        }
        else
        {
            maxMoveSpeed = baseMoveSpeed;
            rotateSpeed = baseRotateSpeed;
            traction = baseTraction;

            for (int i = 0; i < driftEffects.Count; i++)
            {
                driftEffects[i].enabled = false;
            }
        }
    }

    void Boost()
    {
        if (Input.GetKey(KeyCode.LeftShift))
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
}
