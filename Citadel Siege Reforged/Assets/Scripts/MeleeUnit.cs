using System;
using UnityEngine;

public class MeleeUnit : Unit
{
    private Animator animator;
    private ReachDetector reachDetector;
    private Health health;
    private Damager damager;
    [field: SerializeField] private UnitState.UNITSTATE currentState;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        reachDetector = GetComponent<ReachDetector>();
        damager = GetComponent<Damager>();
        health = GetComponent<Health>();
        UnitState = new Idle.Walk(animator, reachDetector, this, health, damager);
        Itself = transform;
    }
    private void Update()
    {
        UnitState = UnitState.Process();
        currentState = UnitState.currentState;
    }
    public void Stop()
    {
        RaiseStopped();
    }
}
