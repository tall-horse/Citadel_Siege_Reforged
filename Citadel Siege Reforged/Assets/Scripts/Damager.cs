using UnityEngine;
using UnityEngine.Events;

public class Damager : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    public UnityEvent OnDamageDealt;
    // private Animator animator;
    private IProperty property;
    void Awake()
    {
        // animator = GetComponent<Animator>();
        property = GetComponent<IProperty>();
    }
    public void Damage()
    {
        if (property.Target == null)
        {
            Debug.Log("No target - no damage");
            return;
        }
        //OnDamageDealt?.Invoke();
        property.Target.Itself.GetComponent<Health>().ModifyHealth(-damageAmount);
    }
}
