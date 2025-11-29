using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GearSlot : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler
{
    public GearSO currGear;

    public Image gearImage;
    public RectTransform gearTransform;

    private CanvasGroup cg;
    public Canvas canvas;

    private InventoryControler inventoryControler;
    // Start is called before the first frame update
    void Start()
    {
        inventoryControler = FindAnyObjectByType(typeof(InventoryControler)) as InventoryControler;
        cg = GetComponent<CanvasGroup>();
        UpdateSlotData();
    }

    public void UpdateSlotData()
    {
        if (currGear != null)
        {
            gearImage.sprite = currGear.gearIcon;
            // Réinitialiser l'alpha à 1 si un item est présent
            Color newColor = gearImage.color;
            newColor.a = 1f;
            gearImage.color = newColor;
        }
        else
        {
            gearImage.sprite = null;
            // Mettre l'alpha à 0 si aucun item n'est présent
            Color newColor = gearImage.color;
            newColor.a = 0f;
            gearImage.color = newColor;
        }
        gearTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cg.blocksRaycasts = false;

        // --- AJOUT : placer un item de l'inventaire dans un GearSlot ---
        if (inventoryControler.selectedItem != null)
        {
            Debug.Log("Tentative de placement d'un item dans un GearSlot");
            // Récupérer le gear correspondant à l’item
            GearSO relatedGear = inventoryControler.selectedItem.itemData.relatedGear;

            if (relatedGear != null)
            {
                // Placer directement dans le slot
                currGear = relatedGear;

                // Détruire l’item sélectionné
                Destroy(inventoryControler.selectedItem.gameObject);
                inventoryControler.selectedItem = null;

                // Mettre à jour l'affichage du slot
                UpdateSlotData();
            }

            return; // on s'arrête là, pas besoin de continuer
        }
        // ---------------------------------------------------------------

        // --- comportement original ---
        if (currGear != null)
        {
            inventoryControler.CreateItem(currGear.gearInventoryData);
            currGear = null;
            UpdateSlotData();
        }
        else
        {
            foreach (GameObject overObj in eventData.hovered)
            {
                if (overObj != gameObject)
                {
                    if (overObj.GetComponent<GearSlot>())
                    {
                        GearSlot itemSlot = overObj.GetComponent<GearSlot>();

                        GearSO prevGear = currGear;

                        currGear = itemSlot.currGear;
                        itemSlot.currGear = prevGear;

                        itemSlot.gearTransform.anchoredPosition = Vector3.zero;
                        itemSlot.UpdateSlotData();

                        if (inventoryControler.selectedItem != null)
                        {
                            Destroy(inventoryControler.selectedItem);
                            inventoryControler.selectedItem = null;
                        }
                    }
                }
            }
        }
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        bool foundSlot = false;

        foreach (GameObject overObj in eventData.hovered)
        {

            if (overObj != gameObject)
            {

                if (overObj.GetComponent<GearSlot>())
                {
                    GearSlot itemSlot = overObj.GetComponent<GearSlot>();

                    GearSO prevGear = currGear;

                    currGear = itemSlot.currGear;
                    itemSlot.currGear = prevGear;

                    itemSlot.gearTransform.anchoredPosition = Vector3.zero;
                    itemSlot.UpdateSlotData();
                    UpdateSlotData();

                    foundSlot = true;
                }
            }
        }

        if (!foundSlot)
        {
            gearTransform.anchoredPosition = Vector3.zero;
        }




        cg.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currGear != null)
        {
            gearTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }
}

