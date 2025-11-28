using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridInventory : MonoBehaviour
{
    public Transform Grid;
    public List<CellSlot> cells = new List<CellSlot>();
    public List<GridItem> items = new List<GridItem>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Grid.childCount; i++)
        {
            cells.Add(Grid.GetChild(i).GetComponent<CellSlot>());
        }
    }
    public void AddItem(GridItem item)
    {
        if(!items.Any(c=>c.ID==item.ID))
        {
            item.isInInventory = true;
            items.Add(item);
        }
    }
}
