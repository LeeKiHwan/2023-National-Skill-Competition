using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI goldText;
    public TextMeshProUGUI waveReadyTimeText;
    public TextMeshProUGUI waveText;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        SetGameUI();
    }

    public void SetGameUI()
    {
        goldText.text = TowerManager.Instance.gold.ToString();

        if (InGameManager.Instance.waveReadyTime > 0) waveReadyTimeText.text = "다음 웨이브까지 " + (int)InGameManager.Instance.waveReadyTime + "초 남음";
        else waveReadyTimeText.text = "";
    }

    public void StartWaveUI()
    {
        StartCoroutine(StartWaveUICo());
    }

    IEnumerator StartWaveUICo()
    {
        waveText.text = "웨이브 " + (InGameManager.Instance.curWave + 1);
        yield return new WaitForSeconds(2);
        waveText.text = "";
        InGameManager.Instance.WaveStart();

        yield break;
    }
}
