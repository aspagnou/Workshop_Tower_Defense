using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform highlighter;


    public void Show(bool b) 
    {
        highlighter.gameObject.SetActive(b);
    }
    public void SetSize(InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * ItemGrid.TileSizeWidth;
        size.y = targetItem.itemData.height * ItemGrid.TileSizeHeight;  
        highlighter.sizeDelta = size;
    }
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        

        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.OnGridPositionY
            );
        highlighter.localPosition = pos;

    }

    // set the parent of the highlighter to the target grid
    public void SetParent(ItemGrid targetGrid)
    {
        if (targetGrid == null) { return; }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    // set the position of the highlighter to the target grid and position
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int poxX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            poxX,
            posY
            );
        highlighter.localPosition = pos;
    }
}
