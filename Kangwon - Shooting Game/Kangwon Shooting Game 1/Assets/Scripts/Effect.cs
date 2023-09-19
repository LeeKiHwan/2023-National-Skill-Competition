using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public void SetDestroyTime(float time)
    {
        Destroy(gameObject, time);
    }
}
