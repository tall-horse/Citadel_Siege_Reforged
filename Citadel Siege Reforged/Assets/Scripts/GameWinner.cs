using System.Linq;
using UnityEngine;

public class GameWinner : MonoBehaviour
{
    private Health health;
    void Awake()
    {
        health = GetComponent<Health>();
    }

    void Start()
    {
        health.OnDead += EndGame;
    }

    private void EndGame()
    {
        Debug.Log("time to end");

        var units = FindObjectsByType<Unit>(FindObjectsSortMode.None);
        foreach (var u in units)
        {
            u.Target = null; // clear target
            u.GetComponent<ReachDetector>().lookingForTargets = false; // stop detection
            u.RaiseStopped(); // transition to Idle
            Destroy(gameObject);
        }
    }
}
