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
            string[] s = { "타워 어드벤처에 온걸 환영합니다.", "타워 어드벤처는 타워를 건설해 몰려오는 적으로부터 최종 보호 시설을 보호하는 타워디펜스 게임입니다.",
        "주어진 재화를 통해 여러 타워와 아이템, 스킬을 구매하여 적의 침략을 막아보세요."};
            UIManager.Instance.Explain(s);
        }
        else if (curStage == 1)
        {
            string[] s = {"2스테이지에서는 1스테이지와 다르게 4곳의 적 출현지점이 존재합니다.", "또한 더 많은 웨이브와 적들이 등장합니다."};
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
                string[] s = {"타워 설치 외에도 아이템과 스킬 구매를 통해 아군 타워엔 이로운 효과를 적들에겐 해로운 효과를 줄 수 있습니다.", 
                    "아이템과 스킬 구매도 적절히 활용하여 최종 보호 시설을 보호 해보세요."};
                UIManager.Instance.Explain(s);
            }

            if (curWave == 3 && curStage==0)
            {
                string[] s = {"보스가 등장했습니다. 보스를 해치워도 나머지 적들을 해치우지 않으면 다음 스테이지로 못넘어가니 주의하세요."};
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
            string[] s = { "축하합니다! 적들을 모두 해치웠습니다!"};
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
