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
        }
        else
        {
            gearImage.sprite = null;

        }

        gearTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cg.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        gearTransform.anchoredPosition = Vector3.zero;
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

