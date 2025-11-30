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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowGrid();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            HideGrid();
        }
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
    public void HideGrid()
    {
        gridUI.transform.SetParent(this.transform);
    }

    public void ShowGrid()
    {
        gridUI.transform.SetParent(mainCanvas.transform);
    }
    // ----------------------------------------------------

}
