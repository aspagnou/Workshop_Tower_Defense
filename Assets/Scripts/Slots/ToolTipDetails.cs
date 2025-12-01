using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TitleText;
    public string DetailsText;
    public float toolTipDelay = 0.2f;
    float timer;

    private PreviewGearSlot previewGearSlot;
    private OutputSlot outputSlot;
    private GearSlot gearSlot;

    bool hasMouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previewGearSlot = GetComponent<PreviewGearSlot>();
        outputSlot = GetComponent<OutputSlot>();
        gearSlot = GetComponent<GearSlot>();
        hasMouse = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMouse && timer < toolTipDelay) 
        {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay) 
            {
                
                if (previewGearSlot != null)
                {
                    ToolTipManager.Instance.Show(previewGearSlot.currGear);
                }
                if (outputSlot != null)
                {
                    ToolTipManager.Instance.Show(outputSlot.currGear);
                }
                if (gearSlot != null)
                {
                    ToolTipManager.Instance.Show(gearSlot.currGear);
                }

            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        timer = 0;
        hasMouse = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.Hide();
        hasMouse = false;
    }
}