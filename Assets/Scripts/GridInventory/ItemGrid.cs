using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ItemGrid : MonoBehaviour
{
    public const float TileSizeWidth = 32f;
    public const float TileSizeHeight = 32f;

    InventoryItem[,] inventoryItemSlot;

    [SerializeField] int gridSizeWidth = 10;
    [SerializeField] int gridSizeHeight = 10;

    

    RectTransform rectTransform;
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
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

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY) 
    {
        if (BoundryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height)==false)
        {
            return false;
        }
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        // pour dire que l'item occupe plusieurs slots
        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for(int y = 0; y < inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem;
            }
        }
        inventoryItem.onGridPositionX = posX;
        inventoryItem.OnGridPositionY = posY;
        inventoryItemSlot[posX, posY] = inventoryItem;

        Vector2 position = new Vector2();
        position.x = posX * TileSizeWidth + TileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY *TileSizeHeight + TileSizeHeight * inventoryItem.itemData.height / 2);

        rectTransform.localPosition = position; 
        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        for (int i = 0; i < toReturn.itemData.width; i++)
        {
            for (int j = 0; j < toReturn.itemData.height; j++)
            {
                inventoryItemSlot[toReturn.onGridPositionX + i, toReturn.OnGridPositionY + j] = null;
            }
        }
        
        return toReturn;
    }

    // verifie si la position est dans la grille
    bool PositionCheck(int posX,int posY) 
    {
        if(posX < 0 || posY < 0 )
        {
            return false;
        }
        if(posX >= gridSizeHeight || posY >= gridSizeHeight)
        {
            return false;
        }
        return true;
    }

    bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(PositionCheck(posX, posY) == false)
        {
            return false;
        }
        posX += width-1;
        posY += height-1;
        if (PositionCheck(posX, posY) == false)
        {
            return false;
        }
        return true;
    }
}
