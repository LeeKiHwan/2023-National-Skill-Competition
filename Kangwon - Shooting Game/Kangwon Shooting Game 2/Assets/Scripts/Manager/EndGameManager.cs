using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI[] names;
    public TextMeshProUGUI[] scores;

    public GameObject menuBtn;
    public GameObject insertRankUI;
    public TextMeshProUGUI scoreText;
    public TMP_InputField nameInputField;

    private void Awake()
    {
        Cursor.visible = true;

        scoreText.text = "Score : " + InGameManager.score;
        ShowRanking();

        if (RankingManager.Instance.ranking.Count < 5 || RankingManager.Instance.ranking[4].Score < InGameManager.score)
        {
            insertRankUI.SetActive(true);
            menuBtn.SetActive(false);
        }
    }

    public void ShowRanking()
    {
        for (int i=0; i<names.Length; i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i=0; i<RankingManager.Instance.ranking.Count; i++)
        {
            names[i].text = RankingManager.Instance.ranking[i].Name;
            scores[i].text = RankingManager.Instance.ranking[i].Score + " P";
        }
    }

    public void InsertRank()
    {
        RankInfo rankInfo = new RankInfo();
        rankInfo.Name = nameInputField.text;
        rankInfo.Score = InGameManager.score;

        RankingManager.Instance.InsertRank(rankInfo);
        ShowRanking();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
