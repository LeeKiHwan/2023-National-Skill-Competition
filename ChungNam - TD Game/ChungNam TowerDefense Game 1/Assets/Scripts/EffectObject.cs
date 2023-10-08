using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    public float destroyTime;

    public void SetDestroy(float destroyTime)
    {
        this.destroyTime = destroyTime;
        Destroy(gameObject);
    }
}
