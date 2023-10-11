using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

[System.Serializable]
public class RankInfo
{
    public string Name;
    public int Score;
}

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;
    public List<RankInfo> ranking;

    public string filePath;

    private void Awake()
    {
        GetFilePath();
        if (File.Exists(filePath))
            LoadCSVFile();

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

        SaveCSVFile();
    }

    public void GetFilePath()
    {
        string str = "";
        var pathArray = Application.dataPath.Split('/');
        for (int i = 0; i < pathArray.Length - 1; i++)
        {
            str += $"{pathArray[i]}/";
        }
        filePath = str+"rank.txt";
        Debug.Log(str);
    }

    public void LoadCSVFile()
    {
        ranking.Clear();
        foreach (string line in File.ReadAllText(filePath).Trim().Split("\n"))
        {
            var value = line.Split(',');
            ranking.Add(new RankInfo { Name = value[1], Score = Int32.Parse(value[2]) });
            Debug.Log(line);
        }
        ranking = ranking.OrderByDescending(s => s.Score).ToList();
        SaveCSVFile();
    }

    public void SaveCSVFile()
    {
        string saveText = "";
        for (int i = 0; i < ranking.Count; i++)
        {
            saveText += $"{i + 1},{ranking[i].Name},{ranking[i].Score}\n";
        }
        File.WriteAllText(filePath, saveText);
    }
}
