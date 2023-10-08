using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionTower : BaseTower
{
    private void Awake()
    {
        Destroy(gameObject, 30);
    }

    public override void Attack()
    {
    }
}
