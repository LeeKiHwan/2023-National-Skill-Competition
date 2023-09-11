using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject target;
    public float speed;

    private void Awake()
    {
        Destroy(gameObject, 1);
    }

    private void Update()
    {
        if (target == null) Destroy(gameObject, 0.1f);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(GameObject target, float speed)
    {
        transform.LookAt(new Vector3(target.transform.position.x, 0.5f, target.transform.position.z));

        this.target = target;
        this.speed = speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            Destroy(gameObject);
        }
    }
}
