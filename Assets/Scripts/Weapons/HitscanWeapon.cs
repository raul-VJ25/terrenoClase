using UnityEngine;

public class HitscanWeapon : Weapon
{
    public float damage = 20f;
    public float range = 100f;
    public float impactForce = 270f;

    public Transform cameraTransform; // Referencia a la cámara para apuntar

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Pew pew!!!!! (Raycast)");

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.indigo, 1.5f);

        // Lanzamos el rayo desde la cámara
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, range))
        {
            Debug.Log($"Impacto en: {hit.collider.name}");

            if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
            {
                Rigidbody rg = hit.rigidbody;
                rg.AddForce(cameraTransform.forward * impactForce, ForceMode.Impulse);

            }

            // Aquí aplicaríamos daño si el objeto golpeado tiene la interfaz IDamageable
            // var target = hit.collider.GetComponent<IDamageable>();
            // target?.TakeDamage(damage);
        }
    }
    public override void ResetAnimation()
    {
        
    }
}