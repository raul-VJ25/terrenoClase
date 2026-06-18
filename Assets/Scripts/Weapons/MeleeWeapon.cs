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
    }

    // ✅ Se llama automáticamente cada vez que el arma se activa (SetActive(true))
    private void OnEnable()
    {
        if (anim != null)
        {
            // Reinicia el Animator a su estado base por defecto
            anim.Rebind();
            anim.Update(0f); // Fuerza la actualización inmediata
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
            // ✅ Reinicio total del Animator
            anim.Rebind();
            anim.Update(0f);
            anim.ResetTrigger("accion");
        }
    }
}