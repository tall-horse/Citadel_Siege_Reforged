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
    protected Unit thisTarget;
    protected Health health;
    protected Damager damager;
    public UnitState(Animator _anim, ReachDetector _reachDetector, Unit _thisTarget, Health _health)
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
    public Idle(Animator _anim, ReachDetector _reachDetector, Unit _thisTarget, Health _health, Damager _damager) : base(_anim, _reachDetector, _thisTarget, _health)
    {
        damager = _damager;
        damager.OnPursueNewTarget += TransitToWalk;
        currentState = UNITSTATE.IDLE;
        health.OnDead += TriggerDeath;
        thisTarget.OnStopped += SetIdle;
    }

    private void TransitToWalk()
    {
        nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
        eventState = EVENT.EXIT;
    }

    public override void Enter()
    {
        anim.SetTrigger("isIdle");
        base.Enter();
    }
    public override void Exit()
    {
        anim.ResetTrigger("isIdle");
        base.Exit();
        damager.OnPursueNewTarget -= TransitToWalk;
        health.OnDead -= TriggerDeath;
        thisTarget.OnStopped -= SetIdle;
    }
    private void TriggerDeath()
    {
        nextState = new Die(anim, reachDetector, thisTarget, health);
        eventState = EVENT.EXIT;
    }
    private void SetIdle()
    {
        nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
        eventState = EVENT.EXIT;
    }
    public class Walk : UnitState
    {
        public Walk(Animator _anim, ReachDetector _reachDetector, Unit _thisTarget, Health _health, Damager _damager) : base(_anim, _reachDetector, _thisTarget, _health)
        {
            damager = _damager;
            damager.OnPursueNewTarget += TransitToWalk;
            currentState = UNITSTATE.WALK;
            reachDetector.OnTargetFound += StartAttacking;
            health.OnDead += TriggerDeath;
            thisTarget.OnStopped += SetIdle;
        }
        public override void Enter()
        {
            anim.SetTrigger("isWalk");
            base.Enter();
        }
        private void TransitToWalk()
        {
            nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
        private void StartAttacking(IProperty target)
        {
            thisTarget.Target = target;
            var targetHealth = target.Itself.GetComponent<Health>();
            targetHealth.OnDead += OnTargetDied;
            nextState = new Attack(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
        private void OnTargetDied()
        {
            if (thisTarget.Target != null)
            {
                var deadTargetHealth = thisTarget.Target.Itself.GetComponent<Health>();
                deadTargetHealth.OnDead -= OnTargetDied; // Clean unsubscription
            }

            thisTarget.Target = null;
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager); // Or Walk if you want
            eventState = EVENT.EXIT;
        }
        public override void Exit()
        {
            anim.ResetTrigger("isWalk");
            base.Exit();
            reachDetector.OnTargetFound -= StartAttacking;
            health.OnDead -= TriggerDeath;
            thisTarget.OnStopped -= SetIdle;
        }
        private void TriggerDeath()
        {
            nextState = new Die(anim, reachDetector, thisTarget, health);
            eventState = EVENT.EXIT;
        }
        private void SetIdle()
        {
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
    }
    public class Attack : UnitState
    {
        private Health targetHealth; // 🔸 Store reference for unsubscribe

        public Attack(Animator _anim, ReachDetector _reachDetector, Unit _thisTarget, Health _health, Damager _damager)
            : base(_anim, _reachDetector, _thisTarget, _health)
        {
            damager = _damager;
            currentState = UNITSTATE.ATTACK;

            // Register listeners
            thisTarget.OnStopped += SetIdle;
            health.OnDead += TriggerDeath;

            // 🔹 Register to the current target's death event
            if (thisTarget.Target != null)
            {
                targetHealth = thisTarget.Target.Itself.GetComponent<Health>();
                targetHealth.OnDead += OnTargetDied;
            }
        }

        public override void Enter()
        {
            anim.SetTrigger("isAttack");
            base.Enter();
        }
        public override void Update()
        {
            // If no current target, find a new one
    if (thisTarget.Target == null || !thisTarget.Target.IsAlive)
    {
        // Try to find a new target from ReachDetector
        var newTarget = reachDetector.FindClosestTarget(); // You might need to implement this

        if (newTarget != null)
        {
            thisTarget.Target = newTarget;
            nextState = new Walk(anim, reachDetector, thisTarget, health, damager);
        }
        else
        {
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
        }
        
        eventState = EVENT.EXIT;
        return;
    }
            if (thisTarget.Target == null)
            {
                nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
                eventState = EVENT.EXIT;
                return;
            }
        }

        public override void Exit()
        {
            anim.ResetTrigger("isAttack");

            // 🔹 Clean up
            if (targetHealth != null)
            {
                targetHealth.OnDead -= OnTargetDied;
            }

            health.OnDead -= TriggerDeath;
            thisTarget.OnStopped -= SetIdle;

            base.Exit();
        }

        private void OnTargetDied()
        {
            thisTarget.Target = null; // 🔹 Clear the dead target

            // Transition to Idle so ReachDetector can search again
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }

        private void TriggerDeath()
        {
            nextState = new Die(anim, reachDetector, thisTarget, health);
            eventState = EVENT.EXIT;
        }

        private void SetIdle()
        {
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
    }

    public class Die : UnitState
    {
        public Die(Animator _anim, ReachDetector _reachDetector, Unit _thisTarget, Health _health) : base(_anim, _reachDetector, _thisTarget, _health)
        {
            currentState = UNITSTATE.DIE;
            thisTarget.OnStopped += SetIdle;
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
            thisTarget.OnStopped -= SetIdle;
        }
        private void SetIdle()
        {
            nextState = new Idle(anim, reachDetector, thisTarget, health, damager);
            eventState = EVENT.EXIT;
        }
    }
}
