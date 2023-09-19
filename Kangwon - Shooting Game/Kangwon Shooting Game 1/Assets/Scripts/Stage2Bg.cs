using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2Bg : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y <= -10.2f)
        {
            transform.position = new Vector3(0, 11, 0);
        }
    }
}
