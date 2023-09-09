using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;

    private void Update()
    {
        KeyMove();
        MouseMove();
    }

    void KeyMove()
    {
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * moveSpeed;
        float z = Input.GetAxisRaw("Vertical") * Time.deltaTime * moveSpeed;
        transform.Translate(x,0,z);
    }

    void MouseMove()
    {
        float mouseX = 0;
        float mouseY = 0;

        if (Input.mousePosition.x < 100) mouseX = -1;
        else if (Input.mousePosition.x > 1820) mouseX = 1;
        
        if (Input.mousePosition.y < 100) mouseY = -1;
        else if (Input.mousePosition.y > 980) mouseY = 1;

        float x = mouseX * Time.deltaTime * moveSpeed;
        float y = mouseY * Time.deltaTime * moveSpeed;

        transform.Translate(x, 0, y);
    }
}
