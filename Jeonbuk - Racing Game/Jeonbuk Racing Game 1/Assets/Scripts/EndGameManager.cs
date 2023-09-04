using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public RectTransform rankingBG;
    public Text scoreText;
    public InputField nameInputField;
    public GameObject insertRankBtn;
    public GameObject goMenuBtn;

    private void Awake()
    {
        ShowRanking();
        scoreText.text = "점수 : " + InGameManager.score;

        if (RankingManager.Instance.ranking.Count < 4 || InGameManager.score > RankingManager.Instance.ranking[4].score)
        {
            insertRankBtn.SetActive(true);
            goMenuBtn.SetActive(false);
        }
    }

    public void ShowRanking()
    {
        foreach (Transform rankInfo in rankingBG.GetComponentsInChildren<Transform>())
        {
            if (rankInfo.transform != rankingBG.transform)
            {
                Destroy(rankInfo.gameObject);
            }
        }

        for (int i = 0; i < RankingManager.Instance.ranking.Count; i++)
        {
            Instantiate(RankingManager.Instance.rankInfoText, rankingBG).GetComponent<Text>().text = (i + 1) + "위 " + RankingManager.Instance.ranking[i].name + ", 점수 : " + RankingManager.Instance.ranking[i].score + "점";
        }
    }

    public void InsertRanking()
    {
        RankInfo rankInfo = new RankInfo();
        rankInfo.name = nameInputField.text;
        rankInfo.score = InGameManager.score;

        RankingManager.Instance.InsertRanking(rankInfo);
        ShowRanking();
    }

    public void GoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
