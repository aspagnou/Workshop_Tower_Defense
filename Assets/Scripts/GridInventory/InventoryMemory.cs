using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class InventoryMemory : MonoBehaviour
{
    
    public List<GearSO> storedGears = new List<GearSO>();
    public BaseTower linkedTower; // sérialisable pour pouvoir assigner manuellement

    void Awake()
    {
        if (linkedTower == null)
            linkedTower = GetComponentInParent<BaseTower>();
    }

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void AddGear(GearSO gear)
    {
        if (gear != null)
        {
            storedGears.Add(gear);
            linkedTower.EquipGear(gear);
            Debug.Log("Gear ajouté : " + gear.name);
        }
    }


    public void RemoveGear(GearSO gear)
    {
        if (gear != null)
        {
            storedGears.Remove(gear);
            linkedTower.UnequipGear(gear);
            Debug.Log("Gear retiré : " + gear.name);
        }
    }
    void Update()
    {
     
    }

    public void DisplayInventory()
    {
        Debug.Log("Mémoire actuelle : " + storedGears.Count);
        foreach (var g in storedGears)
        {
            Debug.Log(" - " + g.name);
        }
    }
}
