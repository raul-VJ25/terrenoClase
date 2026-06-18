using System.Collections;
using UnityEngine;

public class GrenadeExplosionForce : MonoBehaviour
{
    [SerializeField] private float explosionTime = 3f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float upwardForce = 2000f;
    [SerializeField] private GameObject explosionPrefab;

    void Start()
    {
        StartCoroutine(ExplosionTimer());
    }

    IEnumerator ExplosionTimer()
    {
        yield return new WaitForSeconds(explosionTime);
        Explode();
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null && !rb.isKinematic)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardForce, ForceMode.Impulse);
            }
        }

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}