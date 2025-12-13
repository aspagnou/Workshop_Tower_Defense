using System;
using System.Net.Http.Headers;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class ToolTipManager : MonoBehaviour
{
    [Header("Base Tooltip")]

    public Canvas parentCanvas;
    public Transform toolTipTransform;
    public static ToolTipManager Instance;
    public TMP_Text Title, Details;
    public CanvasGroup toolTipCanvasGroup;
    [SerializeField] private Transform lineSpawnTransform;
    [SerializeField] GameObject statLinePrefab;

    [Header("Preview Grid")]
    public Transform itemGridParent;
    public GameObject tilePrefab;


    [Space (20)]

    [Header("Recycle ToolTip")]
    bool isShowingBase;
    bool isShowingRecycle;
    public CanvasGroup recycleCanvasGroup;
    [SerializeField] GameObject RecycleToolTip;
    [SerializeField] private Transform recycleLineSpawnTransform;
    [SerializeField] GameObject recycleLinePrefab;
    public Sprite[] Scraps;

    [Header("icons")]
    // Icônes pour chaque stat
    public Sprite attackDamageIcon;
    public Sprite rangeIcon;
    public Sprite attackSpeedIcon;
    public Sprite criticalChanceIcon;

    
    private InventoryControler inventoryControler;
    private ToolTipManager toolTipManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        isShowingBase = false;
        isShowingRecycle = false;
        inventoryControler = FindAnyObjectByType<InventoryControler>();
        toolTipManager = FindAnyObjectByType<ToolTipManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;
        if (isShowingBase)
        {
            if (toolTipCanvasGroup.alpha < 1)
            {
                toolTipCanvasGroup.alpha += Time.deltaTime * 3;
            }

            // Vérifie si la souris est toujours sur un objet UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Hide();
                return;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition,
                parentCanvas.worldCamera,
                out movePos
            );
            toolTipTransform.position = parentCanvas.transform.TransformPoint(movePos);
        }

        if (isShowingRecycle)
        {
            if (recycleCanvasGroup.alpha < 1)
            {
                recycleCanvasGroup.alpha += Time.deltaTime * 3;
            }
        }
    }


    //child.gameObject != Title.gameObject && 
    public void Show(GearSO gear)
    {
        if (gear == null) return;

        toolTipCanvasGroup.alpha = 0;

        // Nettoie anciennes lignes
        foreach (Transform child in lineSpawnTransform)
        {
            if (child.gameObject != itemGridParent.gameObject)
                Destroy(child.gameObject);
        }

        // Nettoie ancienne grille
        foreach (Transform child in itemGridParent)
            Destroy(child.gameObject);

        // Ajoute statistiques
        AddStatLine(attackDamageIcon, gear.flatAttackDamage, false);
        AddStatLine(rangeIcon, gear.flatRange, false);
        AddStatLine(attackSpeedIcon, gear.flatAttackSpeed, false);
        AddStatLine(criticalChanceIcon, gear.flatCriticalChance, false);

        AddStatLine(attackDamageIcon, gear.percentAttackDamage, true);
        AddStatLine(rangeIcon, gear.percentRange, true);
        AddStatLine(attackSpeedIcon, gear.percentAttackSpeed, true);

        // --- NOUVEAU : affichage de la grille ---
        if (gear.gearInventoryData != null)
        {
            DrawItemGrid(
                gear.gearInventoryData.width,
                gear.gearInventoryData.height
            );
        }

        toolTipTransform.gameObject.SetActive(true);
        isShowingBase = true;

    }
    private void DrawItemGrid(int width, int height)
    {
        if (tilePrefab == null || itemGridParent == null)
        {
            Debug.LogWarning("Tile prefab or grid parent is missing!");
            return;
        }

        // Ajuste automatiquement la grille
        GridLayoutGroup grid = itemGridParent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = width;
        }

        // Génère width * height tiles
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Instantiate(tilePrefab, itemGridParent);
            }
        }

        itemGridParent.gameObject.SetActive(true);
    }



    // Méthode pour ajouter une ligne de stat
    private void AddStatLine(Sprite icon, float value, bool isPercent)
    {
        if (value == 0) return; // Ne pas afficher si la valeur est 0

        // Instancie une nouvelle ligne
        GameObject statLine = Instantiate(statLinePrefab, lineSpawnTransform);

        // Récupère les composants de la ligne
        Image iconImage = statLine.transform.GetChild(0).GetComponent<Image>();
        TMP_Text statText = statLine.transform.GetChild(1).GetComponent<TMP_Text>();

        // Configure l'icône
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(icon != null);

        // Configure le texte
        if (isPercent)
        {
            statText.text = $"+{value}%";
        }
        else
        {
            statText.text = $"+{value}";
        }
    }

    public void Hide()
    {
        toolTipTransform.gameObject.SetActive(false);
        isShowingBase = false;
    }

    public void ShowRecycle() 
    {
        GearSO gear = inventoryControler.selectedItem.itemData.relatedGear;
        if (gear == null) return;

        recycleCanvasGroup.alpha = 0;
        // Supprime les anciennes lignes (si elles existent)
        foreach (Transform child in recycleLineSpawnTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemSO item in gear.recycleRessources) 
        {
            int i= 0;   
            if (item != null)
            {
                if (inventoryControler.selectedItem != null)
                {
                    
                    int amount = gear.amounts[i];
                    AddRecycleLine(item.itemIcon, amount);
                    i++;
                }
                
            }
        }
        RecycleToolTip.SetActive(true);
        isShowingRecycle = true;
    }
    public void AddRecycleLine(Sprite icon,float value) 
    {
        if (value == 0) return; // Ne pas afficher si la valeur est 0
        Debug.Log("Jajoute une ligne");
        // Instancie une nouvelle ligne
        GameObject statLine = Instantiate(recycleLinePrefab, recycleLineSpawnTransform);

        // Récupère les composants de la ligne
        Image iconImage = statLine.transform.GetChild(0).GetComponent<Image>();
        TMP_Text scrapText = statLine.transform.GetChild(1).GetComponent<TMP_Text>();
        Debug.Log(iconImage);
        Debug.Log(scrapText);

        // Configure l'icône
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(icon != null);
        scrapText.text = $"+{value}";
        
    }

    public void HideRecycle() 
    {
        RecycleToolTip.SetActive(false);
        isShowingRecycle = false;
    }
}
