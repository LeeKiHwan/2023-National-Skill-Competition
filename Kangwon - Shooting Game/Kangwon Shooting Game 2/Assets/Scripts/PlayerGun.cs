using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    public static Sprite playerGun;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
