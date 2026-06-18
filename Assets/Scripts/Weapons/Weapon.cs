using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] protected string weaponName;
    [SerializeField] private float fireRate = 0.5f;

    protected float nextFireTime = 0f;
    public string WeaponName => weaponName;

    public virtual void TryAttack()
    {
        if (Time.time >= nextFireTime)
        {
            PerformAttack();
            nextFireTime = Time.time + fireRate;
        }
    }

    protected abstract void PerformAttack();
    public abstract void ResetAnimation();
}