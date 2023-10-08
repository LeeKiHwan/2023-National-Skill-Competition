using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public BaseTower pf;
    public float pfMaxHp;
    public bool destroyedPf;

    [Header("InGame")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI nextWaveTimeText;
    public TextMeshProUGUI protectFacilityHpText;
    public TextMeshProUGUI scoreText;
    public Image protectFacilityHpImage;
    public GameObject mapUI;
    public GameObject complete;
    public GameObject gameOver;
    public RectTransform explainUI;
    public TextMeshProUGUI explainText;
    public Transform itemUI;
    public TextMeshProUGUI itemText;

    private void Awake()
    {
        Cursor.visible = true;

        Instance = this;

        pfMaxHp = pf.maxHp;
    }

    private void Update()
    {

        SetInGameUI();
        GameOverCheck();
    }

    public void OnStageClearUI()
    {
        StartCoroutine(OnStageClearUICo());
    }

    public IEnumerator OnStageClearUICo()
    {
        complete.SetActive(true);
        yield return new WaitForSeconds(2);
        GameManager.Instance.NextStage();

        yield break;
    }

    public void GameOverCheck()
    {
        if (!pf && !destroyedPf)
        {
            StartCoroutine(GameOver());
            destroyedPf = true;
        }
    }

    public IEnumerator GameOver()
    {
        gameOver.SetActive(true);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("EndGame");

        yield break;
    }

    public void SetInGameUI()
    {

        goldText.text = TowerManager.Instance.gold.ToString();
        scoreText.text = GameManager.score + " P";
        if (GameManager.Instance.waveCur > 0) nextWaveTimeText.text = "Next Wave  " + (int)(GameManager.Instance.waveCur/60) + ":" + (int)(GameManager.Instance.waveCur%60);
        waveText.text =  "Wave " + (GameManager.Instance.curWave);
        protectFacilityHpText.text = (int)pf.hp + " / " + (int)pfMaxHp;
        protectFacilityHpImage.fillAmount = pf.hp / pfMaxHp;

        if (Input.GetKey(KeyCode.Tab)) mapUI.SetActive(true);
        else mapUI.SetActive(false);

    }

    public void Explain(string[] text)
    {
        StartCoroutine(ExplainCo(text));
    }

    public IEnumerator ExplainCo(string[] text)
    {
        explainText.text = "";

        while (explainUI.anchoredPosition.x < -650)
        {
            explainUI.anchoredPosition = new Vector3(explainUI.anchoredPosition.x + 5, explainUI.anchoredPosition.y, 0);
            yield return null;
        }

        for (int i = 0; i < text.Length;i++)
        {
            explainText.text = "";
            for (int j=0; j < text[i].Length;j++)
            {
                explainText.text += text[i][j];
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(1.5f);
        }

        while (explainUI.anchoredPosition.x > -1250)
        {
            explainUI.anchoredPosition = new Vector3(explainUI.anchoredPosition.x - 5, explainUI.anchoredPosition.y, 0);
            yield return null;
        }

        yield break;
    }

    public void ItemUI(string Text, Color color)
    {
        StartCoroutine(ItemUICo(Text, color));
    }

    public IEnumerator ItemUICo(string text, Color color)
    {
        itemUI.gameObject.SetActive(true);
        itemText.text = text;
        itemText.color = color;

        while (itemUI.localScale.x < 1)
        {
            itemUI.localScale = new Vector3(itemUI.localScale.x + Time.deltaTime * 5, itemUI.localScale.x + Time.deltaTime * 5, 1);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (itemUI.localScale.x > 0)
        {
            itemUI.localScale = new Vector3(itemUI.localScale.x - Time.deltaTime * 5, itemUI.localScale.x - Time.deltaTime * 5, 1);
            yield return null;
        }
        
        itemUI.gameObject.SetActive(false);

        yield break;
    }
}
