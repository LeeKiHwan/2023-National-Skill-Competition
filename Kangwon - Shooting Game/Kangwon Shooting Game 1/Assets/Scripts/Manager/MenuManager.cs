using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerAttackManager.Instance)
        {
            Destroy(PlayerAttackManager.Instance.gameObject);
        }
    }

    public void StartGame()
    {
        InGameManager.curStage = 0;
        SceneManager.LoadScene("Stage1");
    }
}
