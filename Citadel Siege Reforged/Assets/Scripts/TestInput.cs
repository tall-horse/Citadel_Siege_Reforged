using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestInput : MonoBehaviour
{
    public static event Action OnWarrior1Spawned;
    public static event Action OnWarrior2Spawned;
    [SerializeField] private InputActionReference spawnUnitPlayer1Action;
    [SerializeField] private InputActionReference spawnUnitPlayer2Action;
    void Awake()
    {
        SubscribeControls();
        InputSystem.onDeviceChange += OnDeviceChange;
    }
    public void SubscribeControls()
    {
        spawnUnitPlayer1Action.action.Enable();
        spawnUnitPlayer2Action.action.Enable();
        spawnUnitPlayer1Action.action.performed += TriggerPress1;
        spawnUnitPlayer2Action.action.performed += TriggerPress2;
    }
    public void UnsubscribeCrouch()
    {
        spawnUnitPlayer1Action.action.Disable();
        spawnUnitPlayer2Action.action.Disable();
        spawnUnitPlayer1Action.action.performed -= TriggerPress1;
        spawnUnitPlayer2Action.action.performed -= TriggerPress2;
    }
    private void TriggerPress1(InputAction.CallbackContext context)
    {
        OnWarrior1Spawned?.Invoke();
    }
    private void TriggerPress2(InputAction.CallbackContext context)
    {
        OnWarrior2Spawned?.Invoke();
    }
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                UnsubscribeCrouch();
                break;
            case InputDeviceChange.Reconnected:
                SubscribeControls();
                break;
        }
    }
}
