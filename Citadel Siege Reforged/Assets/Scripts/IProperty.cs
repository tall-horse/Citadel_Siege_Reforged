using UnityEngine;

public interface IProperty
{
    Owner Owner { get; set; }
    IProperty Target { get; set; }
    Transform Itself { get; set; }
}

public enum Owner
{
    LEFT,
    RIGHT
}
