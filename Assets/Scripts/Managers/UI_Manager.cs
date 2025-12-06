using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    [Header("Ressource_Icons")]
    public RectTransform[] rectImages;
    [SerializeField] private GameObject[] ressourceIcon;
    [SerializeField] private TMP_Text[] ressourceTextCraft, ressourceTextHUD;
    [SerializeField] private GameObject craftMenu;
    [SerializeField] private GameObject gearInventoryMenu;
    
    public TMP_Text[] statLines;
    public StatChangeFeedback[] statsUpFeedback;


    // Dictionnaire pour stocker les icônes instanciées par type de ressource
    private Dictionary<int, List<GameObject>> spawnedIcons = new Dictionary<int, List<GameObject>>();

    private void Start()
    {
        Instance = this;
        HideGearMenu();
        // Initialiser le dictionnaire
        for (int i = 0; i < ressourceIcon.Length; i++)
        {
            spawnedIcons[i] = new List<GameObject>();
        }
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

    public void ShowCraftMenu()
    {
        Debug.Log("Jactive");
        craftMenu.SetActive(true);
    }

    public void HideCraftMenu()
    {
        craftMenu?.SetActive(false);
    }

    public void HideGearMenu()
    {
        gearInventoryMenu?.SetActive(false);
    }

    public void ShowGearMenu()
    {
        gearInventoryMenu?.SetActive(true);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ( craftMenu.activeSelf)
            {
                HideCraftMenu();
            }
            else
            {
                ShowCraftMenu();
            }
        }
        
    }
}
