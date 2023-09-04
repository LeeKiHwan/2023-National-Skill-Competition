using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public bool followPlayer;
    public Transform player; // �÷��̾��� ���� ��ü
    public Transform target; // �÷��̾� ���ο��� ī�޶� ��ġ�ؾ��� ���� �˷��ִ� ��ü
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