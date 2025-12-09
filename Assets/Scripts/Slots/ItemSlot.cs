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

    private bool hasDragged = false; // <--- AJOUTÉ

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
            Color newColor = itemImage.color;
            newColor.a = 1f;
            itemImage.color = newColor;
        }
        else
        {
            itemImage.sprite = null;
            Color newColor = itemImage.color;
            newColor.a = 0f;
            itemImage.color = newColor;
        }
        itemTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        hasDragged = false; // <--- RESET
        cg.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;

        // 🔒 Si on n’a pas bougé → simple clic → on ne supprime pas
        if (!hasDragged)
        {
            itemTransform.anchoredPosition = Vector3.zero;
            return;
        }

        bool foundSlot = false;

        foreach (GameObject overObj in eventData.hovered)
        {
            if (overObj != gameObject)
            {
                if (overObj.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
                {
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

        if (!foundSlot)
        {
            // ❌ Drag + drop hors d’un slot → supprimer l’item
            itemTransform.anchoredPosition = Vector3.zero;

            if (currItem != null)
            {
                ResourceManager.Instance.AddResource(currItem, 1);
                currItem = null;
                UpdateSlotData();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currItem != null)
        {
            hasDragged = true; // <--- IMPORTANT
            itemTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }
}
