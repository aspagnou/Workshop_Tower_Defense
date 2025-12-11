using System.Collections;
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
    [Space(10)]
    [Header("Tower Select Animation")]
    public float towerSelectAnimDuration = 0.2f;
    public AnimationCurve towerSelectScaleCurve;
    private RectTransform towerSelectRect;
    private Coroutine towerSelectAnimRoutine;


    [Header("Upgrade Menu")]
    public GameObject upGradeMenu;
    private BaseTower currentlySelectedTower;
    private TowerUpgrade upgradeManager;
    private InventoryControler inventoryControler;
    public GameObject maxLevelPanel;
    public CanvasGroup upgradeConfirmButtonCanvasGroup;

    [Space(10)]
    [Header("SlotMenu")]
    public GameObject slotSelectPanel;
    public Vector3 slotWorldOffset = new Vector3(0, 2f, 0);
    private TowerSlot currentlySelectedTowerSlot;
    public TMP_Text[] slotCostTexts;
    public GameObject[] towerPrefabs;
    [SerializeField] GameObject slotPrefab;

    [Header("Slot Menu Animation")]
    public float slotAnimDuration = 0.2f;
    public AnimationCurve slotScaleCurve;
    private RectTransform slotRect;
    private Coroutine slotAnimRoutine;


    public bool IsSlotMenuOpen => slotSelectPanel.activeSelf;

    void Awake()
    {
        Instance = this;
        towerSelectPanel.SetActive(false);
        slotSelectPanel.SetActive(false);

        slotRect = slotSelectPanel.GetComponent<RectTransform>();
        slotRect.localScale = Vector3.zero; // Caché au départ

        towerSelectRect = towerSelectPanel.GetComponent<RectTransform>();
        towerSelectRect.localScale = Vector3.zero; // Caché au départ

        inventoryControler = FindAnyObjectByType<InventoryControler>();
        saveGrid = GameObject.FindWithTag("FixGridSpawn").GetComponent<ItemGrid>();
        HideUpgradeMenu();
    }

    // --------------------Slot Menu -------------------------------------------------------------------
    public void ShowSlotTowerMenu(TowerSlot towerSlot)
    {
        currentlySelectedTowerSlot = towerSlot;

        // Position
        Vector3 worldPos = towerSlot.transform.position + slotWorldOffset;
        slotSelectPanel.transform.position = worldPos;
        slotSelectPanel.transform.LookAt(cam.transform);
        slotSelectPanel.transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        slotSelectPanel.SetActive(true);

        // 🔥 Lance l’animation
        if (slotAnimRoutine != null) StopCoroutine(slotAnimRoutine);
        slotAnimRoutine = StartCoroutine(PlaySlotMenuPop());

        currentlySelectedTowerSlot.UpdateCostText();

        ResourceManager.Instance.OnManaChanged += UI_Manager.Instance.UpdateSlotColorText;
        UI_Manager.Instance.UpdateSlotColorText(ResourceManager.Instance.mana);

        HideTowerSelectMenu();
    }

    private IEnumerator PlaySlotMenuPop()
    {
        float t = 0f;

        // Start scale
        slotRect.localScale = Vector3.zero;

        while (t < slotAnimDuration)
        {
            t += Time.deltaTime;
            float lerp = t / slotAnimDuration;

            float scale = slotScaleCurve != null ?
                          slotScaleCurve.Evaluate(lerp) :
                          Mathf.Sin(lerp * Mathf.PI * 0.5f); // Ease-out default

            slotRect.localScale = Vector3.one * scale;

            yield return null;
        }

        slotRect.localScale = Vector3.one;
    }

    public void HideSlotTowerMenu()
    {
        if (slotAnimRoutine != null) StopCoroutine(slotAnimRoutine);
        slotAnimRoutine = StartCoroutine(CloseSlotMenu());

        ResourceManager.Instance.OnManaChanged -= UI_Manager.Instance.UpdateSlotColorText;
        currentlySelectedTowerSlot = null;
    }
    private IEnumerator CloseSlotMenu()
    {
        float t = 0f;
        Vector3 start = slotRect.localScale;

        while (t < slotAnimDuration)
        {
            t += Time.deltaTime;
            float lerp = t / slotAnimDuration;

            slotRect.localScale = Vector3.Lerp(start, Vector3.zero, lerp);
            yield return null;
        }

        slotSelectPanel.SetActive(false);
    }


    //------------------------ Tower Select Menu -----------------------------------------------------------
    public void ShowTowerSelectMenu(BaseTower tower)
    {
        currentlySelectedTower = tower;

        Vector3 worldPos = tower.transform.position + worldOffset;

        towerSelectPanel.transform.position = worldPos;
        towerSelectPanel.transform.LookAt(cam.transform);
        towerSelectPanel.transform.rotation = Quaternion.LookRotation(cam.transform.forward);

        towerSelectPanel.SetActive(true);

        // 🔥 Animation pop
        if (towerSelectAnimRoutine != null) StopCoroutine(towerSelectAnimRoutine);
        towerSelectAnimRoutine = StartCoroutine(PlayTowerSelectPop());

        HideSlotTowerMenu();
    }
    private IEnumerator PlayTowerSelectPop()
    {
        float t = 0f;
        towerSelectRect.localScale = Vector3.zero;

        while (t < towerSelectAnimDuration)
        {
            t += Time.deltaTime;
            float lerp = t / towerSelectAnimDuration;

            float scale = towerSelectScaleCurve != null ?
                          towerSelectScaleCurve.Evaluate(lerp) :
                          Mathf.Sin(lerp * Mathf.PI * 0.5f);

            towerSelectRect.localScale = Vector3.one * scale;

            yield return null;
        }

        towerSelectRect.localScale = Vector3.one;
    }



    public void HideTowerSelectMenu()
    {
        if (!towerSelectPanel.activeSelf) return;

        if (towerSelectAnimRoutine != null) StopCoroutine(towerSelectAnimRoutine);
        towerSelectAnimRoutine = StartCoroutine(CloseTowerSelectPop());

        currentlySelectedTower = null;
    }
    private IEnumerator CloseTowerSelectPop()
    {
        float t = 0f;
        Vector3 start = towerSelectRect.localScale;

        while (t < towerSelectAnimDuration)
        {
            t += Time.deltaTime;
            float lerp = t / towerSelectAnimDuration;

            towerSelectRect.localScale = Vector3.Lerp(start, Vector3.zero, lerp);

            yield return null;
        }

        towerSelectPanel.SetActive(false);
    }



    //-------------------------------- Upgrade Menu ----------------------------------------------------------------
    public void ShowUpgradeMenu()
    {
        if (Clicker.Instance.currentSelectedTower != null)
        {
            upGradeMenu.SetActive(true);
            currentlySelectedTower = Clicker.Instance.currentSelectedTower;
            upgradeManager = currentlySelectedTower.towerUpgradeManager;

            upgradeManager.Show(upgradeManager.currentLevel);
            Clicker.Instance.isUpgradeOpen = true;

            // Vérifie si on est au niveau max
            maxLevelPanel.SetActive(upgradeManager.currentLevel >= 2);
        }
    }

    public void HideUpgradeMenu()
    {
        upGradeMenu.SetActive(false);
        maxLevelPanel.SetActive(false);
        Clicker.Instance.isUpgradeOpen = false;
    }

    public void ConfirmUpgrade()
    {
        Clicker.Instance.currentSelectedTower = currentlySelectedTower;
        upgradeManager = currentlySelectedTower.towerUpgradeManager;

        upgradeManager.ConfirmUpgrade();

        // 🔥 Mise à jour immédiate après l'amélioration
        maxLevelPanel.SetActive(upgradeManager.currentLevel >= 2);

        // Réaffiche les coûts mis à jour si pas encore au max
        if (upgradeManager.currentLevel < upgradeManager.upgradeLevels.Length)
            upgradeManager.Show(upgradeManager.currentLevel);
    }

    public void UpdateUpgradeButton()
    {
        upgradeManager.UpdateUpgradeButtonState();
    }



    //----------------------- Sell Tower ------------------------------------------------------------------
    public void SellTower()
    {
        InventoryMemory gearInventory = Clicker.Instance.currentSelectedTower.inventoryMemory;
        TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.GetComponent<TowerUpgrade>();

        Instantiate(slotPrefab, towerUpgrade.gameObject.transform.position, Quaternion.identity);

        if (towerUpgrade != null)
        {
            ResourceManager.Instance.AddMana(towerUpgrade.sellAmount);

            foreach (GearSO gear in gearInventory.storedGears)
            {
                inventoryControler.RetrieveGear(gear);
                inventoryControler.inventoryHighlight.SetParent(saveGrid);
            }

            Clicker.Instance.DeselectTower();
            Destroy(towerUpgrade.gameObject);
        }
    }

    // ---------------------Spawn Tower From Slot -------------------------------------------------------
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
