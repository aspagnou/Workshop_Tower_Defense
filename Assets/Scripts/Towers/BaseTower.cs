using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BaseTower : MonoBehaviour
{
    private GameObject mainCanvas;
    public GameObject gridUI;
    public ItemGrid itemGrid;
    public InventoryMemory inventoryMemory;
    private RectTransform fixRectTransform;

    [Header("Tower Price")]
    public int spawnCost = 100;

    [Header("Range Circle")]
    public GameObject rangeCirclePrefab;

    [Header("Base Tower Stats")]
    public float baseAttackDamage;
    public float baseRange;
    public float baseAttackSpeed;
    public float baseCriticalChance;
    public float critMultiplier = 2f;

    [Header("Current Tower Stats")]
    public float currentAttackDamage;
    public float currentRange;
    public float currentAttackSpeed;
    public float currentCriticalChance;

    [Header("Aerial ?")]
    public bool canAttackAerial = false;

    public List<GearSO> equippedGears = new List<GearSO>();
    public TowerUpgrade towerUpgradeManager;

    // Targeting
    public Transform target;

    // Attack cooldown
    private float attackCooldown = 0f;

    [SerializeField] private Transform pivot;
    [SerializeField] private GameObject projectilePrefab;
    public Transform firePoint;

    [SerializeField] private float rotationSpeed=5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCanvas = GameObject.FindWithTag("MainCanvas");
        fixRectTransform = GameObject.FindWithTag("FixGridSpawn").GetComponent<RectTransform>();
        towerUpgradeManager =GetComponent<TowerUpgrade>();
        gridUI.SetActive(false);
        
        RecalculateStats();
        UpdateRangeCircle();
        ApplySpecialTowerStats();
        HideRange();
        

    }

    protected virtual void ApplySpecialTowerStats()
    {
        // Par défaut, ne fait rien.
    }
    // Update is called once per frame


    //----------------------------- GEAR EQUIP/UNEQUIP ------
    public void EquipGear(GearSO gear)
    {
        if (gear == null) return;

        equippedGears.Add(gear);
        RecalculateStats();
        Debug.Log("Gear équipé : " + gear.gearName);
    }

    public void UnequipGear(GearSO gear)
    {
        if (gear == null) return;

        equippedGears.Remove(gear);
        RecalculateStats();
        Debug.Log("Gear retiré : " + gear.gearName);
    }

    public void RecalculateStats()
    {
        // reset aux valeurs de base
        currentAttackDamage = baseAttackDamage;
        currentRange = baseRange;
        currentAttackSpeed = baseAttackSpeed;
        currentCriticalChance = baseCriticalChance;

        // application des bonus
        foreach (var gear in equippedGears)
        {
            //flat bonuses
            currentAttackDamage += gear.flatAttackDamage;
            currentRange += gear.flatRange;
            currentAttackSpeed += gear.flatAttackSpeed;
            currentCriticalChance += gear.flatCriticalChance;
            if (currentCriticalChance > 100f) currentCriticalChance = 100f;

            //percentage bonuses
            currentAttackDamage *= (1 + gear.percentAttackDamage/100);
            currentRange *= (1 + gear.percentRange/100);
            currentAttackSpeed *= (1 + gear.percentAttackSpeed / 100);
        }

        //Debug.Log($"Stats recalculées : dmg={currentAttackDamage}, range={currentRange}, aspd={currentAttackSpeed}");
        UpdateStats();
        UpdateRangeCircle();
        ApplySpecialTowerStats();
    }


    public void ShowGrid()
    {
        if (mainCanvas == null) Debug.LogError("mainCanvas is NULL !");
        if (gridUI == null) Debug.LogError("gridUI is NULL !");
        if (fixRectTransform == null) Debug.LogError("fixRectTransform is NULL !");
        gridUI.SetActive(true);
        // 1. Changer le parent de la grille vers le canvas
        gridUI.transform.SetParent(mainCanvas.transform);

        // 2. Récupérer le RectTransform de la grille et de l'objet de référence
        RectTransform gridRectTransform = gridUI.GetComponent<RectTransform>();
        RectTransform referenceRectTransform = fixRectTransform;

        // 3. Copier les propriétés du RectTransform de référence vers la grille
        gridRectTransform.anchoredPosition = referenceRectTransform.anchoredPosition;
        gridRectTransform.sizeDelta = referenceRectTransform.sizeDelta;
        gridRectTransform.localRotation = referenceRectTransform.localRotation;
        gridRectTransform.localScale = referenceRectTransform.localScale;
        UpdateStats();

    }

    public void HideGrid()
    {
        if (gridUI != null)
        {
            gridUI.transform.SetParent(this.transform);
            gridUI.SetActive(false);
        }
    }
    public void ShowRange() 
    { 
        rangeCirclePrefab.SetActive(true);
        //Debug.Log("Showing Range Circle");
        UpdateRangeCircle();
    }
    public void HideRange() 
    { 
        rangeCirclePrefab.SetActive(false);
    }
    // ----------------------------------------------------

    public void OnTowerSelected()
    {
        TowerSelectMenuManager.Instance.ShowTowerSelectMenu(this);
        ShowRange();
    }
    public void OnTowerDeselected()
    {
        TowerSelectMenuManager.Instance.HideTowerSelectMenu();
        
        HideGrid();
        HideRange();

    }
    public void UpdateStats()
    {
        TMP_Text[] statLines = UI_Manager.Instance.statLines;
        var feedback = UI_Manager.Instance.statsUpFeedback;

        // Calcul des deltas
        float deltaDamage = currentAttackDamage - float.Parse(statLines[0].text);
        float deltaSpeed = currentAttackSpeed - float.Parse(statLines[1].text);
        float deltaCrit = currentCriticalChance - float.Parse(statLines[2].text);/*.Replace("%", ""));*/
        float deltaRange = currentRange - float.Parse(statLines[3].text);

        // Met à jour les valeurs affichées
        statLines[0].text = $"{currentAttackDamage:F0}";
        statLines[1].text = $"{currentAttackSpeed:F1}";
        statLines[2].text = $"{currentCriticalChance:F0}";
        statLines[3].text = $"{currentRange:F0}";

        // Si un feedback existe, on l'affiche
        feedback[0]?.ShowChange(deltaDamage);
        feedback[1]?.ShowChange(deltaSpeed);
        feedback[2]?.ShowChange(deltaCrit);
        feedback[3]?.ShowChange(deltaRange);
    }

    public void UpdateRangeCircle()
    {
        RectTransform rt = rangeCirclePrefab.GetComponent<RectTransform>();

        float baseRadius = 500f;    // rayon de ton image (1000 px / 2)
        float scale = currentRange / baseRadius;

        rt.sizeDelta = new Vector2(1000f * scale, 1000f * scale);
    }

    // ----------------------------------------------------
    void Update()
    {
        FindTarget();
        
        if (target != null)
        {
            AimAtTarget();
            Shoot();
        }
    }

    void FindTarget()
    {
       
        if (canAttackAerial == false) 
        {
            Enemy[] allEnemies = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            // Trouver uniquement les ennemis terrestres

            List<Enemy> groundEnemies = new List<Enemy>();
            foreach (Enemy enemy in allEnemies)
            {
                if (enemy != null && enemy.enemyType != EnemyType.Flying)
                {
                    groundEnemies.Add(enemy);
                }
            }
            float shortestDistance = Mathf.Infinity;
            Enemy nearestEnemy = null;
            foreach (Enemy enemy in groundEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance && distance <= currentRange)
                {
                    Debug.Log("Ground Enemy found within range");
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null)
                target = nearestEnemy.transform;
            else
                target = null;
            Debug.Log("No ground enemy in range");
            return;

        }
        else 
        {
            Enemy[] allEnemies = UnityEngine.Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            // Trouver tous les ennemis aériens
            List<Enemy> aerialEnemies = new List<Enemy>();
            foreach (Enemy enemy in allEnemies)
            {
                if (enemy != null && enemy.enemyType == EnemyType.Flying)
                {
                    aerialEnemies.Add(enemy);
                }
                float shortestDistance = Mathf.Infinity;
                Enemy nearestEnemy = null;
                foreach (Enemy airEnemy in aerialEnemies)
                {
                    float distance = Vector3.Distance(transform.position, airEnemy.transform.position);
                    if (distance < shortestDistance && distance <= currentRange)
                    {
                        Debug.Log("Aerial Enemy found within range");
                        shortestDistance = distance;
                        nearestEnemy = enemy;
                    }
                }
                if (nearestEnemy != null)
                    target = nearestEnemy.transform;
                else
                    target = null;
                Debug.Log("No Air enemy in range");
                return;

            }

        }
        
    }


    protected virtual void AimAtTarget()
    {
        if (target == null) return;

        Vector3 direction = target.position - pivot.position;
        direction.y = 0f; // Optionnel : verrouille la rotation en 2D sur l'axe Y

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        pivot.rotation = Quaternion.Lerp(
            pivot.rotation,
            lookRotation,
            Time.deltaTime * rotationSpeed
        );
    }



    protected virtual void Shoot()
    {
        // Si pas de cible → ne pas tirer
        if (target == null) return;

        // Gestion cooldown basé sur attack speed
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
            return;
        }

        // Calcul critique
        bool isCrit = Random.value <= (currentCriticalChance / 100f);
        float finalDamage = isCrit ? currentAttackDamage * critMultiplier : currentAttackDamage;

        // Instancier projectile
        Quaternion rot = Quaternion.LookRotation(target.position - firePoint.position);
        SpawnProjectile(rot);

        // Reset cooldown en fonction de la vitesse d’attaque
        attackCooldown = 1f / currentAttackSpeed;
    }


    void SpawnProjectile(Quaternion rotation)
    {
        //calcul critique
        bool isCrit = Random.value <= (currentCriticalChance / 100f);
        float finalDamage = isCrit ? currentAttackDamage * critMultiplier : currentAttackDamage;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, rotation);

        //For classical projectile
        Projectile_classic pC = proj.GetComponent<Projectile_classic>();
        if (pC != null)
        {
            pC.damage = finalDamage;
            pC.isCritical = isCrit;
        }
        //For canon projectile
        Projectile_Canon pCa = proj.GetComponent<Projectile_Canon>();
        if (pCa != null)
        {
            pCa.damage = finalDamage;
            pCa.isCritical = isCrit;
        }

        if (isCrit) Debug.Log("Crit" + finalDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, currentRange);
    }
}
