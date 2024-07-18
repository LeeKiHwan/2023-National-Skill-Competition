using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    public List<RankInfo> rankingList = new List<RankInfo>();
    public GameObject CheckPointsObj;
    public List<CheckPoint> checkPoints = new List<CheckPoint>();


    private void Awake()
    {
        Instance = this;

        foreach(RankInfo car in FindObjectsOfType<RankInfo>())
        {
            rankingList.Add(car);
        }

        foreach(CheckPoint checkPoint in CheckPointsObj.GetComponentsInChildren<CheckPoint>())
        {
            checkPoints.Add(checkPoint);
        }
    }

    private void Update()
    {
        SetRanking();
    }

    void SetRanking()
    {
        rankingList = rankingList.OrderByDescending(p => p.curLap).ToList();

        for (int i=0; i < rankingList.Count - 1; i++)
        {
            if (rankingList[i].curCheckPoint == rankingList[i+1].curCheckPoint && rankingList[i].curLap == rankingList[i+1].curLap)
            {
                if (rankingList[i].nextCheckPointDis > rankingList[i+1].nextCheckPointDis)
                {
                    RankInfo temp = rankingList[i];
                    rankingList[i] = rankingList[i+1];
                    rankingList[i+1] = temp;
                }
            }
            else if (rankingList[i].curCheckPoint < rankingList[i+1].curCheckPoint && rankingList[i].curLap == rankingList[i + 1].curLap)
            {
                RankInfo temp = rankingList[i];
                rankingList[i] = rankingList[i + 1];
                rankingList[i + 1] = temp;
            }
        }
    }

    public void CheckPoint(RankInfo car, int pointNum)
    {
        if (pointNum == checkPoints.Count)
        {
            CheckLab(car);
        }
        else
        {
            rankingList.Find(x => x == car).curCheckPoint = pointNum;
        }
    }

    public void CheckLab(RankInfo car)
    {
        if (car.curLap+1 == GameManager.Instance.targetLab)
        {
            car.GetComponent<Car>().isFinish = true;
        }
        car.curLap += 1;
        car.curCheckPoint = 0;
    }

    public Vector3 GetNextCheckPointTransform(int curCheckPoint)
    {
        foreach(CheckPoint checkPoint in checkPoints)
        {
            if (checkPoint.pointNum == curCheckPoint+1)
            {
                return checkPoint.transform.position;
            }
        }
        return Vector3.zero;
    }
}
