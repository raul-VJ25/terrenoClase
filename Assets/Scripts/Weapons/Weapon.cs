using System;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected string weaponName;
    [SerializeField] private float fireRate = 0.5f;

    [Header("Ammo Settings")]
    [SerializeField] protected int maxAmmo = 30;
    [SerializeField] protected bool usesAmmo = true; // ✅ Ponlo en FALSE para el cuchillo

    protected int currentAmmo;
    protected float nextFireTime = 0f;
    protected bool isReloading = false;

    // Eventos para notificar al HUD (Principio de Inversión de Dependencias)
    public event Action<int, int, bool> OnAmmoChanged; // currentAmmo, maxAmmo, isReloading

    public string WeaponName => weaponName;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool UsesAmmo => usesAmmo;
    public bool IsReloading => isReloading;

    protected virtual void Awake()
    {
        currentAmmo = maxAmmo;
    }

    public virtual void TryAttack()
    {
        if (isReloading) return; // No puede disparar mientras recarga

        if (usesAmmo && currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        if (Time.time >= nextFireTime)
        {
            PerformAttack();
            nextFireTime = Time.time + fireRate;

            if (usesAmmo)
            {
                currentAmmo--;
                OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);

                if (currentAmmo <= 0)
                {
                    StartReload();
                }
            }
        }
    }

    protected void StartReload()
    {
        if (isReloading || !usesAmmo) return;

        isReloading = true;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(5f); // 5 segundos de recarga

        currentAmmo = maxAmmo;
        isReloading = false;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
    }

    // Llamado por el WeaponSwitcher cuando se equipa el arma
    public void OnEquip()
    {
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
    }

    protected abstract void PerformAttack();
    public abstract void ResetAnimation();
}