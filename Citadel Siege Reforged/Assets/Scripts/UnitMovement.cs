using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private CharacterController characterController;
    void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = transform.forward * speed * Time.deltaTime;
        characterController.Move(move);
    }
}
