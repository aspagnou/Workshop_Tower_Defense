using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class PreviewManager : MonoBehaviour
{
    public PreviewGearSlot previewGearSlot;
    [Header("Tier Buttons")]

    public Button[] tierButtons;   // 4 boutons : Tier1, Tier2, Tier3, Tier4

    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;


    // ➜ Listes par tier
    private List<RecipeSO> tier1 = new List<RecipeSO>();
    private List<RecipeSO> tier2 = new List<RecipeSO>();
    private List<RecipeSO> tier3 = new List<RecipeSO>();
    private List<RecipeSO> tier4 = new List<RecipeSO>();

    // ➜ Liste active (dépend du bouton cliqué)
    private List<RecipeSO> activeList = null;

    private int currentRecipeIndex = 0;

    public ItemSlot[] topRow = new ItemSlot[3];
    public ItemSlot[] midRow = new ItemSlot[3];
    public ItemSlot[] bottomRow = new ItemSlot[3];

    void Start()
    {
        // Charger toutes les recettes
        RecipeSO[] loaded = Resources.LoadAll<RecipeSO>("Recipes/");

        // Distribuer les recettes selon leur tier
        foreach (var recipe in loaded)
        {
            switch (recipe.tier)
            {
                case 1: tier1.Add(recipe); break;
                case 2: tier2.Add(recipe); break;
                case 3: tier3.Add(recipe); break;
                case 4: tier4.Add(recipe); break;
                default:
                    Debug.LogWarning($"{recipe.name} a un tier invalide : {recipe.tier}");
                    break;
            }
        }

        // ➜ Choisir un tier par défaut (Tier 1)
        SelectTier(1);
    }

    // ----------------------------------------------------------
    // ------------------------ TIER BUTTONS ---------------------
    // ----------------------------------------------------------
    public void SelectTier(int tier)
    {
        switch (tier)
        {
            case 1: activeList = tier1; break;
            case 2: activeList = tier2; break;
            case 3: activeList = tier3; break;
            case 4: activeList = tier4; break;
        }

        HighlightSelectedTier(tier);

        if (activeList == null || activeList.Count == 0)
        {
            Debug.LogWarning($"Aucune recette dans le tier {tier} !");
            ClearPreview();
            return;
        }

        currentRecipeIndex = 0;
        UpdatePreviewGearSlot();
    }

    // ----------------------------------------------------------
    // --------------------- NAVIGATION -------------------------
    // ----------------------------------------------------------
    public void ShowPreviousRecipe()
    {
        if (activeList == null || activeList.Count == 0) return;

        currentRecipeIndex--;
        if (currentRecipeIndex < 0) currentRecipeIndex = activeList.Count - 1;

        UpdatePreviewGearSlot();
    }

    public void ShowNextRecipe()
    {
        if (activeList == null || activeList.Count == 0) return;

        currentRecipeIndex++;
        if (currentRecipeIndex >= activeList.Count) currentRecipeIndex = 0;

        UpdatePreviewGearSlot();
    }

    // ----------------------------------------------------------
    // ---------------------- DISPLAY RECIPE ---------------------
    // ----------------------------------------------------------
    private void UpdatePreviewGearSlot()
    {
        if (activeList == null || activeList.Count == 0) return;
        if (previewGearSlot == null) return;

        RecipeSO current = activeList[currentRecipeIndex];

        previewGearSlot.currGear = current.output;
        previewGearSlot.UpdateSlotData();

        ShowPreviewScraps(current);
    }

    private void ShowPreviewScraps(RecipeSO recipe)
    {
        FillRow(topRow, recipe.topRow);
        FillRow(midRow, recipe.midRow);
        FillRow(bottomRow, recipe.bottomRow);
    }

    private void FillRow(ItemSlot[] row, ItemSO[] data)
    {
        for (int i = 0; i < 3; i++)
        {
            if (row[i] == null) continue;

            row[i].currItem = (i < data.Length) ? data[i] : null;
            row[i].UpdateSlotData();

            if (row[i].itemImage)
            {
                Color c = row[i].itemImage.color;
                c.a = (row[i].currItem != null) ? 0.5f : 0f;
                row[i].itemImage.color = c;
            }
        }
    }

    private void ClearPreview()
    {
        FillRow(topRow, new ItemSO[3]);
        FillRow(midRow, new ItemSO[3]);
        FillRow(bottomRow, new ItemSO[3]);

        previewGearSlot.currGear = null;
        previewGearSlot.UpdateSlotData();
    }

    private void HighlightSelectedTier(int tier)
    {
        if (tierButtons == null || tierButtons.Length < 4)
            return;

        for (int i = 0; i < tierButtons.Length; i++)
        {
            Image img = tierButtons[i].GetComponent<Image>();
            if (img == null) continue;

            img.color = (i == tier - 1) ? selectedColor : normalColor;
        }
    }

}
