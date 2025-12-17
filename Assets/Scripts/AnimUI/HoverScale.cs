using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverScaleAndColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale Settings")]
    public float hoverScale = 1.2f;     // Taille au survol
    public float scaleSpeed = 10f;      // Vitesse de l’animation

    [Header("Color Settings")]
    public Color hoverColor = Color.white;
    public Color refuseColor = Color.red;// Couleur au survol
    public float colorSpeed = 10f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    private Color initialColor;
    private Color targetColor;

    public bool isUpgradeButton = false;


    private Image img;   // L'Image du bouton
    private float dt => Time.unscaledDeltaTime;

    private void Start()
    {
        // On récupère l'image si elle existe (obligatoire pour changer la couleur)
        img = GetComponent<Image>();
        if (img != null)
        {
            initialColor = img.color;
            targetColor = initialColor;
        }

        initialScale = transform.localScale;
        targetScale = initialScale;
    }

    private void Update()
    {
        // Animation de scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, dt * scaleSpeed);

        // Animation de couleur
        if (img != null)
            img.color = Color.Lerp(img.color, targetColor, dt * colorSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = initialScale * hoverScale;
        targetColor = hoverColor;
        
        if (isUpgradeButton)
        {
            TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.towerUpgradeManager;
            towerUpgrade.UpdateUpgradeButtonState();
            if (towerUpgrade.CanUpgrade())
            {
                //hoverHighlight.enabled = true;
                targetColor = hoverColor;


            }
            else
            {
                targetColor = refuseColor;
            }
        }
    }                                    

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = initialScale;
        targetColor = initialColor;
    }
    public void ResetToInitial()
    {
        targetScale = initialScale;
        targetColor = initialColor;
    }

}
