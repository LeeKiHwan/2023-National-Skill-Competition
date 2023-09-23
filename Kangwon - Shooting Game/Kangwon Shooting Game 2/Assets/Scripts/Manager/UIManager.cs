using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player")]
    public Image playerHp;
    public Image playerMp;
    public Image playerXp;
    public Image allAtkBg;
    public Image invcBg;
    public Image speedBg;
    public Image healBg;

    [Header("Game")]
    public Image fadeInOut;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bossSpawnTimeText;
    public Material uiMat;

    private void Awake()
    {
        Instance = this;

        StartCoroutine(FadeOutCo());
    }

    private void Update()
    {
        if (PlayerAttackManager.Instance.player)
        {
            SetPlayerUI();
        }
        SetGameUI();
        UIFadeInOut();
    }

    public void SetPlayerUI()
    {
        playerHp.fillAmount = (float)PlayerAttackManager.Instance.player.hp / PlayerAttackManager.Instance.player.maxHp;
        playerMp.fillAmount = (float)PlayerAttackManager.Instance.player.mp / PlayerAttackManager.Instance.player.maxMp;
        playerXp.fillAmount = (float)PlayerAttackManager.Instance.xp / PlayerAttackManager.Instance.maxXp;

        allAtkBg.fillAmount = PlayerAttackManager.Instance.allAtkCur / PlayerAttackManager.Instance.allAtkCool;
        invcBg.fillAmount = PlayerAttackManager.Instance.invcCur / PlayerAttackManager.Instance.invcCool;
        speedBg.fillAmount = PlayerAttackManager.Instance.speedCur / PlayerAttackManager.Instance.speedCool;
        healBg.fillAmount = PlayerAttackManager.Instance.healCur / PlayerAttackManager.Instance.healCool;
    }

    public void SetGameUI()
    {
        scoreText.text = "Score : " + InGameManager.score;

        if (InGameManager.Instance.bossSpawnTime > 0 )
        {
            bossSpawnTimeText.text = "Boss " + (int)(InGameManager.Instance.bossSpawnTime / 60) + ":" + (int)(InGameManager.Instance.bossSpawnTime % 60);
        }
        else
        {
            bossSpawnTimeText.text = "";
        }
    }

    public IEnumerator FadeOutCo()
    {
        fadeInOut.color = Color.black;
        while (fadeInOut.color.a > 0)
        {
            fadeInOut.color = new Color(0,0,0, fadeInOut.color.a - Time.deltaTime);
            yield return null;
        }

        yield break;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInCo());
    }

    public IEnumerator FadeInCo()
    {
        fadeInOut.color = Color.clear;
        while (fadeInOut.color.a < 1)
        {
            fadeInOut.color = new Color(0, 0, 0, fadeInOut.color.a + Time.deltaTime);
            yield return null;
        }

        yield break;
    }

    public void UIFadeInOut()
    {
        if (InGameManager.Instance.curStage == 1)
        {
            if (Physics2D.OverlapBoxAll(new Vector2(-6f, 3), new Vector2(5, 3), 0).Length > 0)
            {
                uiMat.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                uiMat.color = Color.white;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(new Vector2(-6f, 3), new Vector2(5, 3));
    }
}
