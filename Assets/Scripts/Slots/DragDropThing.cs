using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropThing : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ResourceManager resourceManager;
    public static DragDropThing currentDrag = null;

    [SerializeField] private ItemSO resource;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector3 startPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    void Start()
    {
        resourceManager = FindFirstObjectByType<ResourceManager>();
        canvas = GetComponentInParent<Canvas>();
    }

    // -------------------------------------------------------------
    // 🔵 DEBUT DU DRAG
    // -------------------------------------------------------------
    public void OnBeginDrag(PointerEventData eventData)
    {
        currentDrag = this;                             // <<< On retient qui est draggé
        startPosition = rectTransform.anchoredPosition;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.7f;
    }

    // -------------------------------------------------------------
    // 🟡 LORS DU DRAG
    // -------------------------------------------------------------
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    // -------------------------------------------------------------
    // 🔴 FIN DU DRAG
    // -------------------------------------------------------------
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        currentDrag = null;                             // <<< Fin du drag


        // Raycast UI pour voir ce qu’on a lâché
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Trouver un objet "Droppable"
        var hit = results.FirstOrDefault(r => r.gameObject.CompareTag("Droppable"));

        if (hit.isValid)
        {
            if (hit.gameObject.TryGetComponent<ItemSlot>(out ItemSlot itemSlot))
            {
                // Placement réussi

                if (itemSlot.currItem != null)
                {
                    // Le slot est déjà occupé
                    ResourceManager.Instance.AddResource(itemSlot.currItem, 1);



                }
                itemSlot.currItem = resource;
                itemSlot.UpdateSlotData();

                resourceManager.UseResource(resource, 1);

                Destroy(gameObject);  // Supprime l’item draggable
                return;
            }
        }

        // Aucun slot valide → retour à la position de départ
        BackToStart();
    }
    public void ForceCancelDrag()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        currentDrag = null;

        // Retour à la position initiale
        rectTransform.anchoredPosition = startPosition;
    }
    private void BackToStart()
    {
        rectTransform.anchoredPosition = startPosition;
    }
}
