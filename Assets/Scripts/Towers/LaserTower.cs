using UnityEngine;
using UnityEngine.VFX;

public class LaserTower : BaseTower
{
    [Header("Laser Tower Specific")]
    public FlameDamageArea flameArea;
    public ParticleSystem flameParticles;
    public VisualEffect laserImpactVFX;

    private bool isFiring = false;

    void Start()
    {
        ApplySpecialTowerStats();
        StopLaser();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            if (!isFiring)
                StartLaser();

            UpdateLaserTransform();
        }
        else
        {
            if (isFiring)
                StopLaser();
        }
    }

    // --------------------- LASER TOGGLE ----------------------
    void StartLaser()
    {
        isFiring = true;

        if (flameArea != null)
            flameArea.gameObject.SetActive(true);

        if (flameParticles != null && !flameParticles.isPlaying)
            flameParticles.Play();

        if (laserImpactVFX != null && !laserImpactVFX.aliveParticleCount.Equals(0))
            laserImpactVFX.Play();
    }

    void StopLaser()
    {
        isFiring = false;

        if (flameArea != null)
            flameArea.gameObject.SetActive(false);

        if (flameParticles != null && flameParticles.isPlaying)
            flameParticles.Stop();

        if (laserImpactVFX != null)
            laserImpactVFX.Stop();
    }

    // ----------------------- UPDATE -------------------------
    protected override void Shoot()
    {
        AimAtTarget();
        // Pas de projectile → rien d'autre
    }

    void UpdateLaserTransform()
    {
        if (flameArea == null) return;

        float length = currentRange;
        flameArea.transform.localScale = new Vector3(1f, 1f, length);

        flameArea.transform.position =
            firePoint.position + firePoint.forward * (length / 2f);

        flameArea.transform.rotation = firePoint.rotation;
    }

    protected override void ApplySpecialTowerStats()
    {
        if (flameArea == null) return;

        float DPS = currentAttackDamage * currentAttackSpeed;
        flameArea.damagePerSecond = DPS;
        flameArea.tickRate = 0.05f;

        flameArea.critChance = currentCriticalChance;
        flameArea.critMultiplier = critMultiplier;
    }
}
