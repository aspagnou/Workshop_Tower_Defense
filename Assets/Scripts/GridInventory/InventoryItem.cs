using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int onGridPositionX;
    public int OnGridPositionY;

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponent<Image>().sprite = itemData.itemIcon;
        Vector2 size = new Vector2();
        size.x = itemData.width * ItemGrid.TileSizeWidth;
        size.y = itemData.height * ItemGrid.TileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;
    }
}
