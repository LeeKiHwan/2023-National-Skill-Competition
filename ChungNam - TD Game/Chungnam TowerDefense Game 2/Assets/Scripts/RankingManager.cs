using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class RankInfo
{
    public string Name;
    public int score;
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    public List<RankInfo> ranking;

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
        ranking = ranking.OrderByDescending(s => s.score).ToList();
    }
}
