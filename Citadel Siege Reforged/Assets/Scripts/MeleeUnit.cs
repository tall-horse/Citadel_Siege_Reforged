using UnityEngine;

public class MeleeUnit : MonoBehaviour, IHealth
{
    private Animator animator;
    private UnitState unitState;
    private ReachDetector reachDetector;
    public IHealth Target { get; set; }
    [field: SerializeField] UnitState.UNITSTATE currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        reachDetector = GetComponent<ReachDetector>();
        unitState = new Idle(animator, reachDetector, this);
    }
    private void Update()
    {
        unitState = unitState.Process();
        currentState = unitState.unitState;
    }
}
