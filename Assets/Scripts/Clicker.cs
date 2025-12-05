using UnityEditor.Rendering;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public static Clicker Instance;

    public Camera cam;           
    public LayerMask towerLayer; 
    [SerializeField] private UI_Manager ui_Manager;

    [HideInInspector]
    public  BaseTower currentSelectedTower = null;
    public TowerSlot currentSelectedTowerSlot = null;

    private bool isGridOpen = false;
    public bool isUpgradeOpen = false;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleClick();
        if (Input.GetKeyDown(KeyCode.F)) { DeselectTower(); }
    }

    private void HandleClick()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // On touche une tour ?
        if (Physics.Raycast(ray, out hit,999f, towerLayer))
        {
            BaseTower tower = hit.collider.GetComponent<BaseTower>();
            if (tower != null)
            {  
                SelectTower(tower);
                tower.OnTowerSelected();
            }

            TowerSlot towerSlot = hit.collider.GetComponent<TowerSlot>();
            if (towerSlot != null)
            {
                currentSelectedTowerSlot = towerSlot;
                TowerSelectMenuManager.Instance.ShowSlotTowerMenu(towerSlot);
            }
        }  
    }

    public void SelectTower(BaseTower tower)
    {
        // Fermer l’ancienne
        if (currentSelectedTower != null)
        {
            // Vérifie si la grille est ouverte
            isGridOpen = currentSelectedTower.gridUI.activeSelf;
            // Vérifie si le menu d'upgrade est ouvert
            isUpgradeOpen = TowerSelectMenuManager.Instance != null && TowerSelectMenuManager.Instance.upGradeMenu.activeSelf;
            currentSelectedTower.OnTowerDeselected();
        }

        // Ouvrir la nouvelle
        currentSelectedTower = tower;
        currentSelectedTower.OnTowerSelected();

        // Ouvrir automatiquement la grille de la nouvelle tour seulement si la grille était ouverte
        if (isGridOpen)
        {
            currentSelectedTower.ShowGrid();
            currentSelectedTower.inventoryMemory.DisplayInventory();
            ui_Manager.ShowGearMenu();
        }

        // Ouvrir automatiquement le menu d'upgrade de la nouvelle tour seulement si le menu d'upgrade était ouvert
        if (isUpgradeOpen)
        {
            TowerSelectMenuManager.Instance.ShowUpgradeMenu();
        }
    }





    public void DeselectTower()
    {
        Debug.Log("Je ferme");
        if (currentSelectedTower != null)
        {
            currentSelectedTower.OnTowerDeselected();
            ui_Manager.HideGearMenu();
            TowerSelectMenuManager.Instance.HideUpgradeMenu();
            currentSelectedTower = null;
        }
        isGridOpen = false;
        isUpgradeOpen = false;
    }

    public void OpenTowerInventoryGrid() 
    {
        if (currentSelectedTower != null) 
        { 
            currentSelectedTower.ShowGrid();
            currentSelectedTower.inventoryMemory.DisplayInventory();
            ui_Manager.ShowGearMenu();
            isGridOpen = true;
        }
    }
    public void CloseTowerInventoryGrid()
    {
        if ( currentSelectedTower != null) 
        {
            currentSelectedTower.HideGrid();
        }
    }
}


