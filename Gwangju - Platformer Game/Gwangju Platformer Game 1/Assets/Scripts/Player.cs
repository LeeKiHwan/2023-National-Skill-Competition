using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;
    public float jumpForce;
    public bool isDash;
    public float dashForce;
    public float dashTime;

    [Header("Under Check")]
    public float underCheckRayDis;
    public bool isExistUnder;

    public ItemType curItem;

    private void Update()
    {
        Move();
        UnderCheck();
        UseItem();
    }

    void Move()
    {
        if (!isDash)
        {
            float x = Input.GetAxis("Horizontal") * speed;
            rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);
        }
    }

    void UnderCheck()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, underCheckRayDis);

        if (hit.collider != null) isExistUnder = true;
        else isExistUnder = false;
    }

    void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (curItem)
            {
                case ItemType.DoubleJumpItem:
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    break;
                case ItemType.DashItem:
                    StartCoroutine(Dash());
                    break;
                case ItemType.RotateItem:
                    break;
            }
            curItem = ItemType.None;
        }
    }

    IEnumerator Dash()
    {
        isDash = true;
        rb.AddForce(Vector3.right * Input.GetAxisRaw("Horizontal") * jumpForce, ForceMode.Impulse);
        yield return new WaitForSeconds(dashTime);
        isDash = false;

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawRay(transform.position, Vector3.down * underCheckRayDis);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && isExistUnder)
        {
            if (collision.gameObject.GetComponent<SpecialBlock>())
            {
                ESpecialBlock blockType = collision.gameObject.GetComponent<SpecialBlock>().thisBlockType;

                switch (blockType)
                {
                    case ESpecialBlock.JumpBlock:
                        rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
                        break;

                    case ESpecialBlock.TempBlock:
                        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        Destroy(collision.gameObject);
                        break;

                    case ESpecialBlock.TrapBlock:
                        break;
                }
            }
            else rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>())
        {
            curItem = other.GetComponent<Item>().itemType;
            Destroy(other.gameObject);
        }
    }
}
