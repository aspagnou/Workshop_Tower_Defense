using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutputSlot : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,  IPointerEnterHandler
{
    public GearSO currGear;
    public Image gearImage;
    public RectTransform gearTransform;

    private ResourceManager resourceManager;
    private GearManager gearManager;
    private CanvasGroup cg;


    public Canvas canvas;

    [Header("Drawer Animation")]
    public RectTransform drawerTransform;
    public Vector2 closedPos;
    public Vector2 openPos;
    public float animSpeed = 8f;

    private bool drawerOpen = false;
    private float lastClickTime = 0f;
    //[SerializeField] private float doubleClickTimeThreshold = 0.3f;

    void Start()
    {
        resourceManager = FindAnyObjectByType<ResourceManager>();
        gearManager = FindAnyObjectByType<GearManager>();
        cg = GetComponent<CanvasGroup>();
        UpdateSlotData();
    }
    void Update()
    {
        Vector2 target = drawerOpen ? openPos : closedPos;
        drawerTransform.anchoredPosition =
        Vector2.Lerp(drawerTransform.anchoredPosition, target, Time.deltaTime * animSpeed);
        

    }

    public void UpdateSlotData()
    {
        if (currGear != null)
        {
            gearImage.sprite = currGear.gearIcon;
            Color c = gearImage.color;
            c.a = 1f;
            gearImage.color = c;

            drawerOpen = true;   // 👉 On ouvre
        }
        else
        {
            Color c = gearImage.color;
            c.a = 0f;
            gearImage.color = c;
            gearImage.sprite = null;

            drawerOpen = false;  // 👉 On ferme
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
                    GearSlot outputSlot = overObj.GetComponent<GearSlot>();
                    GearSO prevGear = currGear;
                    currGear = outputSlot.currGear;
                    outputSlot.currGear = prevGear;
                    outputSlot.gearTransform.anchoredPosition = Vector3.zero;
                    outputSlot.UpdateSlotData();
                    UpdateSlotData();
                    foundSlot = true;
                    resourceManager.ClearSlots();
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
            Debug.Log("Je Drag");
            gearTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currGear != null && !gearManager.IsFullGear())
        {
            gearManager.AddGear(currGear);
            currGear = null;
            resourceManager.ClearSlots();
            UpdateSlotData();
        }
        
        lastClickTime = Time.time;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }
}
