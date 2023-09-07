using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public RectTransform bg;
    public RectTransform rankingBG;
    public AudioClip bgm;
    public List<GameObject> helpUIs;
    public int helpUIIdx;
    public GameObject helpUIPrevBtn;
    public GameObject helpUINextBtn;
    public AudioClip uiInteractSound;

    private void Start()
    {
        SoundManager.Instance.PlayBGM(bgm);
    }

    private void Update()
    {
        BGMove();
    }

    public void UIInteract()
    {
        SoundManager.Instance.PlaySFX(uiInteractSound, false);
    }

    public void GameStart()
    {
        InGameManager.score = 0;
        InGameManager.life = 3;
        InGameManager.curStage = 1;
        SceneManager.LoadScene("Stage1");
    }

    void BGMove()
    {
        bg.anchoredPosition = new Vector2(bg.anchoredPosition.x + -Time.deltaTime * 40, 0);

        if (bg.anchoredPosition.x < -1920) bg.anchoredPosition3D = Vector3.zero;
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

        for (int i=0; i<cnt; i++)
        {
            GameObject rankObj = Instantiate(RankingManager.Instance.rankObj, rankingBG);
            TextMeshProUGUI rankText = rankObj.GetComponentInChildren<TextMeshProUGUI>();
            rankText.text = "Name : "+RankingManager.Instance.ranking[i].name + "   Score : " +RankingManager.Instance.ranking[i].score;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void OnHelpUI()
    {
        helpUIIdx = 0;

        for (int i=0; i<helpUIs.Count; i++)
        {
            helpUIs[i].SetActive(false);
        }
        helpUIs[0].SetActive(true);

        helpUIPrevBtn.SetActive(false);
        helpUINextBtn.SetActive(true);
    }

    public void SetHelpUIBtn(int idx)
    {
        for (int i = 0; i < helpUIs.Count; i++)
        {
            helpUIs[i].SetActive(false);
        }
        helpUIIdx += idx;

        helpUIs[helpUIIdx].SetActive(true);

        if (helpUIIdx == 0) helpUIPrevBtn.SetActive(false);
        else if (helpUIIdx == helpUIs.Count - 1)
        {
            helpUINextBtn.SetActive(false);
        }
        else
        {
            helpUIPrevBtn.SetActive(true);
            helpUINextBtn.SetActive(true);
        }
    }
}
