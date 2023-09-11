using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void Update()
    {
        SetGameUI();
    }

    public void SetGameUI()
    {
        goldText.text = TowerManager.Instance.gold.ToString();
    }
}
