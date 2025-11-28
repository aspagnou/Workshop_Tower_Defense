using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GearSlot : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public GearSO currGear;

    public Image gearImage;
    public RectTransform gearTransform;

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
}

