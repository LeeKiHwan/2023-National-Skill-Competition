using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int checkPointNum;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Car>())
        {
            RacingRankingManager.Instance.CheckPoint(other.gameObject.GetComponent<Car>(), checkPointNum);
        }
    }
}
