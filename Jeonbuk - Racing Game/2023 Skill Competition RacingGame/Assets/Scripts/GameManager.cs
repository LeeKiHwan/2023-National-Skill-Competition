using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Player player;
    public int score;
    public bool isGameStart;
    public int targetLab;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(this);
            Debug.LogError("GameManager");
        }

        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void GameStart()
    {
        isGameStart = true;
    }
}
