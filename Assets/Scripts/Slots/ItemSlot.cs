using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public ItemSO currItem;
    
    public Image itemImage;
    public RectTransform itemTransform;

    private CanvasGroup cg;
    public Canvas canvas;

    
    

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        UpdateSlotData();
    }

    public void UpdateSlotData()
    {
        if (currItem != null)
        {
            itemImage.sprite = currItem.itemIcon;
            // Réinitialiser l'alpha à 1 si un item est présent
            Color newColor = itemImage.color;
            newColor.a = 1f;
            itemImage.color = newColor;
        }
        else
        {
            itemImage.sprite = null;
            // Mettre l'alpha à 0 si aucun item n'est présent
            Color newColor = itemImage.color;
            newColor.a = 0f;
            itemImage.color = newColor;
        }
        itemTransform.anchoredPosition = Vector3.zero;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        cg.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        bool foundSlot = false;

        foreach(GameObject overObj in eventData.hovered)
        {
            if(overObj != gameObject)
            {
                if (overObj.GetComponent<ItemSlot>())
                {
                    ItemSlot itemSlot = overObj.GetComponent<ItemSlot>();

                    ItemSO prevItem = currItem;

                    currItem = itemSlot.currItem;
                    itemSlot.currItem = prevItem;

                    itemSlot.itemTransform.anchoredPosition = Vector3.zero;
                    itemSlot.UpdateSlotData();
                    UpdateSlotData();

                    foundSlot = true;
                }
            }
        }

        if(!foundSlot)
        {
            itemTransform.anchoredPosition = Vector3.zero;
        }

        cg.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currItem != null)
        {
            itemTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
