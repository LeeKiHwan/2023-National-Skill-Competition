using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemy : BaseEnemy
{
    public GameObject splitEnemy;
    public float splitCool;
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

        yield return new WaitForSeconds(splitCool);

        int spawnCount = Random.Range(2, 5);

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject g = Instantiate(splitEnemy, transform.position + new Vector3(Random.Range(-1.5f,1.5f), Random.Range(-1.5f,1.5f)), Quaternion.identity);
            g.GetComponent<SplitEnemy>().splitCount = splitCount + 1;
        }

        Destroy(gameObject);

        yield break;
    }
}
