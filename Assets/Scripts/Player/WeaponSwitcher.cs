using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Arsenal")]
    public Weapon[] weapons;

    private int _currentWeaponIndex = 0;
    private IPlayerInput _input;

    // ✅ NUEVO: Evento para notificar al HUD
    public event Action<Weapon> OnWeaponChanged;

    private void Awake()
    {
        _input = GetComponent<IPlayerInput>();
    }

    void Start()
    {
        EquipWeapon(_currentWeaponIndex);
    }

    private void OnEnable()
    {
        if (_input != null)
        {
            _input.OnShootEvent += HandleShoot;
            _input.OnSwitchWeaponEvent += HandleSwitchWeapon;
        }
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            _input.OnShootEvent -= HandleShoot;
            _input.OnSwitchWeaponEvent -= HandleSwitchWeapon;
        }
    }

    private void HandleSwitchWeapon(float scrollValue)
    {
        if (scrollValue > 0)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
            EquipWeapon(_currentWeaponIndex);
        }
        else if (scrollValue < 0)
        {
            _currentWeaponIndex--;
            if (_currentWeaponIndex < 0)
                _currentWeaponIndex = weapons.Length - 1;
            EquipWeapon(_currentWeaponIndex);
        }
    }

    private void HandleShoot()
    {
        if (weapons.Length > 0)
        {
            weapons[_currentWeaponIndex].TryAttack();
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float scrollValue = context.ReadValue<float>();
            if (scrollValue > 0)
            {
                _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
                EquipWeapon(_currentWeaponIndex);
            }
            else if (scrollValue < 0)
            {
                _currentWeaponIndex--;
                if (_currentWeaponIndex < 0) _currentWeaponIndex = weapons.Length - 1;
                EquipWeapon(_currentWeaponIndex);
            }
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && weapons.Length > 0)
        {
            weapons[_currentWeaponIndex].TryAttack();
        }
    }

    private void EquipWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        weapons[index].gameObject.SetActive(true);
        weapons[index].ResetAnimation();
        weapons[index].OnEquip();

        // ✅ Notificar al HUD del cambio de arma
        OnWeaponChanged?.Invoke(weapons[index]);

        Debug.Log($"Arma equipada: {weapons[index].WeaponName}");
    }
}