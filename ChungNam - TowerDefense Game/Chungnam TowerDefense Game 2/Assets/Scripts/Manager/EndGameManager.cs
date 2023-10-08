using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;
    public Transform cursor;

    public TextMeshProUGUI gameScore;
    public GameObject rankInsertUI;
    public TMP_InputField nameInputField;
    public TextMeshProUGUI nameText;
    public GameObject menuBtn;

    public TextMeshProUGUI[] names;
    public TextMeshProUGUI[] scores;

    private void Awake()
    {
        Instance = this;

        gameScore.text = "Score : " + GameManager.score + " P";

        ShowRanking();

        if (RankingManager.Instance.ranking.Count < 3 || RankingManager.Instance.ranking[2].score < GameManager.score)
        {
            rankInsertUI.SetActive(true);
        }
        else
        {
            menuBtn.SetActive(true);
        }
    }

    private void Update()
    {

        Cursor.visible = false;

        cursor.position = Input.mousePosition;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ShowRanking()
    {
        for (int i=0;i<names.Length;i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i = 0; i < RankingManager.Instance.ranking.Count; i++)
        {
            names[i].text = RankingManager.Instance.ranking[i].Name;
            scores[i].text = RankingManager.Instance.ranking[i].score + " P";
        }
    }

    public void InsertRank()
    {
        RankInfo rankInfo = new RankInfo();
        rankInfo.Name = nameInputField.text;
        rankInfo.score = GameManager.score;

        RankingManager.Instance.InsertRank(rankInfo);
        nameText.text = "Name : " + nameInputField.text;
        rankInsertUI.SetActive(false);
        ShowRanking();
    }
}
