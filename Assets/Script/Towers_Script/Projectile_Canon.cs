using UnityEngine;

public class Projectile_Canon : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;

    public float explosionRadius = 3f;
    public float damage = 10f;

    public bool isCritical = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(Mathf.RoundToInt(damage));
            }
        }
        
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
