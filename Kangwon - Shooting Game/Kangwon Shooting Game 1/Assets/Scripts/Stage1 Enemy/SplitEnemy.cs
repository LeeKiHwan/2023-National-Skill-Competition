using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : BaseEnemy
{
    public GameObject splitEnemy;
    public float splitCoolTime;
    public int splitCount;

    public void Awake()
    {
        StartCoroutine(Split());
    }

    public override void Attack()
    {
    }

    public override void Move()
    {
        Vector2 target = PlayerAttackManager.Instance.player.transform.position;
        Vector2 dir = target - (Vector2)transform.position;

        transform.up = dir.normalized;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator Split()
    {
        if (splitCount == 1) yield break;

        yield return new WaitForSeconds(splitCoolTime);

        int spawnCount = Random.Range(2, 5);

        for (int i = 0; i < 2; i++)
        {
            GameObject g = Instantiate(splitEnemy, transform.position, Quaternion.identity);
            g.GetComponent<SplitEnemy>().splitCount = splitCount + 1;
        }

        Destroy(gameObject);

        yield break;
    }
}
