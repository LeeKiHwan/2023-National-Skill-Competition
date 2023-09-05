using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float jumpForce;

    [Header("Under Check")]
    public Vector3 underCheckPos;
    public Vector3 underCheckSize;
    public bool isExistUnder;

    private void Update()
    {
        Move();
        UnderCheck();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        rb.velocity = new Vector3(x, rb.velocity.y, 0f);
    }

    void UnderCheck()
    {
        Vector3 pos = transform.position + underCheckPos;
        if (Physics.OverlapBox(pos, underCheckSize) != null) isExistUnder = true;
        else isExistUnder = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Vector3 pos = transform.position + underCheckPos;
        Gizmos.DrawWireCube(pos, underCheckSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && isExistUnder)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
