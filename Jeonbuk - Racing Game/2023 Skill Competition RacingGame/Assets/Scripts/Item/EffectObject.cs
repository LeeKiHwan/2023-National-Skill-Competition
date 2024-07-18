using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectObject : MonoBehaviour
{
    private void Awake()
    {
    }

    public void SetDestroyTime(float time)
    {
        Destroy(gameObject, time);
    }
}
