using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RacingRankingManager : MonoBehaviour
{
    public static RacingRankingManager Instance;

    public List<Car> racingRanking;
    public GameObject CheckPointsObj;
    public List<CheckPoint> checkPoints;

    private void Awake()
    {
        Instance = this;

        foreach(CheckPoint checkPoint in CheckPointsObj.GetComponentsInChildren<CheckPoint>())
        {
            checkPoints.Add(checkPoint);
        }

        foreach(Car car in FindObjectsOfType<Car>())
        {
            racingRanking.Add(car);
        }
    }

    public void CheckPoint(Car car, int point)
    {
        racingRanking.Find(p => p == car).curCheckPoint = point;
    }

    public void Update()
    {
        SetRacingRanking();
    }

    public void SetRacingRanking()
    {
        for (int i = racingRanking.Count - 1; i > 0; i--)
        {
            if (racingRanking[i].curCheckPoint > racingRanking[i - 1].curCheckPoint)
            {
                Car temp = racingRanking[i];
                racingRanking[i] = racingRanking[i - 1];
                racingRanking[i - 1] = temp;
            }
        }

        for (int i = racingRanking.Count - 1; i > 0; i--)
        {
            if (racingRanking[i].curCheckPoint == racingRanking[i-1].curCheckPoint && 
                GetNextCheckPointDis(racingRanking[i].transform.position, racingRanking[i].curCheckPoint + 1) < GetNextCheckPointDis(racingRanking[i-1].transform.position, racingRanking[i-1].curCheckPoint + 1))
            {
                Car temp = racingRanking[i];
                racingRanking[i] = racingRanking[i-1];
                racingRanking[i-1] = temp;
            }
        }
    }

    public float GetNextCheckPointDis(Vector3 curPos, int nextCheckPoint)
    {
        Vector3 ncpPos = checkPoints.Find(p => p.checkPointNum == nextCheckPoint) ? checkPoints.Find(p => p.checkPointNum == nextCheckPoint).transform.position : Vector3.zero;
        return Vector3.Distance(curPos, ncpPos);
    }
}
