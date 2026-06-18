using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Arsenal")]
    [SerializeField] private Weapon[] weapons;

    private int _currentWeaponIndex = 0;
    private IPlayerInput _input;

    // ✅ Nuevo evento para el HUD
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
        if (weapons.Length == 0) return;

        if (scrollValue > 0)
        {
            _currentWeaponIndex = (_currentWeaponIndex + 1) % weapons.Length;
        }
        else if (scrollValue < 0)
        {
            _currentWeaponIndex--;
            if (_currentWeaponIndex < 0) _currentWeaponIndex = weapons.Length - 1;
        }

        EquipWeapon(_currentWeaponIndex);
    }

    private void HandleShoot()
    {
        if (weapons.Length > 0)
        {
            weapons[_currentWeaponIndex].TryAttack();
        }
    }

    private void EquipWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }

        weapons[index].ResetAnimation();

        // ✅ Avisamos al arma que ha sido equipada (para que actualice el HUD)
        weapons[index].OnEquip();

        // ✅ Avisamos al HUD que ha cambiado el arma
        OnWeaponChanged?.Invoke(weapons[index]);

        Debug.Log($"Arma equipada: {weapons[index].WeaponName}");
    }
}