using UnityEngine;

public class GearRecycler : MonoBehaviour
{
    private InventoryControler inventoryControler;
    private ResourceManager resourceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryControler = FindAnyObjectByType<InventoryControler>();
        resourceManager = FindAnyObjectByType<ResourceManager>();
    }
     
    public void RecycleGear() 
    {
        if (inventoryControler.selectedItem != null) 
        {
            int i = 0;
            
            foreach (ItemSO item in inventoryControler.selectedItem.itemData.relatedGear.recycleRessources)
            {
                if(item != null) 
                {
                    int amount = inventoryControler.selectedItem.itemData.relatedGear.amounts[i];
                    resourceManager.AddResource(item, amount);
                    i++;
                }
            }
            Destroy(inventoryControler.selectedItem.gameObject);
            Debug.Log("J'ajoute une ressource");
        }
    }
}
