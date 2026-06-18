using UnityEngine;
using TMPro; // Si usas el Text normal de Unity, cambia esto por: using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private TextMeshProUGUI weaponNameText; // Cambia a Text si no usas TMP
    [SerializeField] private TextMeshProUGUI ammoText;       // Cambia a Text si no usas TMP

    [Header("Referencias del Juego")]
    [SerializeField] private WeaponSwitcher weaponSwitcher;

    private Weapon currentWeapon;

    void Start()
    {
        if (weaponSwitcher != null)
        {
            // Nos suscribimos al evento de cambio de arma
            weaponSwitcher.OnWeaponChanged += UpdateWeaponUI;
        }
        else
        {
            Debug.LogError("[HUD] Falta asignar el WeaponSwitcher en el Inspector.");
        }
    }

    void OnDestroy()
    {
        // Buena práctica: Desuscribirse al destruir el objeto para evitar memory leaks
        if (weaponSwitcher != null)
        {
            weaponSwitcher.OnWeaponChanged -= UpdateWeaponUI;
        }

        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }
    }

    private void UpdateWeaponUI(Weapon weapon)
    {
        // Nos desuscribimos del arma anterior para no recibir sus eventos
        if (currentWeapon != null)
        {
            currentWeapon.OnAmmoChanged -= UpdateAmmoUI;
        }

        currentWeapon = weapon;

        if (currentWeapon != null)
        {
            // Actualizamos el nombre
            weaponNameText.text = currentWeapon.WeaponName;

            // Nos suscribimos a los cambios de munición del NUEVO arma
            currentWeapon.OnAmmoChanged += UpdateAmmoUI;

            // Forzamos una actualización inicial de la munición
            UpdateAmmoUI(currentWeapon.CurrentAmmo, currentWeapon.MaxAmmo, currentWeapon.IsReloading);
        }
    }

    private void UpdateAmmoUI(int current, int max, bool isReloading)
    {
        if (isReloading)
        {
            ammoText.text = "Recargando...";
            ammoText.color = Color.yellow; // Opcional: cambiar color mientras recarga
        }
        else if (!currentWeapon.UsesAmmo)
        {
            ammoText.text = "∞"; // Símbolo de infinito para el cuchillo
            ammoText.color = Color.white;
        }
        else
        {
            ammoText.text = $"{current} / {max}";
            ammoText.color = current <= (max * 0.2f) ? Color.red : Color.white; // Opcional: rojo si queda poca munición
        }
    }
}