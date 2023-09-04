using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObj : MonoBehaviour
{
    public void SetDestroy(float destroyTime)
    {
        Destroy(gameObject, destroyTime);
    }
}
