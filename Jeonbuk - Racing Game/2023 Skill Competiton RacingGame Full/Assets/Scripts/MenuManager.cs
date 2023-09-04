using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject ai;
    public float spawnTime;

    public RectTransform rankingBG;
    public AudioClip menuBGM;


    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM(menuBGM);
    }

    public void StartGame()
    {
        InGameManager.score = 0;
        InGameManager.curStage = 0;
        SceneManager.LoadScene("Stage1");
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
