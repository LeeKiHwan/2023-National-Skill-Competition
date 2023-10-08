using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionTower : BaseTower
{
    [Header("IllusionTower Data")]
    public float destroyTime;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    public override void Attack()
    {
    }
}
