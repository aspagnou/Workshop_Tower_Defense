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

    [Header("Upgrade Menu")]
    public GameObject upGradeMenu;
    private BaseTower currentlySelectedTower;
    private TowerUpgrade upgradeManager;
    private InventoryControler inventoryControler;

    void Awake()
    {
        Instance = this;
        towerSelectPanel.SetActive(false);
        inventoryControler=FindAnyObjectByType<InventoryControler>();
        saveGrid= GameObject.FindWithTag("FixGridSpawn").GetComponent<ItemGrid>();
    }

    public void ShowTowerSelectMenu(BaseTower tower)
    {
        currentlySelectedTower = tower;

        // Position de base = position de la tour
        Vector3 worldPos = tower.transform.position;

        // Offset vertical (en unités monde)
        worldPos += new Vector3(0, 2f, 0); // Ajuste la hauteur selon ta scène

        // Appliquer la position au panel
        towerSelectPanel.transform.position = worldPos;

        // Toujours orienter le panel vers la caméra
        towerSelectPanel.transform.LookAt(cam.transform);
        towerSelectPanel.transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        // Activer le panel
        towerSelectPanel.SetActive(true);
    }


    public void HideTowerSelectMenu()
    {
        towerSelectPanel.SetActive(false);
        currentlySelectedTower = null;
    }


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

    public void SellTower() 
    {
        InventoryMemory gearInventory = Clicker.Instance.currentSelectedTower.inventoryMemory;
        TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.GetComponent<TowerUpgrade>();
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
    
}
