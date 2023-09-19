using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecBullet : MonoBehaviour
{
    public int maxHitCount;
    public int hitCount;
    public float stunTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<BaseEnemy>().SetStunTime(stunTime);
            hitCount++;
            if (hitCount == maxHitCount)
            {
                Destroy(gameObject);
            }
        }
    }
}
