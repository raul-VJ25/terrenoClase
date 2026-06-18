using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MeleeWeapon : Weapon
{
    [SerializeField] private float damage = 50f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private Transform cameraTransform;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        // ✅ Forzamos idle al inicio
        if (anim != null)
        {
            anim.Play("idle", 0, 0f);
        }
    }

    // ✅ NUEVO: Se llama cuando el arma se activa
    private void OnEnable()
    {
        if (anim != null)
        {
            // Forzamos el estado idle inmediatamente
            anim.Play("idle", 0, 0f);
            anim.Update(0f);
        }
    }

    protected override void PerformAttack()
    {
        Debug.Log($"{weaponName}: ¡Slash! (Cuchillazo)");

        if (anim != null)
        {
            anim.SetTrigger("accion");
        }

        Vector3 attackPoint = cameraTransform.position + cameraTransform.forward * attackRange;
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, attackRadius);

        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                Debug.Log($"Acuchillado: {hit.name}");
            }
        }
    }

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
        if (anim != null)
        {
            // ✅ Reset completo de la animación
            anim.Play("idle", 0, 0f);
            anim.Update(0f);
            anim.ResetTrigger("accion");
        }
    }
}