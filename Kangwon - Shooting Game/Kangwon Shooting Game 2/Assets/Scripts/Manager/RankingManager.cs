using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class RankInfo
{
    public string Name;
    public int Score;
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    public List<RankInfo> ranking = new List<RankInfo>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InsertRank(RankInfo rankInfo)
    {
        ranking.Add(rankInfo);
        ranking = ranking.OrderByDescending(s => s.Score).ToList();
    }
}
