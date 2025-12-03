using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseTower : MonoBehaviour
{
    private GameObject mainCanvas;
    public GameObject gridUI;
    public ItemGrid itemGrid;
    public InventoryMemory inventoryMemory;
    private RectTransform fixRectTransform;

    

    [Header("Base Tower Stats")]
    public float baseAttackDamage;
    public float baseRange;
    public float baseAttackSpeed;
    public float baseCriticalChance;

    [Header("Current Tower Stats")]
    public float currentAttackDamage;
    public float currentRange;
    public float currentAttackSpeed;
    public float currentCriticalChance;

    public List<GearSO> equippedGears = new List<GearSO>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCanvas = GameObject.FindWithTag("MainCanvas");
        fixRectTransform = GameObject.FindWithTag("FixGridSpawn").GetComponent<RectTransform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

        // application des bonus
        foreach (var gear in equippedGears)
        {
            //flat bonuses
            currentAttackDamage += gear.flatAttackDamage;
            currentRange += gear.flatRange;
            currentAttackSpeed += gear.flatAttackSpeed;
            currentCriticalChance += gear.flatCriticalChance;

            //percentage bonuses
            currentAttackDamage *= (1 + gear.percentAttackDamage/100);
            currentRange *= (1 + gear.percentRange/100);
            currentAttackSpeed *= (1 + gear.percentAttackSpeed / 100);
        }

        Debug.Log($"Stats recalculées : dmg={currentAttackDamage}, range={currentRange}, aspd={currentAttackSpeed}");
    }


    public void ShowGrid()
    {
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

       
    }

    public void HideGrid()
    {
        gridUI.transform.SetParent(this.transform);
    }
    // ----------------------------------------------------

    public void OnTowerSelected()
    {
        TowerSelectMenuManager.Instance.ShowTowerSelectMenu(this);
    }
    public void OnTowerDeselected()
    {
        TowerSelectMenuManager.Instance.HideTowerSelectMenu();
        HideGrid();
    }

}
