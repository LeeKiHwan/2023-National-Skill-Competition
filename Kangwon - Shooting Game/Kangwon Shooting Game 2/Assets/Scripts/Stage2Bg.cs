using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Bg : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -20)
        {
            transform.position = new Vector3(0, 10.3f, 0);
        }

        transform.Translate(Vector2.down * Time.deltaTime);
    }
}
