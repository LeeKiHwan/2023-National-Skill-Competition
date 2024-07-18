using UnityEngine;

public abstract class Car : MonoBehaviour
{
    protected float MaxSpeed { get => maxMoveSpeed + curBoostSpeed; }

    [SerializeField] protected Rigidbody rb;

    [Header("Move Speed")]
    [SerializeField] protected float baseMoveSpeed;
    protected float maxMoveSpeed;

    [Header("Boost")]
    [SerializeField] protected float boostSpeed;
    [SerializeField] protected GameObject boostEffect;
    protected float curBoostSpeed;

    protected Vector3 moveForce;

    private void Awake()
    {
        rb.transform.parent = null;
    }

    protected abstract void Move();
}
