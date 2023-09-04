using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Image playerBoostImage;
    public TextMeshProUGUI playerScoreText;
    public Image playerItemImage;

    [Header("GameInfo UI")]
    public TextMeshProUGUI gameCountDownText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI racingRankingText;
    public TextMeshProUGUI finalRacingRankingText;
    public TextMeshProUGUI curStageGetScoreText;
    public TextMeshProUGUI AccelText;
    public GameObject redLight;
    public GameObject yellowLight;
    public GameObject greenLight;
    public GameObject trafficLight;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SetPlayerUI();
        SetGameUI();
    }

    void SetPlayerUI()
    {
        playerBoostImage.fillAmount = (InGameManager.Instance.player.curBoostValue / InGameManager.Instance.player.maxBoostValue);
        playerScoreText.text = "Score : " + InGameManager.score;
        playerItemImage.sprite = ItemManager.Instance.GetPlayerItemImage();
        AccelText.text = InGameManager.Instance.player.rb.velocity.magnitude > 0 ? ((int)(InGameManager.Instance.player.rb.velocity.magnitude * 2)).ToString() : "0" ;
    }

    void SetGameUI()
    {
        string min = ((int)InGameManager.Instance.time / 60) < 10 ? "0" + ((int)InGameManager.Instance.time / 60) : ((int)InGameManager.Instance.time / 60).ToString();
        string sec = InGameManager.Instance.time < 10 ? "0" + InGameManager.Instance.time.ToString("#.##") : InGameManager.Instance.time.ToString("#.##");
        timeText.text = "Time : " + min + ":" + sec;

        int ranking = 0;
        for (int i=0; i<RacingRankingManager.Instance.racingRanking.Count; i++)
        {
            if (RacingRankingManager.Instance.racingRanking[i] == InGameManager.Instance.player)
            {
                ranking = i;
            }
        }

        string text = "";
        switch (ranking)
        {
            case 0:
                text = $"{ranking + 1}st";
                break;
            case 1:
                text = $"{ranking + 1}nd";
                break;
            case 2:
                text = $"{ranking + 1}rd";
                break;
            default:
                text = $"{ranking + 1}th";
                break;
        }
        racingRankingText.text = text;
    }

    public void GameStartCountDown()
    {
        StartCoroutine(GameStartCountDownCo());
    }

    IEnumerator GameStartCountDownCo()
    {
        yield return new WaitForSeconds(1);

        for (int i=0; i<3; i++)
        {
            switch(i)
            {
                case 0:
                    redLight.SetActive(true);
                    break;
                case 1:
                    yellowLight.SetActive(true);
                    break;
                case 2:
                    greenLight.SetActive(true);
                    break;
            }

            yield return new WaitForSeconds(1);
        }
        gameCountDownText.text = "Start!";
        trafficLight.SetActive(false);
        InGameManager.Instance.isGameStart = true;

        yield return new WaitForSeconds(1);
        gameCountDownText.text = "";

        yield break;
    }

    public void GameFinishCountDown()
    {
        StartCoroutine(GameFinishCountDownCo());
    }

    IEnumerator GameFinishCountDownCo()
    {
        for (int i = 10; i > 0; i--)
        {
            if (!InGameManager.Instance.isPlayerFinish)
            {
                gameCountDownText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }
            else
            {
                break;
            }
        }
        gameCountDownText.text = "";
        if (!InGameManager.Instance.isPlayerFinish)
        {
            ShowFinalRanking();
        }

        yield break;
    }

    public void ShowFinalRanking()
    {
        StartCoroutine(ShowFinalRankingCo());
    }

    IEnumerator ShowFinalRankingCo()
    {
        if (InGameManager.Instance.isPlayerFinish)
        {
            string text = "";
            Color color = new Color();
            switch (InGameManager.Instance.finalRacingRanking)
            {
                case 1:
                    text = $"{InGameManager.Instance.finalRacingRanking}st";
                    color = new Color(1,1,0);
                    break;
                case 2:
                    text = $"{InGameManager.Instance.finalRacingRanking}nd";
                    color = new Color(0.8f, 0.8f, 0.8f);
                    break;
                case 3:
                    text = $"{InGameManager.Instance.finalRacingRanking}rd";
                    color = new Color(0.5f, 0.3f, 0);
                    break;
                default:
                    text = $"{InGameManager.Instance.finalRacingRanking}th";
                    color = new Color(0.2f, 0.2f, 0.2f);
                    break;
            }
            finalRacingRankingText.text = text;
            finalRacingRankingText.color = color;
        }
        else
        {
            finalRacingRankingText.text = "Retire";
        }
        curStageGetScoreText.text = "Get Score : " + InGameManager.Instance.curStageGetScore;

        yield return new WaitForSeconds(2);

        if (InGameManager.Instance.isStageClear && InGameManager.curStage < 2)
        {
            InGameManager.Instance.NextStage();
        }
        else
        {
            InGameManager.Instance.EndGame();
        }

        yield break;
    }
}
