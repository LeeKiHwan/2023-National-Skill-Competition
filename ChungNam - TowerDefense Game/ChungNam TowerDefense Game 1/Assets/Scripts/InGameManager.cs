using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    public static int curStage;
    public float gameTime;

    [Header("Enemy")]
    public int curWave;
    public float waveReadyTime;
    public Transform[] enemySpawnPos;
    public GameObject[] normalEnemys;
    public GameObject middleBossEnemy;
    public GameObject stageBossEnemy;
    public int[] spawnCounts;

    private void Awake()
    {
        Instance = this;
        GameStart();
    }

    public void GameStart()
    {
        StartCoroutine(WaveReady());
    }

    IEnumerator WaveReady()
    {
        waveReadyTime = 10;

        while (waveReadyTime > 0)
        {
            waveReadyTime -= Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.StartWaveUI();

        yield break;
    }

    public void WaveStart()
    {
        StartCoroutine(WaveStartCo());
    }

    IEnumerator WaveStartCo()
    {
        int spawnCount = 0;

        for (int i = 0; i < spawnCounts.Length; i++)
        {
            if (curWave == i)
            {
                spawnCount  = spawnCounts[i];
            }
        }

        while (spawnCount > 0)
        {
            GameObject enemy = null;
            int enemyIdx = Random.Range(0, normalEnemys.Length);

            Transform spawnPos = null;
            int spawnPosIdx = Random.Range(0, enemySpawnPos.Length);

            for (int i = 0; i < normalEnemys.Length; i++)
            {
                if (enemyIdx == i) enemy = normalEnemys[i];
            }

            for (int i = 0; i <  enemySpawnPos.Length; i++)
            {
                if (spawnPosIdx == i) spawnPos = enemySpawnPos[i];
            }

            Instantiate(enemy, spawnPos.position, Quaternion.identity);
            spawnCount--;
            yield return new WaitForSeconds(Random.Range(0.25f, 0.75f));
        }

        while (FindObjectsOfType<BaseEnemy>().Length > 0)
        {
            yield return new WaitForSeconds(1);
        }

        if (curWave < spawnCounts.Length)
        {
            curWave++;
            StartCoroutine(WaveReady());
        }
        else
        {
            StageClear();
        }


        yield break;
    }

    public void StageClear()
    {

    }
}
