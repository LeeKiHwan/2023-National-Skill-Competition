using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public static int curStage;
    public float bossSpawnTime;
    public bool isBossSpawn;
    public bool isBossClear;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (bossSpawnTime > 0) bossSpawnTime -= Time.deltaTime;
    }

    public void StageClear()
    {
        switch (curStage)
        {
            case 0:
                SceneManager.LoadScene("Stage2");
                curStage++;
                break;
            case 1:
                EndGame();
                break;
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
