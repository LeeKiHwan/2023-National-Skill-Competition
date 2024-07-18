using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : MonoBehaviour, IUsableItem
{
    public void UseItem(Car car)
    {
        car.SetInvincibleTime(2);
        car.curItem = null;
    }
}
