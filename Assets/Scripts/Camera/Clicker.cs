
using UnityEngine;
using UnityEngine.EventSystems;

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
        // 0️⃣ Empêche de fermer quoi que ce soit si on clique sur l’UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 999f, towerLayer))
        {
            BaseTower tower = hit.collider.GetComponent<BaseTower>();
            TowerSlot towerSlot = hit.collider.GetComponent<TowerSlot>();

            // ------------------------------------------------------
            // 🟦 1️⃣ Gestion TowerSlot + SlotMenu
            // ------------------------------------------------------
            if (towerSlot != null)
            {
                // Si menu slot déjà ouvert et même slot -> fermer
                if (TowerSelectMenuManager.Instance.IsSlotMenuOpen &&
                    TowerSelectMenuManager.Instance.IsCurrentSlot(towerSlot))
                {
                    TowerSelectMenuManager.Instance.HideSlotTowerMenu();
                    return;
                }

                // Si menu tower était ouvert → on le ferme
                if (TowerSelectMenuManager.Instance.IsTowerMenuOpen)
                {
                    TowerSelectMenuManager.Instance.HideTowerSelectMenu();
                }

                // Ouvrir le menu pour ce slot
                currentSelectedTowerSlot = towerSlot;
                TowerSelectMenuManager.Instance.ShowSlotTowerMenu(towerSlot);
                return;
            }

            // ------------------------------------------------------
            // 🟥 2️⃣ Gestion Tour + TowerSelectMenu
            // ------------------------------------------------------
            if (tower != null)
            {
                // Si tower menu ouvert + même tour -> fermer
                if (TowerSelectMenuManager.Instance.IsTowerMenuOpen &&
                    TowerSelectMenuManager.Instance.IsCurrentTower(tower))
                {
                    TowerSelectMenuManager.Instance.HideTowerSelectMenu();
                    return;
                }

                // Si slot menu ouvert → fermer
                if (TowerSelectMenuManager.Instance.IsSlotMenuOpen)
                {
                    TowerSelectMenuManager.Instance.HideSlotTowerMenu();
                }

                // Sélection de tour
                SelectTower(tower);
                tower.OnTowerSelected();
                return;
            }
        }

        // ------------------------------------------------------
        // 🟩 3️⃣ Aucun hit → clic dans le vide
        // ------------------------------------------------------
        // Fermer SlotMenu si ouvert
        if (TowerSelectMenuManager.Instance.IsSlotMenuOpen)
            TowerSelectMenuManager.Instance.HideSlotTowerMenu();

        // Fermer TowerMenu si ouvert
        if (TowerSelectMenuManager.Instance.IsTowerMenuOpen)
            TowerSelectMenuManager.Instance.HideTowerSelectMenu();
        // Désélectionner la tour actuelle
        DeselectTower();
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


