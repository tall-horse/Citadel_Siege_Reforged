using UnityEngine;

public interface IProperty
{
    IProperty Target { get; set; }
    Transform Itself { get; set; }
}
