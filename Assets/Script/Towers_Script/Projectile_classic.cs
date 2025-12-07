using UnityEngine;

public class Projectile_classic : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 3f;
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
        Enemy target = other.GetComponent<Enemy>();
        if (target != null)
        {
            Debug.Log("Arrow hit enemy, dealing " + damage + " damage.");
            target.TakeDamage(Mathf.RoundToInt(damage));
            Destroy(gameObject);
        }
    }
}
