using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Transform cursor;

    public TextMeshProUGUI[] names;
    public TextMeshProUGUI[] scores;

    public AudioClip bgm;
    public AudioClip uiInteract;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }

    private void Update()
    {
        Cursor.visible = false;

        cursor.position = Input.mousePosition;
    }

    public void GameStart()
    {
        GameManager.score = 0;
        SceneManager.LoadScene("Stage1");
    }

    public void ShowRanking()
    {
        for (int i = 0; i < names.Length; i++)
        {
            names[i].text = "";
            scores[i].text = "";
        }

        for (int i = 0; i < RankingManager.Instance.ranking.Count; i++)
        {
            names[i].text = RankingManager.Instance.ranking[i].Name;
            scores[i].text = RankingManager.Instance.ranking[i].score + " p";
        }
    }

    public void UIClick()
    {
        SoundManager.Instance.PlaySFX(uiInteract);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
