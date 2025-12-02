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
            Destroy(inventoryControler.selectedItem.gameObject);
            foreach (ItemSO item in inventoryControler.selectedItem.itemData.relatedGear.recycleRessources)
            {
                if(item != null)
                    resourceManager.AddResource(item, 1);
            }
            Debug.Log("J'ajoute une ressource");
        }
    }
}
