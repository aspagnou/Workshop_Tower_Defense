using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipDetails : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string TitleText;
    public string DetailsText;
    public float toolTipDelay = 0.2f;
    float timer;

    private PreviewGearSlot PreviewGearSlot;

    bool hasMouse;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PreviewGearSlot = GetComponent<PreviewGearSlot>();
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
                
                if (PreviewGearSlot != null)
                {
                    ToolTipManager.Instance.Show(PreviewGearSlot.currGear);
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