using UnityEngine;

public class ParabolicWeapon : Weapon
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float upwardForce = 5f;

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Granada va!");

        GameObject grenade = Instantiate(grenadePrefab, firePoint.position, firePoint.rotation);

        if (grenade.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Vector3 forceToAdd = (firePoint.forward * throwForce) + (firePoint.up * upwardForce);
            rb.AddForce(forceToAdd, ForceMode.Impulse);
        }
    }

    public override void ResetAnimation()
    {
    }
}