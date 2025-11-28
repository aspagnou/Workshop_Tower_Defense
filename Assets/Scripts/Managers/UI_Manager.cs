using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Ressource_Icons")]
    public RectTransform[] rectImages;
    [SerializeField] private GameObject[] ressourceIcon;
    [SerializeField] private TMP_Text[] ressourceTextCraft, ressourceTextHUD;
    [SerializeField] private GameObject craftMenu;

    [Header("Gear Inventory")]
    [SerializeField] private GameObject gearInventoryMenu;

    public void SpawnResource(int index)
    {
        // Instancie l'icône comme enfant de rectImage
        GameObject newIcon = Instantiate(ressourceIcon[index], rectImages[index]);

        // Réinitialise la position locale à (0, 0, 0)
        newIcon.GetComponent<RectTransform>().localPosition = Vector3.zero;

        // Optionnel : Réinitialise l'échelle si nécessaire
        newIcon.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void UpdateResourceText(int amount, int index)
    {
        ressourceTextCraft[index].text = amount + "x";
        ressourceTextHUD[index].text = amount + "x";
    }
    public void ShowCraftMenu() 
    {
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
}
