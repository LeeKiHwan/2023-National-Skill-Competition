using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Spawner : MonoBehaviour
{
    public GameObject[] enemys;
    public GameObject boss;
    public float spawnCoolTime;
    public float spawnCurTime;

    private void Update()
    {
        if (spawnCurTime > 0) spawnCurTime -= Time.deltaTime;

        else if (PlayerAttackManager.Instance.player && InGameManager.Instance.bossSpawnTime > 0)
        {
            SpawnEnemy();
            spawnCurTime = spawnCoolTime;
        }
        else if (PlayerAttackManager.Instance.player && InGameManager.Instance.bossSpawnTime <= 0 && !InGameManager.Instance.isBossSpawn)
        {
            SpawnBoss();
            InGameManager.Instance.isBossSpawn = true;
        }
    }

    public void SpawnEnemy()
    {
        int idx = Random.Range(0, enemys.Length - 1);

        Instantiate(enemys[idx], (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(Random.Range(-8, 8), 7), Quaternion.identity);
    }

    public void SpawnBoss()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        Instantiate(boss, (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(0, 8), Quaternion.identity);
    }
}
