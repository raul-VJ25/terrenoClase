using UnityEngine;

public class HitscanWeapon : Weapon
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float impactForce = 270f;
    [SerializeField] private Transform cameraTransform;

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Pew pew!!!!! (Raycast)");
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.indigo, 1.5f);

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, range))
        {
            Debug.Log($"Impacto en: {hit.collider.name}");

            if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
            {
                hit.rigidbody.AddForce(cameraTransform.forward * impactForce, ForceMode.Impulse);
            }
        }
    }

    public override void ResetAnimation()
    {
    }
}