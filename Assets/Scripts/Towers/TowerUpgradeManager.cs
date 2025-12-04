using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{

    [Header("Level 1 Upgrade Costs")]
    public ItemSO[] level1_scraps;
    public int[] level1_costs;
    
    [SerializeField] GameObject costLinePrefab;
    private Transform lineSpawnTransform;
    private Clicker clicker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clicker = FindAnyObjectByType<Clicker>();
        lineSpawnTransform = GameObject.FindWithTag("CostContainer").transform.GetChild(0).GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        
        BaseTower tower = clicker.currentSelectedTower;
        // Supprime les anciennes lignes (si elles existent)
        foreach (Transform child in lineSpawnTransform)
        {
             Destroy(child.gameObject);
            
        }

        for (int i = 0; i < level1_costs.Length; i++)
        {
            AddCostLine(level1_scraps[i].itemIcon, level1_costs[i]);
            Debug.Log(level1_scraps[i].itemIcon.name);
        }
    }
    public void Hide() 
    {
        //gameObject.SetActive(false);
    }

    public void AddCostLine(Sprite icon, int value)
    {
        if (value == 0) return; // Ne pas afficher si la valeur est 0

        // Instancie une nouvelle ligne
        GameObject statLine = Instantiate(costLinePrefab, lineSpawnTransform);

        // Récupère les composants de la ligne
        Image iconImage = statLine.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        TMP_Text costText = statLine.transform.GetChild(1).GetComponent<TMP_Text>();

        // Configure l'icône
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(icon != null);

        // Configure le texte
        costText.text = $"x{value}";
    }
}
