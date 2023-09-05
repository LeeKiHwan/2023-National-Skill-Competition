using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMonster : MonoBehaviour, IMonster
{
    public Rigidbody rb;
    public bool isDash;
    public float dashForce;
    public float dashTime;
    public float playerCheckRange;

    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        Collider[] playerCheck = Physics.OverlapSphere(transform.position, playerCheckRange, 1 << LayerMask.NameToLayer("Player"));

        if (playerCheck.Length > 0 && !isDash)
        {
            Debug.Log(playerCheck[0].name);
            StartCoroutine(Dash(playerCheck[0].transform.position));
        }

        if (!isDash) rb.velocity = new Vector3(0,rb.velocity.y, 0);
    }

    IEnumerator Dash(Vector3 playerPos)
    {
        isDash = true;

        Vector3 dashDir = playerPos - transform.position;
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);
        
        yield return new WaitForSeconds(dashTime);

        isDash = false;

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1,0,0,0.25f);
        Gizmos.DrawSphere(transform.position, playerCheckRange);
    }

    public void Die()
    {

    }
}
