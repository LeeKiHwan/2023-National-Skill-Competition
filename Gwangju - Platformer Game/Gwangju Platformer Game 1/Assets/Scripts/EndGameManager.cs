using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public RectTransform rankingBG;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    public GameObject rankInsertObj;
    public GameObject goToMenuBtn;
    public TMP_InputField nameInputField;

    private void Awake()
    {
        scoreText.text = "Score : " + InGameManager.score;

        if (RankingManager.Instance.ranking.Count < 5 || RankingManager.Instance.ranking[4].score < InGameManager.score)
        {
            rankInsertObj.SetActive(true);
            goToMenuBtn.SetActive(false);
        }

        ShowRanking();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowRanking()
    {
        foreach (Transform rankObj in rankingBG.GetComponentsInChildren<Transform>())
        {
            if (rankObj != rankingBG.transform)
            {
                Destroy(rankObj.gameObject);
            }
        }

        int cnt = RankingManager.Instance.ranking.Count > 5 ? 5 : RankingManager.Instance.ranking.Count;

        for (int i = 0; i < cnt; i++)
        {
            GameObject rankObj = Instantiate(RankingManager.Instance.rankObj, rankingBG);
            TextMeshProUGUI rankText = rankObj.GetComponentInChildren<TextMeshProUGUI>();
            rankText.text = "Name : " + RankingManager.Instance.ranking[i].name + "   Score : " + RankingManager.Instance.ranking[i].score;
        }
    }

    public void InsertRank()
    {
        RankInfo rankInfo = new RankInfo();
        rankInfo.score = InGameManager.score;
        rankInfo.name = nameInputField.text;

        RankingManager.Instance.InsertRank(rankInfo);
        nameText.text = "Name : " + nameInputField.text;

        ShowRanking();
    }
}
