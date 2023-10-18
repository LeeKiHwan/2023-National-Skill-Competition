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
    public GameObject[] weapons;
    public float weaponSpawnCool;

    [Header("Stage1")]
    public GameObject boss;
    public GameObject[] enemys;
    public float[] enemySpawnCools;
    public float enemySpawnCool;
    public float enemySpawnCur;

    [Header("Sound")]
    public AudioClip bgm;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);

        StartCoroutine(SpawnWeaponCo());
    }

    private void Update()
    {
        bossSpawnTime -= Time.deltaTime;

        SetEnemySpawnCool();

        if (!isBossSpawned)
        {
            SpawnEnemy();
        }
        CheatKey();
    }

    public IEnumerator SpawnWeaponCo()
    {
        while (true)
        {
            if (!PlayerAttackManager.Instance.player) yield return new WaitForSeconds(weaponSpawnCool);
            if (curStage ==0 || curStage == 2)
            {
                Vector2 pos = PlayerAttackManager.Instance.player.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                Instantiate(weapons[Random.Range(0, weapons.Length)], pos, Quaternion.identity);
            }
            else if (curStage == 1)
            {
                Instantiate(weapons[Random.Range(0, weapons.Length)], new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-3.5f, 3.5f)), Quaternion.identity);
            }
            yield return new WaitForSeconds(weaponSpawnCool);
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
            int idx = Random.Range(0, enemys.Length);

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

    public void GetScore(int getScore)
    {
        score += getScore;
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

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StageClear();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            PlayerAttackManager.Instance.GetXp(100);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlayerAttackManager.Instance.player.invcTime = 1000;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PlayerAttackManager.Instance.player.invcTime = 0;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            PlayerAttackManager.Instance.player.TakeHeal(100);
            PlayerAttackManager.Instance.player.GetMp(100);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            Vector2 spawnPos = Vector2.zero;
            int idx = Random.Range(0, enemys.Length);

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
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            if (curStage == 0 || curStage == 2)
            {
                Vector2 pos = PlayerAttackManager.Instance.player.transform.position + new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
                Instantiate(weapons[Random.Range(0, weapons.Length)], pos, Quaternion.identity);
            }
            else if (curStage == 1)
            {
                Instantiate(weapons[Random.Range(0, weapons.Length)], new Vector3(Random.Range(-7.5f, 7.5f), Random.Range(-3.5f, 3.5f)), Quaternion.identity);
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            foreach(BaseEnemy enemy in FindObjectsOfType<BaseEnemy>())
            {
                enemy.TakeDamage(10000);
            }
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            if (!isBossSpawned)
            {
                bossSpawnTime = 1;
            }
        }
    }
}
