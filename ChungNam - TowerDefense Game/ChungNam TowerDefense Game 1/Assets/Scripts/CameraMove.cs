using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public float moveSpeed;
    public Rect map;
    public MeshRenderer mesh;

    private void Awake()
    {
        GetMapRect();
    }

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

        if (Input.mousePosition.x < 50) mouseX = -1;
        else if (Input.mousePosition.x > 1870) mouseX = 1;
        
        if (Input.mousePosition.y < 50) mouseY = -1;
        else if (Input.mousePosition.y > 1030) mouseY = 1;

        float x = mouseX * Time.deltaTime * moveSpeed;
        float y = mouseY * Time.deltaTime * moveSpeed;

        transform.Translate(x, 0, y);
    }

    void GetMapRect()
    {
        float width = mesh.bounds.size.x;
        float height = mesh.bounds.size.z;
        float x = mesh.transform.position.x - (width / 2);
        float z = mesh.transform.position.z - (height / 2);
        map = new Rect(new Vector2(x, z), new Vector2(width, height));
    }

    public void ClickMap(RawImage rawImage)
    {
        Vector3[] corners = new Vector3[4];
        rawImage.rectTransform.GetWorldCorners(corners);
        Rect newRect = new Rect(corners[0], corners[2] - corners[0]);
        Vector3 mP = Input.mousePosition;
        float x = (map.width * (mP.x - newRect.x) / newRect.width) + map.x;
        float z = (map.height * (mP.y - newRect.y) / newRect.height) + map.y;

        transform.position = new Vector3(x, transform.position.y, z-6);
    }
}
