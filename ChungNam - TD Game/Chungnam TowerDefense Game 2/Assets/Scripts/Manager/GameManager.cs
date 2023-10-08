using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct WaveData
{
    public int spawnCount;
    public float spawnCool;
    public bool isBossSpawn;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("InGame")]
    public int curStage;
    public static int score;
    public GameObject patrolUnit;

    [Header("Enemy")]
    public GameObject[] normalEnemys;
    public GameObject boss;

    [Header("Wave")]
    public int curWave;
    public float waveCool;
    public float waveCur;
    public WaveData[] waveDatas;
    public Transform[] spawnPos;

    [Header("Sound")]
    public AudioClip bgm;
    public AudioClip stageClearSFX;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);

        if (curStage == 0)
        {
            string[] s = { "Ÿ�� ��庥ó�� �°� ȯ���մϴ�.", "Ÿ�� ��庥ó�� Ÿ���� �Ǽ��� �������� �����κ��� ���� ��ȣ �ü��� ��ȣ�ϴ� Ÿ�����潺 �����Դϴ�.",
        "�־��� ��ȭ�� ���� ���� Ÿ���� ������, ��ų�� �����Ͽ� ���� ħ���� ���ƺ�����."};
            UIManager.Instance.Explain(s);
        }
        else if (curStage == 1)
        {
            string[] s = {"2�������������� 1���������� �ٸ��� 4���� �� ���������� �����մϴ�.", "���� �� ���� ���̺�� ������ �����մϴ�."};
            UIManager.Instance.Explain(s);
        }
    }

    private void Update()
    {
        WaveTimer();
        CheatKey();
    }

    public void GetScore(int getScore)
    {
        score += getScore;
    }

    public void WaveTimer()
    {
        if (waveCur > 0) waveCur -= Time.deltaTime;
        else if (waveCur <= 0 && curWave < waveDatas.Length)
        {
            if (curWave==1 && curStage==0)
            {
                string[] s = {"Ÿ�� ��ġ �ܿ��� �����۰� ��ų ���Ÿ� ���� �Ʊ� Ÿ���� �̷ο� ȿ���� ���鿡�� �طο� ȿ���� �� �� �ֽ��ϴ�.", 
                    "�����۰� ��ų ���ŵ� ������ Ȱ���Ͽ� ���� ��ȣ �ü��� ��ȣ �غ�����."};
                UIManager.Instance.Explain(s);
            }

            if (curWave == 3 && curStage==0)
            {
                string[] s = {"������ �����߽��ϴ�. ������ ��ġ���� ������ ������ ��ġ���� ������ ���� ���������� ���Ѿ�� �����ϼ���."};
                UIManager.Instance.Explain(s);
            }

            StartCoroutine(SpawnEnemy(waveDatas[curWave].spawnCount, waveDatas[curWave].spawnCool, waveDatas[curWave].isBossSpawn));
            curWave++;
            waveCur = waveCool;
        }
    }

    public IEnumerator SpawnEnemy(int spawnCount, float spawnCool, bool isBossSpawn)
    {
        if (isBossSpawn)
        {
            int bossSpawnPos = Random.Range(0, spawnPos.Length);
            Instantiate(boss, spawnPos[bossSpawnPos].position, Quaternion.identity);
        }

        for (int i=0; i<spawnCount; i++)
        {
            int randEnemy = Random.Range(0, normalEnemys.Length);
            int randPos = Random.Range(0, spawnPos.Length);
            Instantiate(normalEnemys[randEnemy], spawnPos[randPos].position, Quaternion.identity);
            yield return new WaitForSeconds(spawnCool);
        }

        yield break;
    }

    public void CheckEnemy()
    {
        StartCoroutine(CheckEnemyCo());
    }

    public IEnumerator CheckEnemyCo()
    {
        yield return new WaitForSeconds(1);

        if (FindObjectsOfType<BaseEnemy>().Length <= 0 && curWave >= waveDatas.Length)
        {
            StageClear();
        }

        yield break;
    }

    public void StageClear()
    {
        UIManager.Instance.OnStageClearUI();
        SoundManager.Instance.PlaySFX(stageClearSFX);
        if (curStage == 1)
        {
            string[] s = { "�����մϴ�! ������ ��� ��ġ�����ϴ�!"};
            UIManager.Instance.Explain(s);
        }
    }


    public void NextStage()
    {
        if (curStage == 0)
        {
            SceneManager.LoadScene("Stage2");
        }
        else if (curStage == 1)
        {
            SceneManager.LoadScene("EndGame");
        }
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {

        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TowerManager.Instance.GetGold(1000);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            foreach(BaseEnemy enemy in FindObjectsOfType<BaseEnemy>())
            {
                Destroy(enemy);
                CheckEnemy();
            }
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            foreach (BaseEnemy enemy in FindObjectsOfType<BaseEnemy>())
            {
                enemy.TakeDamage(10000, null);
            }
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene("Menu");
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene("Stage1");
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("Stage2");
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            Instantiate(patrolUnit, Vector3.zero, Quaternion.identity);
        }
    }
}
