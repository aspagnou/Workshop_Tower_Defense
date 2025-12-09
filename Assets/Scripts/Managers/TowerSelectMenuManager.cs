using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectMenuManager : MonoBehaviour
{
    public static TowerSelectMenuManager Instance;
    ItemGrid saveGrid;
    [Header("SelectMenu")]
    public GameObject towerSelectPanel;
    public Camera cam;
    public GameObject mainCanvas;
    public Vector3 worldOffset = new Vector3(0, 2f, 0);
    public bool IsTowerMenuOpen => towerSelectPanel.activeSelf;

    

    [Header("Upgrade Menu")]
    public GameObject upGradeMenu;
    private BaseTower currentlySelectedTower;
    private TowerUpgrade upgradeManager;
    private InventoryControler inventoryControler;

    [Space (10)]
    [Header("SlotMenu")] 
    public GameObject slotSelectPanel;
    public Vector3 slotWorldOffset = new Vector3(0, 2f, 0);
    private TowerSlot currentlySelectedTowerSlot;
    public TMP_Text[] slotCostTexts;
    public GameObject[] towerPrefabs;
    [SerializeField] GameObject slotPrefab;

    public bool IsSlotMenuOpen => slotSelectPanel.activeSelf;

    

    void Awake()
    {
        Instance = this;
        towerSelectPanel.SetActive(false);
        slotSelectPanel.SetActive(false);
        inventoryControler =FindAnyObjectByType<InventoryControler>();
        saveGrid= GameObject.FindWithTag("FixGridSpawn").GetComponent<ItemGrid>();
    }

    // Slot Menu
    public void ShowSlotTowerMenu(TowerSlot towerSlot) 
    {
        currentlySelectedTowerSlot = towerSlot;
        Vector3 worldPos = towerSlot.transform.position;
        worldPos += slotWorldOffset; // Ajuste la hauteur selon ta scène

        slotSelectPanel.transform.position = worldPos;
        slotSelectPanel.transform.LookAt(cam.transform);
        slotSelectPanel.transform.rotation = Quaternion.LookRotation(cam.transform.forward);
        slotSelectPanel.SetActive(true);
        currentlySelectedTowerSlot.UpdateCostText();
        // On s'abonne AU MOMENT où le menu apparaît
        ResourceManager.Instance.OnManaChanged += UI_Manager.Instance.UpdateSlotColorText;

        // On force une mise à jour immédiate
        UI_Manager.Instance.UpdateSlotColorText(ResourceManager.Instance.mana);
        HideTowerSelectMenu();

    }
    public void HideSlotTowerMenu() 
    {
        slotSelectPanel.SetActive(false);
        ResourceManager.Instance.OnManaChanged -= UI_Manager.Instance.UpdateSlotColorText;
        currentlySelectedTowerSlot = null;
    }

   
    // Tower Select Menu
    public void ShowTowerSelectMenu(BaseTower tower)
    {
        currentlySelectedTower = tower;

        // Position de base = position de la tour
        Vector3 worldPos = tower.transform.position;

        // Offset vertical (en unités monde)
        worldPos += worldOffset; // Ajuste la hauteur selon ta scène

        // Appliquer la position au panel
        towerSelectPanel.transform.position = worldPos;

        // Toujours orienter le panel vers la caméra
        towerSelectPanel.transform.LookAt(cam.transform);
        towerSelectPanel.transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        // Activer le panel
        towerSelectPanel.SetActive(true);
        HideSlotTowerMenu();
    }
    public void HideTowerSelectMenu()
    {
        towerSelectPanel.SetActive(false);
        currentlySelectedTower = null;
    }

    // Upgrade Menu
    public void ShowUpgradeMenu()
    {
        if (Clicker.Instance.currentSelectedTower != null)
        {
            upGradeMenu.SetActive(true);
            currentlySelectedTower = Clicker.Instance.currentSelectedTower;
            upgradeManager = currentlySelectedTower.towerUpgradeManager;
            upgradeManager.Show(upgradeManager.currentLevel);
            Clicker.Instance.isUpgradeOpen = true;
        }
    }
    public void HideUpgradeMenu()
    {
        upGradeMenu.SetActive(false);
        Clicker.Instance.isUpgradeOpen = false;
    }
    public void ConfirmUpgrade() 
    {
        Clicker.Instance.currentSelectedTower = currentlySelectedTower;
        upgradeManager = currentlySelectedTower.towerUpgradeManager;
        upgradeManager.ConfirmUpgrade();
    }

    // Sell Tower
    public void SellTower() 
    {
        InventoryMemory gearInventory = Clicker.Instance.currentSelectedTower.inventoryMemory;
        TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.GetComponent<TowerUpgrade>();
        Instantiate(slotPrefab, towerUpgrade.gameObject.transform.position,Quaternion.identity);
        if (towerUpgrade != null) 
        {
            ResourceManager.Instance.AddMana(towerUpgrade.sellAmount);
            foreach(GearSO gear in gearInventory.storedGears) 
            {
                inventoryControler.RetrieveGear(gear);
                inventoryControler.inventoryHighlight.SetParent(saveGrid);
                
            }
            Clicker.Instance.DeselectTower();
            Destroy(towerUpgrade.gameObject);
        }
    }
    
    // SpawnTower
    public void SpawnTowerFromSlot(int i) 
    {
        Clicker.Instance.currentSelectedTowerSlot.SpawnTower(i);
    }
    public bool IsCurrentSlot(TowerSlot slot)
    {
        return currentlySelectedTowerSlot == slot;
    }
    public bool IsCurrentTower(BaseTower tower)
    {
        return currentlySelectedTower == tower;
    }
}
