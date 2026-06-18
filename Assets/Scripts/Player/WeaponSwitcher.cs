using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [Header("Arsenal")]
    public Weapon[] weapons; // Asignar los GameObjects de las armas en el Inspector
    private int _currentWeaponIndex = 0;

    private IPlayerInput _input;

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

    // Este método lo llamaría el New Input System (ej. una Action para la rueda del ratón)
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Lógica simplificada: si movemos la rueda hacia arriba, pasamos a la siguiente
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

    // Este método lo llamaría el New Input System al hacer clic izquierdo
    public void OnFire(InputAction.CallbackContext context)
    {
        // Dependiendo del tipo de arma (automática vs semi), podrías usar context.performed o context.ReadValueAsButton()
        if (context.performed && weapons.Length > 0)
        {
            // ¡Polimorfismo en acción! No sabemos qué arma es, pero sabe atacar.
            weapons[_currentWeaponIndex].TryAttack();
        }
    }

    private void EquipWeapon(int index)
    {
        // Apagamos todas las armas y encendemos solo la seleccionada
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
            weapons[i].ResetAnimation();
        }

        Debug.Log($"Arma equipada: {weapons[index].weaponName}");
    }
}