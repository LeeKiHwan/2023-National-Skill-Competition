using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int pointNum;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RankInfo>() != null)
        {
            if (collision.GetComponent<RankInfo>().curCheckPoint + 1 == pointNum)
            {
                RankingManager.Instance.CheckPoint(collision.GetComponent<RankInfo>(), pointNum);
            }
            else
            {
            }
        }
    }
}
