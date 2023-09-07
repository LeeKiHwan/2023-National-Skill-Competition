using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DashMonster : MonoBehaviour, IMonster
{
    public Rigidbody rb;
    public Collider col;
    public Transform mesh;
    public bool isDash;
    public float dashForce;
    public float dashTime;
    public float playerCheckRange;
    public int score;
    public GameObject dieEffect;
    public AudioClip dieSFX;

    private void Update()
    {
        col.transform.rotation = Quaternion.identity;
        mesh.rotation = Quaternion.Euler(0, 180, 0);

        Attack();
    }

    public void Attack()
    {
        Collider[] playerCheck = Physics.OverlapSphere(transform.position, playerCheckRange, 1 << LayerMask.NameToLayer("Player"));

        if (playerCheck.Length > 0 && !isDash)
        {
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
        InGameManager.Instance.AddScore(score);
        InGameManager.Instance.monsters.Remove(gameObject);
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySFX(dieSFX, false);
        InGameManager.Instance.CheckMonster();
        Destroy(gameObject);
    }
}
