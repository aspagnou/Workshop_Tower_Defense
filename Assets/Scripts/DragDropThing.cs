using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropThing : MonoBehaviour, IPointerClickHandler
{
    private ResourceManager resourceManager;

    [SerializeField] private ItemSO resource;
    public bool resetPositionOnRelease = true;

    private bool isSelected = false;
    private Vector3 startPosition;

    void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
    }

    void Update()
    {
        // Si l’item est sélectionné, il suit la souris
        if (isSelected)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 1er clic  Sélectionne l’objet
        if (!isSelected)
        {
            isSelected = true;
            startPosition = transform.position;
            return;
        }

        // 2e clic  Tentative de placement
        isSelected = false;

        // Raycast UI sous le clic
        var hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);

        var hit = hits.FirstOrDefault(t => t.gameObject.CompareTag("Droppable"));

        if (hit.isValid)
        {
            // On a cliqué sur un slot valide
            if (hit.gameObject.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
            {
                itemSlot.currItem = resource;
                itemSlot.UpdateSlotData();

                resourceManager.UseResource(resource, 1);

                Destroy(gameObject);
            }
        }
        else
        {
            // Clique hors d'un "Droppable"  Retour à la position d’origine
            if (resetPositionOnRelease)
                transform.position = startPosition;
        }
    }
}
