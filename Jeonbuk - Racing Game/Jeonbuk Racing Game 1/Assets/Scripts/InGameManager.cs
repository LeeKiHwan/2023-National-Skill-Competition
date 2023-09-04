using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;

    public Player player;
    public static int score;
    public int curStageGetScore;
    public bool isGameStart;
    public bool isPlayerFinish;
    public static int curStage;
    public int finalRacingRanking;
    public bool isStageClear;
    public float time;
    public AudioClip inGameBGM;

    int itemIdx;
    bool isPlayerInvc;
    
    private void Awake()
    {
        Instance = this;
        finalRacingRanking = 1;
        player = GameObject.Find("Player").GetComponent<Player>();
        SoundManager.Instance.PlayBGM(inGameBGM);
    }

    private void Update()
    {
        CheatKey();

        if (!isPlayerFinish && isGameStart)
        {
            time += Time.deltaTime;
        }
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        UIManager.Instance.GameStartCountDown();
    }

    public void AddScore(int score)
    {
        InGameManager.score += score;
        curStageGetScore += score;
    }

    public void FinishGame(Car.CarType carType)
    {
        if (carType == Car.CarType.Player)
        {
            if (finalRacingRanking == 1)
            {
                isStageClear = true;
            }
            isPlayerFinish = true;
            UIManager.Instance.ShowFinalRanking();
        }
        else if (carType == Car.CarType.AI)
        {
            if (RacingRankingManager.Instance.racingRanking[0] != player && finalRacingRanking == 1)
            {
                UIManager.Instance.GameFinishCountDown();
            }
            finalRacingRanking++;
        }
    }

    public void NextStage()
    {
        switch (curStage)
        {
            case 0:
                SceneManager.LoadScene("Stage2");
                curStage = 1;
                break;
            case 1:
                SceneManager.LoadScene("Stage3");
                curStage = 2;
                break;
            case 2:
                SceneManager.LoadScene("Stage1");
                curStage = 0;
                break;
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("EndGame");
    }

    public void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NextStage();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            switch (itemIdx)
            {
                case 0:
                    player.curItem = ItemType.Trap;
                    itemIdx++;
                    break;
                case 1:
                    player.curItem = ItemType.Bomb;
                    itemIdx++;
                    break;
                case 2:
                    player.curItem = ItemType.Shield;
                    itemIdx++;
                    break;
                case 3:
                    player.curItem = ItemType.ChargeBoost;
                    itemIdx = 0;
                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            player.baseMoveSpeed += 5;
            player.driftMoveSpeed += 5;
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            isPlayerInvc = !isPlayerInvc;
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            foreach(Car ai in FindObjectsOfType<Car>())
            {
                ai.Stun(10);
            }
        }

        if (isPlayerInvc) player.isInvc = true;
    }
}
