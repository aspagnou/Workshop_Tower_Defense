using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    InventoryControler inventoryControler;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryControler = FindAnyObjectByType(typeof(InventoryControler)) as InventoryControler;
        itemGrid = GetComponent<ItemGrid>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryControler.SelectedItemGrid = itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryControler.SelectedItemGrid = null;
    }

    



}
