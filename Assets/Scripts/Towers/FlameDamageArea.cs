using UnityEngine;

public class FlameDamageArea : MonoBehaviour
{
    public float damagePerSecond = 20f;
    public float tickRate = 0.1f;

    [Header("Critical Strike")]
    public float critChance = 0f;      // en pourcentage
    public float critMultiplier = 1.3f;

    private float tickTimer;

    void Update()
    {
        tickTimer -= Time.deltaTime;
    }

    void OnTriggerStay(Collider other)
    {
        if (tickTimer > 0f)
            return;

        Enemy e = other.GetComponent<Enemy>();
        if (e != null && e.enemyType != EnemyType.Flying)
        {
            tickTimer = tickRate;

            // --- Calcul du dégât normal par tick ---
            float baseDamage = damagePerSecond * tickRate;

            // --- Calcul critique ---
            bool isCrit = Random.value <= (critChance / 100f);
            float finalDamage = isCrit ? baseDamage * critMultiplier : baseDamage;

            if (isCrit)
                Debug.Log($" CRIT TICK ! {finalDamage} damage");
            else
                Debug.Log($"FlameDamageArea deals {finalDamage} dmg");

            e.TakeDamage(finalDamage);
        }
    }
}
