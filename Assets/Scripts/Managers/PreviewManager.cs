using UnityEngine;
using System.Collections.Generic;

public class PreviewManager : MonoBehaviour
{
    public PreviewGearSlot previewGearSlot;
    private List<RecipeSO> recipes = new List<RecipeSO>();
    private int currentRecipeIndex = 0;

    public ItemSlot[] topRow = new ItemSlot[3];
    public ItemSlot[] midRow = new ItemSlot[3];
    public ItemSlot[] bottomRow = new ItemSlot[3];

    private List<ItemSlot[]> allSlots = new List<ItemSlot[]>();
    void Start()
    {
        allSlots.Add(topRow);
        allSlots.Add(midRow);
        allSlots.Add(bottomRow);


        RecipeSO[] loadedRecipes = Resources.LoadAll<RecipeSO>("Recipes/");
        recipes.AddRange(loadedRecipes);

        // Vérifier si des recettes ont été chargées
        if (recipes.Count > 0)
        {
            // Afficher la première recette
            UpdatePreviewGearSlot();
        }
        else
        {
            Debug.LogWarning("Aucune recette trouvée dans le dossier Resources/Recipes/");
        }

        //foreach(RecipeSO recipe in recipes) {Debug.Log(recipe.name); }
        
        
    }
    public void ShowPreviewScraps()
    {
        if (recipes.Count == 0 || currentRecipeIndex < 0 || currentRecipeIndex >= recipes.Count)
        {
            Debug.LogWarning("Aucune recette sélectionnée ou liste vide.");
            return;
        }

        RecipeSO currentRecipe = recipes[currentRecipeIndex];

        // Mettre à jour les slots avec les items de la recette
        for (int i = 0; i < 3; i++)
        {
            // Top Row
            if (topRow[i] != null)
            {
                topRow[i].currItem = (i < currentRecipe.topRow.Length) ? currentRecipe.topRow[i] : null;
                topRow[i].UpdateSlotData();
                // Appliquer un alpha de 0.5 si un item est présent, sinon 0
                if (topRow[i].itemImage != null)
                {
                    Color newColor = topRow[i].itemImage.color;
                    newColor.a = (topRow[i].currItem != null) ? 0.5f : 0f;
                    topRow[i].itemImage.color = newColor;
                }
            }

            // Mid Row
            if (midRow[i] != null)
            {
                midRow[i].currItem = (i < currentRecipe.midRow.Length) ? currentRecipe.midRow[i] : null;
                midRow[i].UpdateSlotData();
                // Appliquer un alpha de 0.5 si un item est présent, sinon 0
                if (midRow[i].itemImage != null)
                {
                    Color newColor = midRow[i].itemImage.color;
                    newColor.a = (midRow[i].currItem != null) ? 0.5f : 0f;
                    midRow[i].itemImage.color = newColor;
                }
            }

            // Bottom Row
            if (bottomRow[i] != null)
            {
                bottomRow[i].currItem = (i < currentRecipe.bottomRow.Length) ? currentRecipe.bottomRow[i] : null;
                bottomRow[i].UpdateSlotData();
                // Appliquer un alpha de 0.5 si un item est présent, sinon 0
                if (bottomRow[i].itemImage != null)
                {
                    Color newColor = bottomRow[i].itemImage.color;
                    newColor.a = (bottomRow[i].currItem != null) ? 0.5f : 0f;
                    bottomRow[i].itemImage.color = newColor;
                }
            }
        }
    }




    // Met à jour l'affichage du PreviewGearSlot avec la recette actuelle
    private void UpdatePreviewGearSlot()
    {
        if (recipes.Count == 0 || previewGearSlot == null)
            return;

        RecipeSO currentRecipe = recipes[currentRecipeIndex];
        if (currentRecipe.output != null)
        {
            previewGearSlot.currGear = currentRecipe.output;
            previewGearSlot.UpdateSlotData();
            ShowPreviewScraps();
        }
    }

    // Affiche la recette précédente
    public void ShowPreviousRecipe()
    {
        if (recipes.Count == 0)
            return;

        currentRecipeIndex--;
        if (currentRecipeIndex < 0)
            currentRecipeIndex = recipes.Count - 1;

        UpdatePreviewGearSlot();
    }

    // Affiche la recette suivante
    public void ShowNextRecipe()
    {
        if (recipes.Count == 0)
            return;

        currentRecipeIndex++;
        if (currentRecipeIndex >= recipes.Count)
            currentRecipeIndex = 0;

        UpdatePreviewGearSlot();
    }

    
}
