using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
public class GridItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int ID;
    public GridInventory inv;
    public bool canRotate = true;
    public bool isInInventory = false;

    bool isDrag;
    Vector2 CurrentPos;
    Vector2 offset;
    Vector2 localMousePosition;

    RectTransform rect;
    Image sprite;

    HashSet<CellSlot> cells = new HashSet<CellSlot>();
    HashSet<CellSlot> oldCells = new HashSet<CellSlot>();
    HashSet<Vector2> tp = new HashSet<Vector2>();
    Transform currentParent;
    void Start()
    {
        ID=Mathf.RoundToInt(Random.Range(0,1000000));
        sprite = GetComponent<Image>();

        rect = GetComponent<RectTransform>();
        CurrentPos = rect.anchoredPosition;
        currentParent=transform.parent;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.parent=inv.transform;
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        for (int i = 0; i < inv.cells.Count; i++)
        {
            var c = inv.cells[i];

            if ((c.FID == ID))
            {
                c.SetEmpty();
            }
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inv.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out offset);
        offset -= rect.anchoredPosition;
        isDrag = true;
        sprite.raycastTarget = false;
        CurrentPos = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
       RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inv.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out localMousePosition);
        rect.anchoredPosition = localMousePosition - offset; 

        tp.Clear();
        cells.Clear();

        for (int i = 0; i < inv.cells.Count; i++)
        {
            var c = inv.cells[i];
            if (c.isOverlaped(rect,ID))
            {
                Vector2 cellLocalPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                inv.GetComponent<RectTransform>(),
                c.rect.position,
                eventData.pressEventCamera,
                out cellLocalPosition);

                tp.Add(cellLocalPosition);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
       isDrag=false;
        sprite.raycastTarget = true;
        
        if(tp.Count == 0)
        {
            if(!isInInventory)transform.parent=currentParent;
            else
            {
                rect.anchoredPosition = CurrentPos;

                foreach(var c in oldCells)
                    c.SetFill(ID);
            }
            foreach (var c in inv.cells)
                c.UpdateCell();
            return;
        }
        rect.anchoredPosition =tp.First();

        for(int i = 0;i<inv.cells.Count;i++)
        {
            var c = inv.cells[i];
            if (c.isOverlaped(rect,ID))
            {
                cells.Add(c);
            }


        }
        foreach(var c in cells) 
        {
            if(c.FID != -1 && c.FID != ID)
            {
                if(!isInInventory)transform.parent=currentParent;
                else
                {
                    rect.anchoredPosition = CurrentPos;
                    foreach(var oc in oldCells)
                        oc.SetFill(ID);
                }
                foreach (var cc in inv.cells)
                    cc.UpdateCell();
                return;
            }
        }
        foreach(var c in cells)
        {
            c.SetFill(ID);
        }
        inv.AddItem(this);
        CurrentPos = tp.First();

        oldCells.Clear();
        foreach(var c in cells)
            oldCells.Add(c);

    }

    void UpdateSlots() 
    {  
        foreach(var c in cells)
            c.UpdateCell();
        tp.Clear();
        cells.Clear();

        foreach(var c in inv.cells) 
        {
            if (c.isOverlaped(rect,ID))
            {
                cells.Add(c);
                Vector2 cellLocalPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    inv.GetComponent<RectTransform>(),
                    c.rect.position,
                    null,
                    out cellLocalPosition);

                tp.Add(cellLocalPosition);
            }
        }

    }
   

    // Update is called once per frame
    void Update()
    {
        if(isDrag)
        {
            if(canRotate && Input.mouseScrollDelta.y >=1 || canRotate && Input.mouseScrollDelta.y <= 1) 
            {
                if(rect.pivot.x == 0)
                {
                    rect.rotation = Quaternion.Euler(0, 0, 90);
                    rect.pivot = new Vector2(1, 1);
                }
                else if(rect.pivot.x == 1)
                {
                    rect.rotation = Quaternion.Euler(0, 0, 0);
                    rect.pivot = new Vector2(0, 1);
                }

                rect.anchoredPosition = localMousePosition - offset;
                UpdateSlots();
            }
        }

    }
}
