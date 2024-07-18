using UnityEngine;

public class AI : Car
{
    [Header("AI Ray")]
    [SerializeField] private int rayCount;
    [SerializeField] private float rayDis;
    [SerializeField] private float rayAngle;
    RaycastHit leftOutRayHit;
    RaycastHit leftInRayHit;
    RaycastHit rightOutRayHit;
    RaycastHit rightInRayHit;

    [Header("Ground Check Ray")]
    [SerializeField] float gravityForce;
    [SerializeField] Transform groundCheckRayStartPos;
    [SerializeField] float groundCheckRayDis;

    private void Update()
    {
        Move();
        ShotRay();
        Rotate();
    }

    private void FixedUpdate()
    {
        MoveForward();
    }

    protected override void Move()
    {
        maxMoveSpeed = baseMoveSpeed;

        moveForce += transform.forward * MaxSpeed * Time.deltaTime;

        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, Time.deltaTime) * moveForce.magnitude;
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

    private void ShotRay()
    {
        if (rayCount % 2 != 0 && rayCount == 0) return;

        int rayIdx = 0;
        for (float i = -rayAngle / 2; i <= rayAngle / 2; i += rayAngle / (rayCount - 1))
        {
            var rot = Quaternion.AngleAxis(i, transform.up);
            var dir = rot * transform.forward;

            switch (rayIdx)
            {
                case 0:
                    Physics.Raycast(transform.position, dir, out leftOutRayHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 1:
                    Physics.Raycast(transform.position, dir, out leftInRayHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 2:
                    Physics.Raycast(transform.position, dir, out rightInRayHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
                case 3:
                    Physics.Raycast(transform.position, dir, out rightOutRayHit, rayDis, 1 << LayerMask.NameToLayer("Wall"));
                    break;
            }
            rayIdx++;
        }
    }

    private void Rotate()
    {
        float leftOutDis = leftOutRayHit.collider != null ? leftOutRayHit.distance : rayDis;
        float leftInDis = leftInRayHit.collider != null ? leftInRayHit.distance : rayDis;
        float rightOutDis = rightOutRayHit.collider != null ? rightOutRayHit.distance : rayDis;
        float rightInDis = rightInRayHit.collider != null ? rightInRayHit.distance : rayDis;

        float leftRotateValue = -leftOutDis + -leftInDis;
        float rightRotateValue = rightOutDis + rightInDis;

        float rotateValue = leftRotateValue + rightRotateValue;
        transform.Rotate(new Vector3(0, rotateValue, 0));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 1);

        for (float i = -rayAngle / 2; i <= rayAngle / 2; i += rayAngle / (rayCount - 1))
        {
            var rot = Quaternion.AngleAxis(i, transform.up);
            var dir = rot * transform.forward;

            Gizmos.DrawRay(transform.position, dir * rayDis);
        }

        Gizmos.color = Color.yellow;

        Gizmos.DrawRay(groundCheckRayStartPos.position, -transform.up * groundCheckRayDis);
    }
}
