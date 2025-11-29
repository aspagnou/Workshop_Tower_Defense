using UnityEngine;
using UnityEngine.EventSystems;

public class GearManager : MonoBehaviour
{
    public GearSlot[] gearSlots;
    private InventoryControler inventoryControler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryControler = FindAnyObjectByType(typeof(InventoryControler)) as InventoryControler;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (inventoryControler != null)
            {
                inventoryControler.TryEquipInFirstFreeGearSlot();
            }
        }
    }
    public void AddGear(GearSO gear) 
    {
        for (int i = 0; i < gearSlots.Length; i++) 
        {
            if(gearSlots[i].currGear == null) 
            {
                gearSlots[i].currGear = gear;
                gearSlots[i].UpdateSlotData();
                break;
            }
        }
    }
    public bool IsFullGear() 
    {
        for (int i = 0; i < gearSlots.Length; i++)
        {
            if (gearSlots[i].currGear == null)
            {
                return false;
            }
        }
        return true;
    }
}
