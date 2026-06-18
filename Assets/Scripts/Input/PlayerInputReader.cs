using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, IPlayerInput
{
    private FPSinput _gameInput;

    public event Action OnShootEvent;
    public event Action<float> OnSwitchWeaponEvent;

    void Awake()
    {
        _gameInput = new FPSinput();
        _gameInput.Player.Shoot.performed += DoShoot;
        _gameInput.Player.SwitchWeapon.performed += DoSwitchWeapon;
    }

    private void DoSwitchWeapon(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        OnSwitchWeaponEvent?.Invoke(scrollValue);
    }

    private void DoShoot(InputAction.CallbackContext context)
    {
        OnShootEvent?.Invoke();
    }

    void OnEnable() => _gameInput.Enable();
    void OnDisable() => _gameInput.Disable();

    public Vector2 MoveInput => _gameInput.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => _gameInput.Player.Look.ReadValue<Vector2>();
    public bool isJumping => _gameInput.Player.Jump.WasPressedThisFrame();
    public bool isRunning => _gameInput.Player.Sprint.IsPressed();
}