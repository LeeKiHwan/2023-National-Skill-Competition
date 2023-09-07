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
    public int blockTouch;
    public bool isStun;

    [Header("Dash")]
    public bool isDash;
    public float dashForce;
    public float dashTime;

    [Header("Under Check")]
    public Vector3 underCheckPos;
    public Vector3 underCheckSize;

    public ItemType curItem;
    public AudioClip itemGetSFX;

    private void Update()
    {
        Move();
        UseItem();
    }

    void Move()
    {
        col.transform.rotation = Quaternion.identity;

        if (!isDash && !isStun)
        {
            float x = Input.GetAxis("Horizontal") * speed;
            rb.velocity = new Vector3(x, rb.velocity.y, rb.velocity.z);
        }
        else if (isStun) rb.velocity = Vector3.zero;

        if (rb.velocity.y > 0)
        {
            isJump = true;
            blockTouch = 0;
        }
        else isJump = false;
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
    
    IEnumerator Stun(float time)
    {
        isStun = true;
        yield return new WaitForSeconds(time);
        isStun = false;

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.25f);
        Gizmos.DrawCube(transform.position + underCheckPos, underCheckSize * 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<IMonster>() != null)
        {
            if (Physics.OverlapBox(transform.position + underCheckPos, underCheckSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Monster")).Length > 0)
            {
                collision.gameObject.GetComponent<IMonster>().Die();
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                InGameManager.Instance.Die();
            }
        }
        else if (!isJump && Physics.OverlapBox(transform.position + underCheckPos, underCheckSize, Quaternion.identity, 1 << LayerMask.NameToLayer("Platform")).Length > 0 && blockTouch == 0)
        {
            float jumpForceValue = 1;

            if (collision.gameObject.GetComponent<SpecialBlock>())
            {
                ESpecialBlock blockType = collision.gameObject.GetComponent<SpecialBlock>().thisBlockType;

                switch (blockType)
                {
                    case ESpecialBlock.JumpBlock:
                        jumpForceValue = 1.5f;
                        break;

                    case ESpecialBlock.TempBlock:
                        Destroy(collision.gameObject);
                        break;

                    case ESpecialBlock.TrapBlock:
                        InGameManager.Instance.Die();
                        break;
                }
            }
            rb.AddForce(Vector3.up * jumpForce * jumpForceValue, ForceMode.Impulse);
            ++blockTouch;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Item>())
        {
            if (other.GetComponent<Item>().itemType == ItemType.RotateItem)
            {
                StartCoroutine(Stun(0.01f));
                transform.position = other.transform.position;
                InGameManager.Instance.SetTargetRotation(other.GetComponent<Item>().rotateVec);
            }
            else
            {
                curItem = other.GetComponent<Item>().itemType;
            }
            Destroy(other.gameObject);
            SoundManager.Instance.PlaySFX(itemGetSFX, false);
        }
        if (other.CompareTag("UnderBlock"))
        {
            InGameManager.Instance.Die();
        }
    }
}
