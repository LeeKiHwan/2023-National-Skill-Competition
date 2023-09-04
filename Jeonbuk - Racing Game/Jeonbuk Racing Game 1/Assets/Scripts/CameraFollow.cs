using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool followPlayer;
    public Transform player; // 플레이어의 상위 객체
    public Transform target; // 플레이어 내부에서 카메라가 위치해야할 곳을 알려주는 객체
    public float followSpeed;

    void Update()
    {
        if (followPlayer)
            PlayerFollow();
    }

    void PlayerFollow()
    {
        transform.LookAt(player.position);

        transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);
    }
}