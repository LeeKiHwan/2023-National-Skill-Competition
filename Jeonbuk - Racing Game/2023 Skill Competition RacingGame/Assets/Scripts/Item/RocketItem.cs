using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketItem : MonoBehaviour, IUsableItem
{
    public void UseItem(Car car)
    {
        if (car.GetComponent<RankInfo>() == RankingManager.Instance.rankingList[0]) return;

        Instantiate(ItemManager.Instance.RocketObject, RankingManager.Instance.rankingList[0].transform).GetComponent<RocketObject>().LockOn();
        RankingManager.Instance.rankingList[0].GetComponent<Car>().isLockOn = true;
        car.curItem = null;
    }
}
