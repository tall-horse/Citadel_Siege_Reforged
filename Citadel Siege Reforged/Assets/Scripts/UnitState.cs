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
    protected IProperty thisTarget;
    protected Health health;
    protected Damager damager;
    public UnitState(Animator _anim, ReachDetector _reachDetector, IProperty _thisTarget, Health _health)
    {
        anim = _anim;
        reachDetector = _reachDetector;
        thisTarget = _thisTarget;
        health = _health;
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
    public Idle(Animator _anim, ReachDetector _reachDetector, IProperty _thisTarget, Health _health, Damager _damager) : base(_anim, _reachDetector, _thisTarget, _health)
    {
        damager = _damager;
        damager.OnPursueNewTarget += () =>
        {
            nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        };
        currentState = UNITSTATE.IDLE;
        health.OnDead += TriggerDeath;
    }
    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }
    public override void Update()
    {
        nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
        eventState = EVENT.EXIT;
    }
    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
    }
    private void TriggerDeath()
    {
        nextState = new Die(anim, reachDetector, thisTarget, health);
        eventState = EVENT.EXIT;
    }
    public class Walk : UnitState
    {
        public Walk(Animator _anim, ReachDetector _reachDetector, IProperty _thisTarget, Health _health, Damager _damager) : base(_anim, _reachDetector, _thisTarget, _health)
        {
            damager = _damager;
            damager.OnPursueNewTarget += () =>
            {
                nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
                eventState = EVENT.EXIT;
            };
            currentState = UNITSTATE.WALK;
            reachDetector.OnTargetFound += StartAttacking;
            health.OnDead += TriggerDeath;
        }
        public override void Enter()
        {
            anim.SetTrigger("isWalk");
            base.Enter();
        }
        private void StartAttacking(IProperty target)
        {
            thisTarget.Target = target;
            nextState = new Attack(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalk");
            base.Exit();
        }
        private void TriggerDeath()
        {
            nextState = new Die(anim, reachDetector, thisTarget, health);
            eventState = EVENT.EXIT;
        }
    }
    public class Attack : UnitState
    {
        public Attack(Animator _anim, ReachDetector _reachDetector, IProperty _thisTarget, Health _health, Damager _damager)
        : base(_anim, _reachDetector, _thisTarget, _health)
        {
            damager = _damager;
            damager.OnPursueNewTarget += () =>
            {
                nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
                eventState = EVENT.EXIT;
            };
            currentState = UNITSTATE.ATTACK;
            health.OnDead += TriggerDeath;

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
        private void TriggerDeath()
        {
            nextState = new Die(anim, reachDetector, thisTarget, health);
            eventState = EVENT.EXIT;
        }
    }
    public class Die : UnitState
    {
        public Die(Animator _anim, ReachDetector _reachDetector, IProperty _thisTarget, Health _health) : base(_anim, _reachDetector, _thisTarget, _health)
        {
            currentState = UNITSTATE.DIE;
        }
        public override void Enter()
        {
            anim.SetTrigger("isDead");
            base.Enter();
        }
        public override void Exit()
        {
            anim.ResetTrigger("isDead");
            base.Exit();
        }
    }
}
