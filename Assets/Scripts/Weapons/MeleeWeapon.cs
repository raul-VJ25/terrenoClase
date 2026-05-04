using UnityEngine;

[RequireComponent(typeof(Animation))]

public class MeleeWeapon : Weapon
{
    public float damage = 50f;
    public float attackRange = 2f;
    public float attackRadius = 1f; // Lo "gordo" que es el golpe
    public Transform cameraTransform;

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Slash! (Cuchillazo)");
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward, Color.indigo, 1.5f);

        if (anim != null)
        {
            anim.SetTrigger("accion");
        }

        // Calculamos el punto frente a la cámara
        Vector3 attackPoint = cameraTransform.position + cameraTransform.forward * attackRange;

        // Detectamos todo lo que esté en esa pequeña esfera
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, attackRadius);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log($"Acuchillado: {hit.name}");
                // target?.TakeDamage(damage);
            }
        }
    }

    // Útil para que los alumnos vean el área de ataque en la vista de escena
    private void OnDrawGizmosSelected()
    {
        if (cameraTransform != null)
        {
            Gizmos.color = Color.red;
            Vector3 attackPoint = cameraTransform.position + cameraTransform.forward * attackRange;
            Gizmos.DrawWireSphere(attackPoint, attackRadius);
        }
    }

    public override void ResetAnimation()
    {
        anim.Play("idle", 0, 0f);
    }
}