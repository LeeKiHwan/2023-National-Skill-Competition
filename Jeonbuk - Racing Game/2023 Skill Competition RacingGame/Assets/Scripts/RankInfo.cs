using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankInfo : MonoBehaviour
{
    public int curLap;
    public int curCheckPoint;
    public float nextCheckPointDis;

    private void Update()
    {
        SetNextCheckPointDis();
    }

    void SetNextCheckPointDis()
    {
        nextCheckPointDis = Vector2.Distance(transform.position, RankingManager.Instance.GetNextCheckPointTransform(curCheckPoint));
    }
}
