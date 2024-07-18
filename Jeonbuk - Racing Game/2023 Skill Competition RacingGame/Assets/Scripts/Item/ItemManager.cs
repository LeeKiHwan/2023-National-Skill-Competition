using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [Header("Item Object")]
    public GameObject RandomItem;
    public GameObject TrapObject;
    public GameObject EnemyTrapObject;
    public GameObject RocketObject;

    [Header("Item Sprite")]
    public Sprite TrapSprite;
    public Sprite RocketSprite;
    public Sprite ShieldSprite;
    public Sprite NullSprite;

    private void Awake()
    {
        Instance = this;
    }

    public Sprite GetPlayerItemSprite()
    {
        switch (GameManager.Instance.player.curItem)
        {
            case TrapItem:
                return TrapSprite;
            case RocketItem:
                return RocketSprite;
            case ShieldItem:
                return ShieldSprite;
        }
        return NullSprite;
    }
}
