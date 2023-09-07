using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public List<GameObject> lifes;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public Image playerItemImage;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SetGameUI();
    }

    void SetGameUI()
    {
        for (int i = 0; i < lifes.Count; i++)
        {
            if (i < InGameManager.life) lifes[i].SetActive(true);
            else lifes[i].SetActive(false);
        }

        scoreText.text = "Score : " + InGameManager.score;
        timeText.text = "Time : " + ((int)InGameManager.Instance.time / 60) + ":" + ((int)InGameManager.Instance.time % 60);

        switch (InGameManager.Instance.player.curItem)
        {
            case ItemType.DoubleJumpItem:
                break;
            case ItemType.DashItem:
                break;
            case ItemType.RotateItem:
                break;
            case ItemType.None:
                break;
        }
    }
}
