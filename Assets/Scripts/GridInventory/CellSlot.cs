using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
public class CellSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rect;
    Image sprite;
    public bool fill;
    public int FID = -1; // Fill ID
    public Color normalC, hoverC, fillC, fillHoverC = Color.red;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!fill)
            sprite.color = hoverC;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!fill)
            sprite.color = normalC; 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void SetFill(int FID)
    {
        this.FID = FID;
        fill = true;
        sprite.color = fillC;
    }
    public void SetEmpty()
    {         
        FID = -1;
        fill = false;
        sprite.color = normalC;
    }
    public void UpdateCell() 
    {
        if (fill)
            sprite.color = fillC;
        else sprite.color = normalC;

    }

    public bool isOverlaped(RectTransform rectB, int id) 
    {
        Rect rectAWorld = GetWorldSpaceRect(rect);
        Rect rectBWorld = GetWorldSpaceRect(rectB);

        bool hover = rectAWorld.Overlaps(rectBWorld);

        if (hover && !fill)
            sprite.color = hoverC;
        else if(hover && fill &&id != FID)
            sprite.color = fillHoverC;
        else if(!fill)
            sprite.color = normalC;
        else if(fill)
            sprite.color = fillC;

        return hover;

    }

    Rect GetWorldSpaceRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector2 min = new Vector2(corners.Min(c => c.x), corners.Min(c => c.y));
        Vector2 size = new Vector2(corners.Max(c => c.x) - min.x, corners.Max(c => c.y) - min.y);
        return new Rect(min, size);

    }
}
