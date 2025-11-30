using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseTower : MonoBehaviour, IPointerEnterHandler
{
    private GameObject mainCanvas;
    public GameObject gridUI;
    public ItemGrid itemGrid;
    public InventoryMemory inventoryMemory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCanvas = GameObject.FindWithTag("MainCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ShowGrid();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            HideGrid();
        }
    }

    public void HideGrid()
    {
        gridUI.transform.SetParent(this.transform);
    }

    public void ShowGrid()
    {
        gridUI.transform.SetParent(mainCanvas.transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered on tower: " + this.name);
    }
}
