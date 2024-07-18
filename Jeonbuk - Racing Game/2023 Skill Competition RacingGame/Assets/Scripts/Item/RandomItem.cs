using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour, IGetableItem
{
    public void GetItem(Car car)
    {
        int itemIdx = Random.Range(0, 3);
        switch (itemIdx)
        {
            case 0:
                car.curItem = new TrapItem();
                break;
            case 1:
                car.curItem = new RocketItem();
                break;
            case 2:
                car.curItem = new ShieldItem();
                break;
        }
        Destroy(gameObject);
    }
}
