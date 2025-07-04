using UnityEngine;

public class ReachDetector : MonoBehaviour
{
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Ground")) return;

        Debug.Log("Start Attacking with " + hit.collider.name);
    }

}
