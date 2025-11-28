using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControler : MonoBehaviour
{
    [HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryItem selectedItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;
    private void Update()
    {
        ItemIconDrag();
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            CreateRandomItem();
        }

        if (selectedItemGrid == null) { return; }


        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();

        selectedItem = inventoryItem;
        rectTransform = inventoryItem.GetComponent<RectTransform>();

        rectTransform.SetParent(canvasTransform);

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);

    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tiledGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

        if (selectedItem == null)
        {
            PickUpItem(tiledGridPosition);

        }
        else
        {
            PlaceItem(tiledGridPosition);
        }
    }

    private void PlaceItem(Vector2Int tiledGridPosition)
    {
        bool complete =selectedItemGrid.PlaceItem(selectedItem, tiledGridPosition.x, tiledGridPosition.y);
        if (complete)
        {
            selectedItem = null;
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

