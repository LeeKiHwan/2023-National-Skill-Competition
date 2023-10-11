using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour
{
    public WeaponType weaponType;

    private void Awake()
    {
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerAttackManager.Instance.GetWeapon(weaponType);
            Destroy(gameObject);
        }
    }
}
