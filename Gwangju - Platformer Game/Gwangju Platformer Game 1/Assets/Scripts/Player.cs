using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public Rigidbody rb;
    public Collider col;
    public float speed;
    public float jumpForce;
    public bool isJump;

    [Header("Dash")]
    public bool isDash;
    public float dashForce;
    public float dashTime;

    [Header("Under Check")]
    public Vector3 underCheckPos;
    public Vector3 underCheckSize;
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
        col.transform.rotation = Quaternion.identity;

        if (!isDash)
        {
            float x = Input.GetAxis("Horizontal") * speed;
            rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.y < 0) isJump = false;
    }

    void UnderCheck()
    {
        if (Physics.OverlapBox(transform.position + underCheckPos, underCheckSize) != null) isExistUnder = true;
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
        Gizmos.DrawCube(transform.position + underCheckPos, underCheckSize);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null && isExistUnder && !isJump)
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
                        InGameManager.Instance.Die();
                        break;
                }
            }
            else rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>())
        {
            if (other.GetComponent<Item>().itemType == ItemType.RotateItem)
            {
                InGameManager.Instance.SetTargetRotation(other.GetComponent<Item>().rotateVec);
            }
            else
            {
                curItem = other.GetComponent<Item>().itemType;
            }
            Destroy(other.gameObject);
        }
    }
}
