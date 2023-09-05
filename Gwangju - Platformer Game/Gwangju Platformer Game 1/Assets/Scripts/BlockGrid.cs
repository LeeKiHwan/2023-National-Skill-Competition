using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlockGrid : MonoBehaviour
{
    private void Update()
    {
        Vector3 pos = transform.position;
        int x = (int)pos.x;
        int y = (int)pos.y;
        int z = (int)pos.z;
        transform.position = new Vector3(x,y,z);
    }
}
