using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public Player player;
    public GameObject map;
    public List<GameObject> monsters;
    public Vector3 targetRotation;
    public float rotateTime;
    public bool isRotating;
    public static int curStage;
    public static int life;
    public static int score;
    public float time;
    public bool isStageClear;
    public AudioClip bgm;
    public bool isPlayerInvc;

    private void Awake()
    {
        Instance = this;
        map = GameObject.Find("Map");
        player = GameObject.Find("Player").GetComponent<Player>();

        foreach (GameObject monster in GameObject.FindGameObjectsWithTag("Monster"))
        {
            monsters.Add(monster);
        }
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }

    private void Update()
    {
        RotateMap();

        if (!isStageClear) time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.P)) SceneManager.LoadScene("EndGame");
    }

    void RotateMap()
    {
        map.transform.rotation = Quaternion.Lerp(map.transform.rotation, Quaternion.Euler(targetRotation), rotateTime);
        rotateTime += Time.deltaTime / 5;

        if (map.transform.rotation != Quaternion.Euler(targetRotation))
        {
            isRotating = false;
        }
        else isRotating = true;

    }

    public void SetTargetRotation(Vector3 rotateValue)
    {
        rotateTime = 0;
        targetRotation = rotateValue;
    }

    public void AddScore(int score)
    {
        InGameManager.score += score;
    }

    public void Die()
    {
        if (isPlayerInvc) return;

        if (life > 1)
        {
            life--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else SceneManager.LoadScene("EndGame");
    }

    public void CheckMonster()
    {
        StartCoroutine(CheckMonsterCo());
    }

    IEnumerator CheckMonsterCo()
    {
        yield return new WaitForSeconds(0.25f);
        if (monsters.Count == 0) StageClear();
        yield break;
    }

    void StageClear()
    {
        switch (curStage)
        {
            case 1:
                //curStage++;
                //SceneManager.LoadScene("Stage2");
                SceneManager.LoadScene("EndGame");
                break;
            case 2:
                curStage++;
                SceneManager.LoadScene("Stage3");
                break;
            case 3:
                curStage++;
                SceneManager.LoadScene("Stage4");
                break;
            case 4:
                curStage++;
                SceneManager.LoadScene("Stage5");
                break;
            case 5:
                SceneManager.LoadScene("EndGame");
                break;
        }
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isPlayerInvc = !isPlayerInvc;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            life = 3;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            switch (curStage)
            {
                case 1:
                    //curStage++;
                    //SceneManager.LoadScene("Stage2");
                    SceneManager.LoadScene("EndGame");
                    break;
                case 2:
                    curStage++;
                    SceneManager.LoadScene("Stage3");
                    break;
                case 3:
                    curStage++;
                    SceneManager.LoadScene("Stage4");
                    break;
                case 4:
                    curStage++;
                    SceneManager.LoadScene("Stage5");
                    break;
                case 5:
                    curStage = 1;
                    SceneManager.LoadScene("Stage1");
                    break;
            }
        }

    }
}
