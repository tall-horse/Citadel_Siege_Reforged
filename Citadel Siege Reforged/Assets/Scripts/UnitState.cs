using UnityEngine;

public class UnitState
{
    public enum UNITSTATE
    {
        IDLE, WALK, ATTACK, DIE
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public UNITSTATE currentState;
    protected EVENT eventState;
    protected Animator anim;
    protected UnitState nextState;
    protected ReachDetector reachDetector;
    protected IHealth thisTarget;
    public UnitState(Animator _anim, ReachDetector _reachDetector, IHealth _thisTarget)
    {
        anim = _anim;
        reachDetector = _reachDetector;
        thisTarget = _thisTarget;
        eventState = EVENT.ENTER;
    }
    public virtual void Enter() { eventState = EVENT.UPDATE; }

    public virtual void Update() { eventState = EVENT.UPDATE; }

    public virtual void Exit() { eventState = EVENT.EXIT; }

    public UnitState Process()
    {
        if (eventState == EVENT.ENTER) Enter();
        if (eventState == EVENT.UPDATE) Update();
        if (eventState == EVENT.EXIT)
        {
            Exit();
            return nextState;
        }
        return this;
    }

}
public class Idle : UnitState
{
    public Idle(Animator _anim, ReachDetector _reachDetector, IHealth _thisTarget) : base(_anim, _reachDetector, _thisTarget)
    {
        currentState = UNITSTATE.IDLE;
    }
    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }
    public override void Update()
    {
        nextState = new Walk(anim, reachDetector, thisTarget);
        eventState = EVENT.EXIT;
    }
    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
    public class Walk : UnitState
    {
        public Walk(Animator _anim, ReachDetector _reachDetector, IHealth _thisTarget) : base(_anim, _reachDetector, _thisTarget)
        {
            currentState = UNITSTATE.WALK;
            reachDetector.OnTargetFound += StartAttacking;
        }
        public override void Enter()
        {
            anim.SetTrigger("isWalk");
            base.Enter();
        }
        private void StartAttacking(IHealth target)
        {
            thisTarget.Target = target;
            nextState = new Attack(anim, reachDetector, thisTarget);
            eventState = EVENT.EXIT;
        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalk");
            base.Exit();
        }
    }
    public class Attack : UnitState
    {
        public Attack(Animator _anim, ReachDetector _reachDetector, IHealth _thisTarget) : base(_anim, _reachDetector, _thisTarget)
        {
            currentState = UNITSTATE.ATTACK;
        }
        public override void Enter()
        {
            anim.SetTrigger("isAttack");
            base.Enter();
        }
        public override void Exit()
        {
            anim.ResetTrigger("isAttack");
            base.Exit();
        }
    }
}
