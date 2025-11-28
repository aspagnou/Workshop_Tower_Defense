using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PreviewGearSlot : MonoBehaviour
{
    public GearSO currGear;
    public Image gearImage;
    public RectTransform gearTransform;

    private ResourceManager resourceManager;
    private GearManager gearManager;
    private CanvasGroup cg;


    public Canvas canvas;
    
    

    void Start()
    {
        
        cg = GetComponent<CanvasGroup>();
        UpdateSlotData();
    }

    public void UpdateSlotData()
    {
        if (currGear != null)
        {
            gearImage.sprite = currGear.gearIcon;
            // Réinitialiser l'alpha à 1 si un item est présent
            Color newColor = gearImage.color;
            newColor.a = 1f;
            gearImage.color = newColor;
        }
        else
        {
            gearImage.sprite = null;
            // Mettre l'alpha à 0 si aucun item n'est présent
            Color newColor = gearImage.color;
            newColor.a = 0f;
            gearImage.color = newColor;
        }
        gearTransform.anchoredPosition = Vector3.zero;
    }
}
