using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player")]
    public Slider playerBoostSlider;
    public Image playerItemImage;
    public Text playerLabText;
    public Text playerScoreText;

    [Header("Game")]
    public Text gameStartText;
    public Text RankingText;

    private void Awake()
    {
        Instance = this;

        StartCoroutine(OnGameStartText());
    }

    private void Update()
    {
        SetPlayerUI();
        SetRankingUI();
    }

    void SetPlayerUI()
    {
        playerBoostSlider.value = GameManager.Instance.player.curBoost / GameManager.Instance.player.maxBoost;
        playerItemImage.sprite = ItemManager.Instance.GetPlayerItemSprite();
        playerLabText.text = "Lab : " + (GameManager.Instance.player.GetComponent<RankInfo>().curLap+1) + " / " + GameManager.Instance.targetLab;
        playerScoreText.text = "Score : " + GameManager.Instance.score;
    }

    void SetRankingUI()
    {
        RankingText.text = "";
        for (int i = 0; i < RankingManager.Instance.rankingList.Count; i++)
        {
            RankingText.text += (i+1) + "À§" + ":" + RankingManager.Instance.rankingList[i].name + "\n";
        }
    }
    IEnumerator OnGameStartText()
    {
        gameStartText.text = "3";
        yield return new WaitForSeconds(1);
        gameStartText.text = "2";
        yield return new WaitForSeconds(1);
        gameStartText.text = "1";
        yield return new WaitForSeconds(1);
        gameStartText.text = "GO!";
        GameManager.Instance.GameStart();
        yield return new WaitForSeconds(1);
        gameStartText.text = "";

        yield break;
    }
}
