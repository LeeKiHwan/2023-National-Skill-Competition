using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ItemType
{
    Trap,
    Bomb,
    Shield,
    ChargeBoost,
    None
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public GameObject TrapObj;
    public GameObject BombTargetedObj;
    public GameObject BombEffectObj;
    public GameObject ShieldEffectObj;

    [Header("Item Image")]
    public Sprite TrapImage;
    public Sprite BombImage;
    public Sprite ShieldImage;
    public Sprite ChargeBoostImage;
    public Sprite NoneImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public Sprite GetPlayerItemImage()
    {
        switch (InGameManager.Instance.player.curItem)
        {
            case ItemType.Trap:
                return TrapImage;
            case ItemType.Bomb:
                return BombImage;
            case ItemType.Shield:
                return ShieldImage;
            case ItemType.ChargeBoost:
                return ChargeBoostImage;
            case ItemType.None:
                return NoneImage;
        }

        return null;
    }
}
