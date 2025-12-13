using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Manager : MonoBehaviour
{
    [Header("AudioManager")]
    [SerializeField] VolumeSettings audioVolumePanel;

    public static UI_Manager Instance;
    [Header("Ressource_Icons")]
    public RectTransform[] rectImages;
    [SerializeField] private GameObject[] ressourceIcon;
    [SerializeField] private TMP_Text[] ressourceTextCraft, ressourceTextHUD;
    [SerializeField] private GameObject craftMenu;
    [SerializeField] private GameObject gearInventoryMenu;
    
    public TMP_Text[] statLines;
    public StatChangeFeedback[] statsUpFeedback;

    [Header("Craft Menu Animation")]
    public RectTransform craftMenuRect;
    public float slideDuration = 0.25f;

    private Vector2 craftMenuClosedPos;
    private Vector2 craftMenuOpenPos;
    private Coroutine slideRoutine;
    [SerializeField ]private AiguilleCraft[] aiguilleCrafts;

    // Dictionnaire pour stocker les icônes instanciées par type de ressource
    private Dictionary<int, List<GameObject>> spawnedIcons = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        Instance = this;
        HideGearMenu();
        
        craftMenuRect = craftMenu.GetComponent<RectTransform>();

        craftMenuOpenPos = craftMenuRect.anchoredPosition;
        craftMenuClosedPos = craftMenuOpenPos + new Vector2(-600f, 0f); // Décale hors écran

        craftMenuRect.anchoredPosition = craftMenuClosedPos; // Start hidden
        HideCraftMenu();
        foreach(AiguilleCraft aiguille in aiguilleCrafts)
        {
            aiguille.TogglePosition();
        }
        // Initialiser le dictionnaire
        for (int i = 0; i < ressourceIcon.Length; i++)
        {
            spawnedIcons[i] = new List<GameObject>();
        }
        
    }

    public void Start()
    {
        audioVolumePanel.LoadVolume();
    }
    public void SpawnResource(int index)
    {
        // Instancie l'icône comme enfant de rectImage
        GameObject newIcon = Instantiate(ressourceIcon[index], rectImages[index]);

        // Réinitialise la position locale à (0, 0, 0)
        newIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Réinitialise l'échelle si nécessaire
        newIcon.GetComponent<RectTransform>().localScale = Vector3.one;

        // Ajoute l'icône à la liste correspondante
        spawnedIcons[index].Add(newIcon);
    }

    // Supprime les icônes de ressource en fonction de l'index et de la quantité
    public void RemoveResourceIcons(int index, int quantity)
    {
        // Vérifie si l'index est valide
        if (index >= 0 && index < spawnedIcons.Count && spawnedIcons[index].Count > 0)
        {
            // Détruit les icônes correspondantes en fonction de la quantité
            for (int i = 0; i < quantity && spawnedIcons[index].Count > 0; i++)
            {
                GameObject iconToRemove = spawnedIcons[index][0];
                spawnedIcons[index].RemoveAt(0);
                Destroy(iconToRemove);
            }
        }
    }

    public void UpdateResourceText(int amount, int index)
    {
        ressourceTextCraft[index].text = amount + "x";
        ressourceTextHUD[index].text = amount + "x";
    }

    // Affiche ou masque le menu d'artisanat
    public void ToggleCraftMenu()
    {
        if (craftMenu.activeSelf)
        {
            HideCraftMenu();
        }
        else
        {
            ShowCraftMenu();
            Debug.Log("Craft Menu Shown");
        }
    }
    public void ShowCraftMenu()
    {
        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideMenu(true));

        
        foreach (AiguilleCraft aiguille in aiguilleCrafts)
        {
            aiguille.TogglePosition();
        }
    }

    public void HideCraftMenu()
    {
        // 🛑 Si un item est en cours de drag → on annule le drag proprement
        if (DragDropThing.currentDrag != null)
        {
            DragDropThing.currentDrag.ForceCancelDrag();
        }

        if (slideRoutine != null) StopCoroutine(slideRoutine);
        slideRoutine = StartCoroutine(SlideMenu(false));
        
        foreach (AiguilleCraft aiguille in aiguilleCrafts)
        {
            aiguille.TogglePosition();
        }
    }


    private IEnumerator SlideMenu(bool show)
    {
        Vector2 start = craftMenuRect.anchoredPosition;
        Vector2 end = show ? craftMenuOpenPos : craftMenuClosedPos;

        float t = 0f;

        if (show)
            craftMenu.SetActive(true);

        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / slideDuration);
            craftMenuRect.anchoredPosition = Vector2.Lerp(start, end, lerp);
            yield return null;
        }

        if (!show)
            craftMenu.SetActive(false);
    }


    // Affiche ou masque le menu d'inventaire d'équipement
    public void HideGearMenu()
    {
        gearInventoryMenu?.SetActive(false);
    }

    public void ShowGearMenu()
    {
        gearInventoryMenu?.SetActive(true);
    }

    // Met à jour la couleur du texte des coûts dans le menu de sélection des tours
    private void OnEnable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnManaChanged += UpdateSlotColorText;
    }

    private void OnDisable()
    {
        if (ResourceManager.Instance != null)
            ResourceManager.Instance.OnManaChanged -= UpdateSlotColorText;
    }

    public void UpdateSlotColorText(int mana)
    {
        GameObject[] towerPrefabs = TowerSelectMenuManager.Instance.towerPrefabs;
        TMP_Text[] costsText = TowerSelectMenuManager.Instance.slotCostTexts;

        for (int i = 0; i < towerPrefabs.Length; i++)
        {
            int cost = towerPrefabs[i].GetComponent<BaseTower>().spawnCost;

            costsText[i].color = (mana < cost) ? Color.red : Color.white;
        }
    }
    



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
           ToggleCraftMenu();
        }
        
    }
}
