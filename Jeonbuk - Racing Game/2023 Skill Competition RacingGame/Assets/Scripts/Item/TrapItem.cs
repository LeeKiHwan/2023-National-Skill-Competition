using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapItem : MonoBehaviour, IUsableItem
{
    public void UseItem(Car car)
    {
        Vector3 itemSpawnPos = car.transform.position + -car.transform.up;

        if (car.riderType == RiderType.Player)
        {
            Instantiate(ItemManager.Instance.TrapObject, itemSpawnPos, Quaternion.identity).GetComponent<TrapObject>().Spawn(RiderType.Player, car.transform.rotation.eulerAngles);
        }
        else
        {
            Instantiate(ItemManager.Instance.EnemyTrapObject, itemSpawnPos, Quaternion.identity).GetComponent<TrapObject>().Spawn(RiderType.Enemy, car.transform.rotation.eulerAngles);
        }
        car.curItem = null;
    }
}
