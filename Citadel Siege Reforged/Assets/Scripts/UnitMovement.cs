using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    private Rigidbody rb;
    private Unit unit;
    private float indirectMultipliyer = 10f;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (unit.UnitState.currentState != UnitState.UNITSTATE.WALK) return;
        rb.linearVelocity = transform.forward * speed * indirectMultipliyer * Time.deltaTime;
    }
}
