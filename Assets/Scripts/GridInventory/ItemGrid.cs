using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemGrid : MonoBehaviour
{
    const float TileSizeWidth = 32f;
    const float TileSizeHeight = 32f;

    InventoryItem[,] inventoryItemSlot;

    [SerializeField] int gridSizeWidth = 10;
    [SerializeField] int gridSizeHeight = 10;

    [SerializeField] GameObject inventoryItemPrefab;

    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        PlaceItem(inventoryItem, 1,2);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    Vector2 positionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnTheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnTheGrid.x / TileSizeWidth);
        tileGridPosition.y = (int)(positionOnTheGrid.y / TileSizeHeight);

        return tileGridPosition;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY) 
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        inventoryItemSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * TileSizeWidth + TileSizeWidth / 2;
        position.y = -(posY *TileSizeHeight + TileSizeHeight / 2);

        rectTransform.localPosition = position; 
    }

    
}
