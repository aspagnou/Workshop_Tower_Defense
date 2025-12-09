using UnityEngine;

public class LaserTower : BaseTower
{
    [Header("Laser Tower Specific")]
    public FlameDamageArea flameArea;
    public ParticleSystem flameParticles;

    void Start()
    {
        ApplySpecialTowerStats();
    }
    
    void FixedUpdate()
    {
        if (target != null) {flameArea.gameObject.SetActive(true);}
        else {flameArea.gameObject.SetActive(false);}
    }

    protected override void Shoot()
    {
        // Un laser tire en continu → pas de tir projectile
        AimAtTarget();
        
    }

    protected override void ApplySpecialTowerStats()
    {
        if (flameArea == null) return;

        float DPS = currentAttackDamage * currentAttackSpeed;
        flameArea.damagePerSecond = DPS;

        flameArea.tickRate = 0.05f; // 20 ticks/sec

        // NOUVEAU → transfert de crit
        flameArea.critChance = currentCriticalChance;
        flameArea.critMultiplier = critMultiplier;

        float length = currentRange;
        float diameter = 1f;

        flameArea.transform.localScale = new Vector3(diameter, diameter, length);

        flameArea.transform.position =
            firePoint.position + firePoint.forward * (length / 2f);

        flameArea.transform.rotation = firePoint.rotation;
    }

}
