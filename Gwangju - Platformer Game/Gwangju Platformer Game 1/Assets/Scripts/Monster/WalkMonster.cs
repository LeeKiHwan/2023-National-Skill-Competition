using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMonster : MonoBehaviour, IMonster
{
    public Rigidbody rb;
    public float speed;
    public int moveDir;

    public Vector3 wallCheckPos;
    public Vector3 wallCheckSize;

    public Vector3 floorCheckPos;
    public Vector3 floorCheckSize;

    private void Update()
    {
        Move();
        CheckRotate();
    }

    public void Move()
    {
        rb.velocity = new Vector3(speed * moveDir, rb.velocity.y, rb.velocity.z);
    }

    public void CheckRotate()
    {
        if (Physics.OverlapBox(transform.position + wallCheckPos, wallCheckSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Platform")).Length > 0)
        {
            Rotate();
        }
        if (Physics.OverlapBox(transform.position + floorCheckPos, floorCheckSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Platform")).Length < 1)
        {
            Rotate();
        }
    }

    public void Rotate()
    {
        wallCheckPos = new Vector3(wallCheckPos.x * -1, wallCheckPos.y, wallCheckPos.z);
        floorCheckPos = new Vector3(floorCheckPos.x * -1, floorCheckPos.y, floorCheckPos.z);
        moveDir *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(transform.position + wallCheckPos, wallCheckSize * 2);
        Gizmos.DrawCube(transform.position + floorCheckPos, floorCheckSize * 2);
    }

    public void Die()
    {

    }
}
