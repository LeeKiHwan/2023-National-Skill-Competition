using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    DoubleJumpItem,
    DashItem,
    RotateItem
}

public class Item : MonoBehaviour
{
    public ItemType itemType;
}
