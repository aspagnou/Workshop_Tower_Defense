using UnityEngine;

public class Projectile_Canon : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;

    public float explosionRadius = 3f;
    public float damage = 10f;

    public bool isCritical = false;
    public LayerMask contactLayers;

    [Header("Explosion Indicator")]
    public GameObject explosionIndicatorPrefab;
    public float indicatorDuration = 0.5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 🔥 Vérifie si le layer de l'objet est dans le LayerMask
        if (IsInLayerMask(other.gameObject.layer, contactLayers))
        {
            Explode();
        }
    }

    bool IsInLayerMask(int layer, LayerMask mask)
    {
        return (mask.value & (1 << layer)) != 0;
    }

    void Explode()
    {
        ShowExplosionIndicator();

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null && enemy.enemyType != EnemyType.Flying)
            {
                enemy.TakeDamage(Mathf.RoundToInt(damage));
            }
        }

        Destroy(gameObject);
    }

    void ShowExplosionIndicator()
    {
        if (explosionIndicatorPrefab == null)
            return;

        GameObject indicator = Instantiate(explosionIndicatorPrefab, transform.position, Quaternion.identity);

        // Scale la sphère selon le radius
        indicator.transform.localScale = Vector3.one * (explosionRadius * 2f);

        Destroy(indicator, indicatorDuration);
    }
}
