using UnityEngine;

public abstract class Unit : MonoBehaviour, IHealth
{
    [field: SerializeField] public IHealth Target { get; set; }
    public Transform Itself { get; set; }
    public UnitState UnitState { get; protected set; }
}
