using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [Header("Ressource_Icons")]
    public RectTransform[] rectImages;
    [SerializeField] private GameObject[] ressourceIcon;
    [SerializeField] private TMP_Text[] ressourceText;

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
        ressourceText[index].text = amount + "x";
    }
}
