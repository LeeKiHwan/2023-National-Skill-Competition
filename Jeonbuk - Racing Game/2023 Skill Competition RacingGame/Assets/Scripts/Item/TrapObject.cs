using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    RiderType master;
    [SerializeField] float trapedTime;
    [SerializeField] float destroyTime;

    private void Awake()
    {
        Destroy(gameObject, destroyTime);
    }

    public void Spawn(RiderType master, Vector3 Rot)
    {
        this.master = master;
        transform.rotation = Quaternion.Euler(Rot);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Car>() != null && collision.GetComponent<Car>().riderType != master)
        {
            collision.GetComponent<Car>().Traped(trapedTime);
            Destroy(gameObject);
        }
    }
}
