using UnityEngine;

public class Structure : MonoBehaviour, IProperty
{
    [field: SerializeField] public Owner Owner { get; set; }
    [field: SerializeField] public IProperty Target { get; set; }
    public Transform Itself { get; set; }
    void Awake()
    {
        Itself = transform;
    }
}
