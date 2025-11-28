using UnityEngine;

public class GearManager : MonoBehaviour
{
    public GearSlot[] gearSlots;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
