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
    [SerializeField] protected bool usesAmmo = true;

    protected int currentAmmo;
    protected float nextFireTime = 0f;
    protected bool isReloading = false;
    private Coroutine _reloadCoroutine;

    // Eventos para notificar al HUD
    public event Action<int, int, bool> OnAmmoChanged;

    // ✅ Propiedad pública para acceder al nombre
    public string WeaponName => weaponName;
    public int CurrentAmmo => currentAmmo;
    public int MaxAmmo => maxAmmo;
    public bool UsesAmmo => usesAmmo;
    public bool IsReloading => isReloading;

    protected virtual void Awake()
    {
        currentAmmo = maxAmmo;
    }

    private void OnDisable()
    {
        CancelReload();
    }

    public virtual void TryAttack()
    {
        if (isReloading) return;

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
        _reloadCoroutine = StartCoroutine(ReloadCoroutine());
    }

    public void CancelReload()
    {
        if (_reloadCoroutine != null)
        {
            StopCoroutine(_reloadCoroutine);
            _reloadCoroutine = null;
        }
        isReloading = false;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
    }

    private IEnumerator ReloadCoroutine()
    {
        yield return new WaitForSeconds(3.1f);

        currentAmmo = maxAmmo;
        isReloading = false;
        _reloadCoroutine = null;
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
    }

    public void OnEquip()
    {
        CancelReload();
        OnAmmoChanged?.Invoke(currentAmmo, maxAmmo, isReloading);
    }

    protected abstract void PerformAttack();
    public abstract void ResetAnimation();
}