using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    private Rigidbody rb;
    private Unit unit;
    private float indirectMultipliyer = 10f;
    private Health health;
    private Collider collider;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();
        health = GetComponent<Health>();
        collider = GetComponent<Collider>();
    }
    private void Start()
    {
        health.OnDead += DisablePhysics;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unit.UnitState.currentState != UnitState.UNITSTATE.WALK) return;
        rb.linearVelocity = transform.forward * speed * indirectMultipliyer * Time.deltaTime;
    }

    private void DisablePhysics()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        collider.enabled = false;
    }
}
