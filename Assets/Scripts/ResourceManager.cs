using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public ItemSO[] resources;
    
    [SerializeField] private UI_Manager ui_Manager;
    public ItemSlot[] craftingSlots;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ItemSO resource in resources)
        {
            resource.amount = 0;
            ui_Manager.UpdateResourceText(resource.amount,resource.index);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (ItemSO resource in resources)
            {
                AddResource(resource, 1);
                
                Debug.Log($"Added Resource. New Amount: {resource.amount}");
            }
        }

    }
    public void AddResource(ItemSO resource, int amount)
    {
        resource.amount += amount;
        ui_Manager.SpawnResource(resource.index);
        ui_Manager.UpdateResourceText(resource.amount,resource.index);
    }
    public void UseResource(ItemSO resource, int amount)
    {
        resource.amount -= amount;
        if (resource.amount <= 0)
        {
            resource.amount = 0;
            
        }
        ui_Manager.UpdateResourceText(resource.amount, resource.index);
    }

    public void ClearSlots() 
    {
        foreach(ItemSlot itemSlot in craftingSlots) 
        {
            itemSlot.currItem = null;
            itemSlot.UpdateSlotData();
        }
    }
    public void CancelCraft() 
    {
        foreach(ItemSlot itemSlot in craftingSlots) 
        {
            if(itemSlot.currItem != null) 
            {
                AddResource(itemSlot.currItem, 1);
            }
        }
        ClearSlots();
    }
}
