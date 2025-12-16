using UnityEngine;
using UnityEngine.VFX;

public class LaserTower : BaseTower
{
    [Header("Laser Tower Specific")]
    public FlameDamageArea flameArea;
    
    public VisualEffect laserImpactVFX;
    public VisualEffect[] laserFusionVFX;
    TowerUpgrade towerUpgrade;

    [Header("Laser VFX Binding")]
    [SerializeField] private Transform laserEndPoint; // Empty ou direct calcul
    [SerializeField] private string laserEndPropertyName = "BeamEndPoint";


    private bool isFiring = false;

    void Start()
    {
        towerUpgrade = GetComponent<TowerUpgrade>();
        ApplySpecialTowerStats();
        StopLaser();
    }

    void FixedUpdate()
{
    if (target != null)
    {
            UpdateLaserTransform();
            UpdateLaserEndPoint();
            if (!isFiring) 
        {
            StartLaser();     
            
        }
            if (towerUpgrade != null && towerUpgrade.currentLevel == 2)
            {
                foreach (VisualEffect effect in laserFusionVFX)
                {
                    effect.enabled = true;
                    effect.Play();
                }
            }
        }
    else
    {
        if (isFiring) 
            {
                StopLaser();
                foreach (VisualEffect effect in laserFusionVFX)
                {
                    effect.enabled = false;
                    effect.Stop();
                }
            }
            

    }
}

    void UpdateLaserEndPoint()
    {
        if (laserEndPoint == null || firePoint == null) return;

        laserEndPoint.position =
            firePoint.position + firePoint.forward * currentRange;
        
    }

    // --------------------- LASER TOGGLE ----------------------
    void StartLaser()
    {
        isFiring = true;

        if (flameArea != null)
            flameArea.gameObject.SetActive(true);

       

        if (laserImpactVFX != null)
        {
            laserImpactVFX.enabled = true;
            laserImpactVFX.Play();
        }
           
        //&& !laserImpactVFX.aliveParticleCount.Equals(0)
        //laserImpactVFX.Play();
    }

    void StopLaser()
    {
        isFiring = false;

        if (flameArea != null)
            flameArea.gameObject.SetActive(false);

        

        if (laserImpactVFX != null) 
        {
            laserImpactVFX.enabled = false;
            laserImpactVFX.Stop();
        }
            
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
        flameArea.transform.localScale = new Vector3(1.2f, 1.2f, length);

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
    void UpdateLaserVFX()
    {
        if (laserImpactVFX == null || firePoint == null) return;

        Vector3 endPos = firePoint.position + firePoint.forward * currentRange;

        laserImpactVFX.SetVector3(laserEndPropertyName, endPos);
    }

}
