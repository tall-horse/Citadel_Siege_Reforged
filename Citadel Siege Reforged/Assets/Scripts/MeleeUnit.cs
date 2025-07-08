using UnityEngine;

public class MeleeUnit : Unit
{
    private Animator animator;
    private ReachDetector reachDetector;
    [field: SerializeField] private UnitState.UNITSTATE currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        reachDetector = GetComponent<ReachDetector>();
        UnitState = new Idle(animator, reachDetector, this);
        Itself = transform;
    }
    private void Update()
    {
        UnitState = UnitState.Process();
        currentState = UnitState.currentState;
    }
}
