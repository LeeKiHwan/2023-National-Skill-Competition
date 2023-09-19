using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Slider hpSlider;
    public Slider mpSlider;
    public Image xpImage;
    public Image allAttackBg;
    public Image invcBg;
    public Image speedUpBg;
    public Image healBg;
    public GameObject[] attackSkills;
    public GameObject baseSkill;

    [Header("Game UI")]
    public TextMeshProUGUI bossSpawnTimeText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PlayerAttackManager.Instance.player)
        {
            SetPlayerUI();
        }
        SetGameUI();
    }

    public void SetPlayerUI()
    {
        hpSlider.value = (float)PlayerAttackManager.Instance.player.hp / PlayerAttackManager.Instance.player.maxHp;
        mpSlider.value = (float)PlayerAttackManager.Instance.player.mp / PlayerAttackManager.Instance.player.maxMp;
        xpImage.fillAmount = (float)PlayerAttackManager.Instance.xp / PlayerAttackManager.Instance.maxXp;

        allAttackBg.fillAmount = PlayerAttackManager.Instance.allAttckCurTime / PlayerAttackManager.Instance.allAttckCoolTime;
        invcBg.fillAmount = PlayerAttackManager.Instance.invcCurTime / PlayerAttackManager.Instance.invcCoolTime;
        speedUpBg.fillAmount = PlayerAttackManager.Instance.speedUpCurTime / PlayerAttackManager.Instance.speedUpCoolTime;
        healBg.fillAmount = PlayerAttackManager.Instance.healCurTime / PlayerAttackManager.Instance.healCoolTime;
    }

    public void SetGameUI()
    {
        if (InGameManager.Instance.bossSpawnTime > 0)
        {
            bossSpawnTimeText.text = "Boss  " + ((int)InGameManager.Instance.bossSpawnTime / 60) + ":" + ((int)InGameManager.Instance.bossSpawnTime % 60);
        }
        else bossSpawnTimeText.text = "";
    }

    public void LevelUpUI()
    {
        Time.timeScale = 0;
        List<int> showIdx= new List<int>();

        while (showIdx.Count < 3)
        {
            int idx = Random.Range(0, attackSkills.Length);
            if (!showIdx.Contains(idx))
            {
                showIdx.Add(idx);
            }
        }

        for (int i=0; i<attackSkills.Length; i++)
        {
            if (showIdx.Contains(i))
            {
                attackSkills[i].SetActive(true);
            }
        }

        baseSkill.SetActive(true);
    }

    public void SelectSkill()
    {
        baseSkill.SetActive(false);
        for (int i=0; i<attackSkills.Length;i++)
        {
            attackSkills[i].SetActive(false);
        }
        Time.timeScale = 1;
    }
}
