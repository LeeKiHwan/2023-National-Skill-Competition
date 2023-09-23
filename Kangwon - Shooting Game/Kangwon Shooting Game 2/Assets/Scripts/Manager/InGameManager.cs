using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    [Header("Game")]
    public static int score;
    public int curStage;
    public float bossSpawnTime;
    public bool isBossSpawned;

    [Header("Stage1")]
    public GameObject boss;
    public GameObject[] enemys;
    public float[] enemySpawnCools;
    public float enemySpawnCool;
    public float enemySpawnCur;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        bossSpawnTime -= Time.deltaTime;

        SetEnemySpawnCool();

        if (!isBossSpawned)
        {
            SpawnEnemy();
        }
    }

    public void SetEnemySpawnCool()
    {
        if (bossSpawnTime <= 0 && !isBossSpawned)
        {
            SpawnBoss();
            isBossSpawned = true;
        }

        if (bossSpawnTime > 80)
        {
            enemySpawnCool = enemySpawnCools[0];
        }
        else if (bossSpawnTime > 40)
        {
            enemySpawnCool = enemySpawnCools[1];
        }
        else if (bossSpawnTime > 0)
        {
            enemySpawnCool = enemySpawnCools[2];
        }
    }

    public void SpawnEnemy()
    {
        if (enemySpawnCur > 0) enemySpawnCur -= Time.deltaTime;

        if (enemySpawnCur <= 0)
        {
            Vector2 spawnPos = Vector2.zero;
            int idx = Random.Range(0, enemys.Length-1);

            if (curStage == 0)
            {
                int yDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;

                float x = Random.Range(-15, 15);
                float y = Mathf.Abs(x) > 10 ? Random.Range(-6, 6) : yDir * 8;

                spawnPos = new Vector2(x, y) + (Vector2)PlayerAttackManager.Instance.player.transform.position;
                Instantiate(enemys[idx], spawnPos, Quaternion.identity);
            }

            if (curStage == 1)
            {
                spawnPos = new Vector2(Random.Range(-5, 5), 7);
                Instantiate(enemys[idx], spawnPos, Quaternion.identity);
            }

            enemySpawnCur = enemySpawnCool;
        }
    }

    public void SpawnBoss()
    {
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }

        if (curStage == 0)
        {
            int yDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;

            float x = Random.Range(-15, 15);
            float y = Mathf.Abs(x) > 10 ? Random.Range(-6, 6) : yDir * 8;

            Vector2 spawnPos = new Vector2(x, y) + (Vector2)PlayerAttackManager.Instance.player.transform.position;
            Instantiate(boss, spawnPos, Quaternion.identity);
        }

        if (curStage == 1)
        {
            Instantiate(boss, new Vector3(0,7), Quaternion.identity);
        }

        if (curStage == 2)
        {
            Instantiate(boss, new Vector2(PlayerAttackManager.Instance.player.transform.position.x + 6, PlayerAttackManager.Instance.player.transform.position.y), Quaternion.identity);
        }
    }

    public void StageClear()
    {
        StartCoroutine(StageClearCo());
    }

    public IEnumerator StageClearCo()
    {
        switch (curStage)
        {
            case 0:
                UIManager.Instance.FadeIn();
                yield return new WaitForSeconds(2);
                curStage = 1;
                SceneManager.LoadScene("Stage2");
                break;
            case 1:
                UIManager.Instance.FadeIn();
                yield return new WaitForSeconds(2);
                curStage = 2;
                SceneManager.LoadScene("Stage3");
                break;
            case 2:
                SceneManager.LoadScene("EndGame");
                break;
        }

        yield break;
    }
}
