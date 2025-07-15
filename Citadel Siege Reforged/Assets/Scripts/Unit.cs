using System;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IProperty
{
    [field: SerializeField] public Owner Owner { get; set; }
    [field: SerializeField] public IProperty Target { get; set; }
    public Transform Itself { get; set; }
    public UnitState UnitState { get; protected set; }
    public event Action OnStopped;
    public void RaiseStopped()
    {
        OnStopped?.Invoke();
    }
}
