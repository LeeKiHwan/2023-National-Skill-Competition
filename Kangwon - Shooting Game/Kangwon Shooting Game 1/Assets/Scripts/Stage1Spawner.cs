using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Spawner : MonoBehaviour
{
    public GameObject[] enemys;
    public GameObject boss;
    public List<float> spawnCoolTimeList;
    public float spawnCoolTime;
    public float spawnCurTime;

    private void Update()
    {
        if (InGameManager.Instance.bossSpawnTime > 100)
        {
            spawnCoolTime = spawnCoolTimeList[0];
        }
        else if (InGameManager.Instance.bossSpawnTime > 80)
        {
            spawnCoolTime = spawnCoolTimeList[1];
        }
        else if (InGameManager.Instance.bossSpawnTime > 60)
        {
            spawnCoolTime = spawnCoolTimeList[2];
        }
        else if (InGameManager.Instance.bossSpawnTime > 40)
        {
            spawnCoolTime = spawnCoolTimeList[3];
        }
        else if (InGameManager.Instance.bossSpawnTime > 20)
        {
            spawnCoolTime = spawnCoolTimeList[4];
        }

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
        int yDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        float x = Random.Range(-15, 15);
        float y = 0;

        if (Mathf.Abs(x) > 10)
        {
            y = Random.Range(-6, 6);
        }
        else
        {
            y = yDir * 8;
        }

        int idx = Random.Range(0, enemys.Length-1);

        Instantiate(enemys[idx], (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(x, y), Quaternion.identity);
    }

    public void SpawnBoss()
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        int yDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;

        float x = Random.Range(-15, 15);
        float y = 0;

        if (Mathf.Abs(x) > 10)
        {
            y = Random.Range(-6, 6);
        }
        else
        {
            y = yDir * 8;
        }

        Instantiate(boss, (Vector2)PlayerAttackManager.Instance.player.transform.position + new Vector2(x, y), Quaternion.identity);
    }
}
