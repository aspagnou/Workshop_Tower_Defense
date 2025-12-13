using UnityEngine;
using UnityEngine.EventSystems;

public class RecyclingTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float timer;
    bool hasMouse;
    public float toolTipDelay = 0.2f;
    private InventoryControler inventoryControler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasMouse = false;
        inventoryControler =FindAnyObjectByType<InventoryControler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasMouse && timer < toolTipDelay)
        {
            timer += Time.deltaTime;
            if (timer >= toolTipDelay)
            {

                ToolTipManager.Instance.ShowRecycle();

            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryControler.selectedItem != null)
        {
            Debug.Log("J'affiche");
            timer = 0;
            hasMouse = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManager.Instance.HideRecycle();
        hasMouse = false;
    }

}
