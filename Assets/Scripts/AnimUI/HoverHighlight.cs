using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image highlightObject;
    public Color UpgradableColor = Color.yellow;
    public Color NonUpgradableColor = Color.red;
    //private HoverScaleAndColor hoverHighlight;
    
    
    
    public bool isUpgradable = true;

    private void Start()
    {
        if (highlightObject != null)
            highlightObject.enabled = false; // Masqué au départ
        //hoverHighlight = GetComponent<HoverScaleAndColor>();
        //if(hoverHighlight != null) 
        //{
        //    hoverHighlight.enabled = false;
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.towerUpgradeManager;
        towerUpgrade.UpdateUpgradeButtonState();
        if (highlightObject == null)
            return;
        if (towerUpgrade.CanUpgrade())
        {
            //hoverHighlight.enabled = true;
            highlightObject.enabled = true;
            Debug.Log(highlightObject.name);
        }
        //else
        //{
        //    hoverHighlight.enabled = true;
        //}

            //highlightObject.enabled = true;
            //highlightObject.color = isUpgradable ? UpgradableColor : NonUpgradableColor;
        }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlightObject == null)
            return;

        //highlightObject.enabled = false;

    }
}
