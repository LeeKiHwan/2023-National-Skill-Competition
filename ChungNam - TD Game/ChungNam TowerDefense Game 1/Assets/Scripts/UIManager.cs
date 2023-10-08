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

        if (InGameManager.Instance.waveReadyTime > 0) waveReadyTimeText.text = "���� ���̺���� " + (int)InGameManager.Instance.waveReadyTime + "�� ����";
        else waveReadyTimeText.text = "";
    }

    public void StartWaveUI()
    {
        StartCoroutine(StartWaveUICo());
    }

    IEnumerator StartWaveUICo()
    {
        waveText.text = "���̺� " + (InGameManager.Instance.curWave + 1);
        yield return new WaitForSeconds(2);
        waveText.text = "";
        InGameManager.Instance.WaveStart();

        yield break;
    }
}
