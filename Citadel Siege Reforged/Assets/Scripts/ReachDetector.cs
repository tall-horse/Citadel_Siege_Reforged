using System;
using UnityEngine;

public class ReachDetector : MonoBehaviour
{
    public event Action<IHealth> OnTargetFound;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Ground")) return;

        var potentialTarget = hit.collider.GetComponent<IHealth>();
        if (potentialTarget == null)
        {
            Debug.Log("No target found");
            return;
        }
        else
        {
            OnTargetFound?.Invoke(potentialTarget);
            Debug.Log("target found");
        }
    }

}
