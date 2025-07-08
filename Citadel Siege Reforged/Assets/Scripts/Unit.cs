using UnityEngine;

public abstract class Unit : MonoBehaviour, IProperty
{
    [field: SerializeField] public IProperty Target { get; set; }
    public Transform Itself { get; set; }
    public UnitState UnitState { get; protected set; }
    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

}
