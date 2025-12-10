using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image highlightObject;
    public Color UpgradableColor = Color.yellow;
    public Color NonUpgradableColor = Color.red;
    
    
    
    public bool isUpgradable = true;

    private void Start()
    {
        if (highlightObject != null)
            highlightObject.enabled = false; // Masqué au départ
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.towerUpgradeManager;
        towerUpgrade.UpdateUpgradeButtonState();
        if (highlightObject == null)
            return;
        if (towerUpgrade.CanUpgrade())
        {
            highlightObject.enabled = true;
            Debug.Log(highlightObject.name);
        }
        
        //highlightObject.enabled = true;
        //highlightObject.color = isUpgradable ? UpgradableColor : NonUpgradableColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightObject == null)
            return;

        highlightObject.enabled = false;
    }
}
