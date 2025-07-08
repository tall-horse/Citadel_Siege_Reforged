using UnityEngine;

public class MeleeUnit : Unit
{
    private Animator animator;
    private ReachDetector reachDetector;
    private Health health;
    [field: SerializeField] private UnitState.UNITSTATE currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        reachDetector = GetComponent<ReachDetector>();
        health = GetComponent<Health>();
        UnitState = new Idle(animator, reachDetector, this, health);
        Itself = transform;
    }
    private void Update()
    {
        UnitState = UnitState.Process();
        currentState = UnitState.currentState;
    }
}
