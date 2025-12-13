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
    public BaseTower selectedTower;
    private ToolTipManager toolTipManager;


    public InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    [Header("Gear Slots")]
    public GearSlot[] allGearSlots;

    [Header("Tooltip Delay")]
    public float inventoryToolTipDelay = 0.25f;
    private float inventoryTooltipTimer = 0f;
    private bool inventoryHoveringItem = false;
    private InventoryItem lastHoveredItem = null;


    public InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight= GetComponent<InventoryHighlight>();
        toolTipManager = FindAnyObjectByType<ToolTipManager>();
       

    }
    private void Update()
    {
        ItemIconDrag();
        HandleInventoryTooltipDelay();



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

        if (oldPosition == positionOnGrid)
            return;

        oldPosition = positionOnGrid;

        // Vérifier si les coordonnées sont valides
        if (positionOnGrid.x < 0 || positionOnGrid.y < 0 ||
            positionOnGrid.x >= selectedItemGrid.gridSizeWidth ||
            positionOnGrid.y >= selectedItemGrid.gridSizeHeight)
        {
            inventoryHighlight.Show(false);
            toolTipManager.Hide();
            inventoryHoveringItem = false;
            lastHoveredItem = null;
            return;
        }

        // -----------------------------
        //  CAS : AUCUN ITEM SÉLECTIONNÉ (hover dans la grille)
        // -----------------------------
        if (selectedItem == null)
        {
            itemToHighLight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighLight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighLight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighLight);

                // --- Gestion délai tooltip ---
                if (lastHoveredItem != itemToHighLight)
                {
                    lastHoveredItem = itemToHighLight;
                    inventoryTooltipTimer = 0f;
                    inventoryHoveringItem = true;
                    toolTipManager.Hide();
                }
            }
            else
            {
                // Pas d’item sous la souris
                inventoryHighlight.Show(false);
                toolTipManager.Hide();
                inventoryHoveringItem = false;
                lastHoveredItem = null;
            }
        }
        // -----------------------------
        //  CAS : ITEM TENUE PAR LA SOURIS (drag)
        // -----------------------------
        else
        {
            // En mode drag  pas de tooltip
            inventoryHoveringItem = false;
            lastHoveredItem = null;

            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.itemData.width,
                selectedItem.itemData.height));

            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem,
                positionOnGrid.x, positionOnGrid.y);
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
        //Debug.Log("Tentative de placement de l'item");
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

    public void RetrieveGear(GearSO gear) 
    {
        foreach (GearSlot slot in allGearSlots)
        {
            if (slot.currGear == null)
            {
                slot.currGear = gear;
                slot.UpdateSlotData();
                return;
            }
        }
        ResourceManager.Instance.AddMana(100);
    }

    private void PickUpItem(Vector2Int tiledGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tiledGridPosition.x, tiledGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            toolTipManager.Hide();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            toolTipManager.Hide();
            rectTransform.position = Input.mousePosition;
        }
    }
    private void HandleInventoryTooltipDelay()
    {
        if (!inventoryHoveringItem || lastHoveredItem == null)
            return;

        inventoryTooltipTimer += Time.deltaTime;

        if (inventoryTooltipTimer >= inventoryToolTipDelay)
        {
            toolTipManager.Show(lastHoveredItem.itemData.relatedGear);
            inventoryHoveringItem = false; // Pour éviter spam
        }
    }

}

