using UnityEngine;

public interface IHealth
{
    IHealth Target { get; set; }
    Transform Itself { get; set; }
}
