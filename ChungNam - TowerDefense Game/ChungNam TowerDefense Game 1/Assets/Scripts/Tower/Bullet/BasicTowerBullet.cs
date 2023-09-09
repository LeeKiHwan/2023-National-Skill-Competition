using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasicTowerBullet : MonoBehaviour
{
    public BaseEnemy targetEnemy;
    public float speed;

    private void Awake()
    {
        Destroy(gameObject, 1);
    }

    private void Update()
    {
        if (targetEnemy == null) Destroy(gameObject);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(BaseEnemy targetEnemy, float speed)
    {
        transform.LookAt(new Vector3(targetEnemy.transform.position.x, 0.5f, targetEnemy.transform.position.z));

        this.targetEnemy = targetEnemy;
        this.speed = speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BaseEnemy>() == targetEnemy)
        {
            Destroy(gameObject);
        }
    }
}
