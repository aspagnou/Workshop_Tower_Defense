using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TowerUpgrade : MonoBehaviour
{
    [Header("Upgrade Levels")]
    public UpgradeLevel[] upgradeLevels;

    [SerializeField] GameObject costLinePrefab;
    private Transform lineSpawnTransform;
    private Clicker clicker;
    public int currentLevel = 0;

    private List<GameObject> costLines = new List<GameObject>();


    void Start()
    {
        clicker = FindAnyObjectByType<Clicker>();
        lineSpawnTransform = GameObject.FindWithTag("CostContainer").transform.GetChild(0).GetChild(0);
    }

    public void Show(int level)
    {
        currentLevel = level;

        // Supprime les anciennes lignes (si elles existent)
        foreach (Transform child in lineSpawnTransform)
        {
            Destroy(child.gameObject);
        }

        // Nettoie la liste des lignes de coût
        costLines.Clear();

        // Vérifie que le niveau est valide
        if (level < 0 || level >= upgradeLevels.Length)
        {
            Debug.LogError("Niveau d'upgrade invalide.");
            return;
        }

        // Affiche les coûts pour le niveau sélectionné
        for (int i = 0; i < upgradeLevels[level].costs.Length; i++)
        {
            AddCostLine(upgradeLevels[level].scraps[i].itemIcon, upgradeLevels[level].costs[i], upgradeLevels[level].scraps[i].index);
        }
    }

    public void AddCostLine(Sprite icon, int value, int resourceIndex)
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

        // Vérifie si la quantité de ressource est suffisante
        if (ResourceManager.Instance.resources[resourceIndex].amount >= value)
        {
            costText.color = Color.green; // Vert si assez de ressources
        }
        else
        {
            costText.color = Color.red; // Rouge si pas assez de ressources
        }

        // Ajoute la ligne à la liste
        costLines.Add(statLine);
    }

    public void UpdateCostLineColors()
    {
        if (currentLevel < 0 || currentLevel >= upgradeLevels.Length)
        {
            Debug.LogError("Niveau d'upgrade invalide.");
            return;
        }

        // Parcourir les lignes de coût
        for (int i = costLines.Count - 1; i >= 0; i--)
        {
            if (costLines[i] == null)
            {
                // Supprimer les références nulles
                costLines.RemoveAt(i);
                continue;
            }

            if (i < upgradeLevels[currentLevel].costs.Length)
            {
                TMP_Text costText = costLines[i].transform.GetChild(1).GetComponent<TMP_Text>();
                if (costText != null)
                {
                    int resourceIndex = upgradeLevels[currentLevel].scraps[i].index;
                    int requiredAmount = upgradeLevels[currentLevel].costs[i];

                    if (ResourceManager.Instance.resources[resourceIndex].amount >= requiredAmount)
                    {
                        costText.color = Color.green; // Vert si assez de ressources
                    }
                    else
                    {
                        costText.color = Color.red; // Rouge si pas assez de ressources
                    }
                }
            }
        }
    }

    public void ConfirmUpgrade()
    {
        // Vérifie que le niveau est valide
        if (currentLevel < 0 || currentLevel >= upgradeLevels.Length)
        {
            Debug.LogError("Niveau d'upgrade invalide.");
            return;
        }

        // Vérifie les ressources disponibles
        for (int i = 0; i < upgradeLevels[currentLevel].costs.Length; i++)
        {
            if (ResourceManager.Instance.resources[i].amount < upgradeLevels[currentLevel].costs[i])
            {
                Debug.Log("Not enough resources");
                return;
            }
        }

        // Enlève les scraps
        for (int i = 0; i < upgradeLevels[currentLevel].costs.Length; i++)
        {
            ResourceManager.Instance.UseResource(upgradeLevels[currentLevel].scraps[i], upgradeLevels[currentLevel].costs[i]);
            UI_Manager.Instance.RemoveResourceIcons(upgradeLevels[currentLevel].scraps[i].index, upgradeLevels[currentLevel].costs[i]);
        }

        // Passe au niveau suivant
        currentLevel++;
        ItemGrid grid = Clicker.Instance.currentSelectedTower.itemGrid;
        if (grid!= null)
            grid.ResizeGrid(grid.gridSizeWidth+1, grid.gridSizeHeight);
        Show(currentLevel);
    }
}
