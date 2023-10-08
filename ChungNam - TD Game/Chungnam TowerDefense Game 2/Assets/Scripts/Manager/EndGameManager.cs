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

    public AudioClip rankInsertSFX;
    public AudioClip bgm;
    public AudioClip scoreSFX;
    public AudioClip scoreEndSFX;

    private void Awake()
    {
        SoundManager.Instance.PlayBGM(bgm);
        Instance = this;

        StartCoroutine(ScoreCo());

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

    public IEnumerator ScoreCo()
    {
        int score = 0;
        int scoreSound = 0;
        while (GameManager.score >= score)
        {
            score+=100;
            scoreSound += 1;
            if (scoreSound % 10 == 0)
            {
                SoundManager.Instance.PlaySFX(scoreSFX);
            }
            gameScore.text = score + " P";
            yield return new WaitForSeconds(0.01f);
        }
        SoundManager.Instance.PlaySFX(scoreEndSFX);

        yield break;
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
        SoundManager.Instance.PlaySFX(rankInsertSFX);
        RankInfo rankInfo = new RankInfo();
        rankInfo.Name = nameInputField.text;
        rankInfo.score = GameManager.score;

        RankingManager.Instance.InsertRank(rankInfo);
        nameText.text = "Name : " + nameInputField.text;
        rankInsertUI.SetActive(false);
        ShowRanking();
    }
}
