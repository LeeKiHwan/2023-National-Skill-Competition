using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RankInfo
{
    public int score;
    public string name;
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    public List<RankInfo> ranking = new List<RankInfo>();
    public GameObject rankObj;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void InsertRank(RankInfo rankInfo)
    {
        ranking.Add(rankInfo);
        ranking = ranking.OrderByDescending(r => r.score).ToList();
    }
}
