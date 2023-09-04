using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankInfo
{
    public int score;
    public string name;
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    public List<RankInfo> ranking = new List<RankInfo>();
    public GameObject rankInfoText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void InsertRanking(RankInfo rankInfo)
    {
        ranking.Add(rankInfo);
        ranking.OrderBy(s => s.score);
    }
}
