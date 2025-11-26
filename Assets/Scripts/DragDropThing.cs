using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropThing : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private ResourceManager resourceManager;
    public bool _resetPositionOnRelease = true;
    [SerializeField] private ItemSO resource;
    Vector3 _startPosition;

    void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"Dragging {eventData.position}");
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"Begin Drag {eventData.position}");

        if (_resetPositionOnRelease)
            _startPosition = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log($"End Drag {eventData.position}");

        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);

        var hit = hits.FirstOrDefault(t => t.gameObject.CompareTag("Droppable"));
        if (hit.isValid)
        {
            Debug.Log($"Dropped {gameObject} on {hit.gameObject}");
            hit.gameObject.TryGetComponent<ItemSlot>(out ItemSlot itemSlot);
            if (itemSlot != null)
            {
                itemSlot.currItem=resource;
                itemSlot.UpdateSlotData();
                resourceManager.UseResource(resource, 1);
                Destroy(gameObject);
            }
            return;
        }

        if (_resetPositionOnRelease)
            transform.position = _startPosition;
    }
}