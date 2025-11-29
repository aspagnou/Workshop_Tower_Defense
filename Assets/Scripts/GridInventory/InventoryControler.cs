using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControler : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid;
        set {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        } 
    }

    
    public InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight= GetComponent<InventoryHighlight>();
        
    }
    private void Update()
    {
        ItemIconDrag();
        
        Debug.Log("Selected grid: " + (selectedItem != null ? selectedItem.name : "null"));

        if (selectedItemGrid == null) 
        { 
            inventoryHighlight.Show(false);
            return; 
        }
        

        HandleHighLight();

        if (Input.GetMouseButtonDown(0))
        {
            
            LeftMouseButtonPress();
            
        }
    }
    Vector2Int oldPosition;
    InventoryItem itemToHighLight;
    private void HandleHighLight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();

        if(oldPosition == positionOnGrid)
        {
            return;
        }
        oldPosition = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighLight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);
            if (itemToHighLight != null)
            {
                // show highlight
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighLight);
                
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighLight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }

        }
        else {
            // show highlight where the item will be placed
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x, 
                positionOnGrid.y,
                selectedItem.itemData.width,
                selectedItem.itemData.height)
                );
            inventoryHighlight.SetSize(selectedItem);
            
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x,positionOnGrid.y);
        }
    }

    public void CreateItem(ItemData item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();

        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();

        rectTransform.SetParent(canvasTransform);

        //int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(item);

    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tiledGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tiledGridPosition);

        }
        else
        {
            PlaceItem(tiledGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;
        if (selectedItem != null)
        {
            position.x -= (selectedItem.itemData.width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (selectedItem.itemData.height - 1) * ItemGrid.TileSizeHeight / 2;
        }
       
        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tiledGridPosition)
    {
        Debug.Log("Tentative de placement de l'item");
        bool complete =selectedItemGrid.PlaceItem(selectedItem, tiledGridPosition.x, tiledGridPosition.y, ref overlapItem);
        if (complete)
        {
            selectedItem = null;
            if(overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>(); 
            }
        }
        


    }

    public void TryEquipInFirstFreeGearSlot()
    {
        GearSlot[] allGearSlots = FindObjectsByType<GearSlot>(FindObjectsSortMode.InstanceID);
        Debug.Log("Nombre de GearSlots trouvés : " + allGearSlots.Length);
        if (selectedItem == null) return;
        if (selectedItemGrid != null) return;

        GearSO gear = selectedItem.itemData.relatedGear;
        if (gear == null) return;

        // Trouver le premier slot libre    
        foreach (GearSlot slot in allGearSlots)
        {
            if (slot.currGear == null)
            {
                slot.currGear = gear;
                slot.UpdateSlotData();

                Destroy(selectedItem.gameObject);
                selectedItem = null;
                return;
            }
        }
    }

    private void PickUpItem(Vector2Int tiledGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tiledGridPosition.x, tiledGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}

