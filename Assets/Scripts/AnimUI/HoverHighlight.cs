using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image highlightObject;
    public Color UpgradableColor = Color.yellow;
    public Color NonUpgradableColor = Color.red;
    private HoverScaleAndColor hoverHighlight;
    private float dt => Time.unscaledDeltaTime;
    //private Coroutine disableRoutine;



    public bool isUpgradable = true;

    private void Start()
    {
        if (highlightObject != null)
            highlightObject.enabled = false; // Masqué au départ
        hoverHighlight = GetComponent<HoverScaleAndColor>();
        if(hoverHighlight != null)
        {
            hoverHighlight.enabled = false;
        }
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
            Debug.Log("hover activé");
            highlightObject.enabled = true;
            
        }
        else
        {
            //hoverHighlight.enabled = true;
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
    private void Update()
    {
        //if (!Clicker.Instance.isUpgradeOpen) return;

        //TowerUpgrade towerUpgrade = Clicker.Instance.currentSelectedTower.towerUpgradeManager;

        //towerUpgrade.UpdateUpgradeButtonState();

        //if (towerUpgrade.CanUpgrade())
        //{
        //    // Réactive immédiatement si possible
        //    if (!hoverHighlight.enabled)
        //        hoverHighlight.enabled = true;

        //    if (disableRoutine != null)
        //    {
        //        StopCoroutine(disableRoutine);
        //        disableRoutine = null;
        //    }
        //}
        //else
        //{
        //    // Lance le retour propre UNIQUEMENT si pas déjà en cours
        //    if (hoverHighlight.enabled && disableRoutine == null)
        //    {
        //        disableRoutine = StartCoroutine(DisableHoverAfterReset());
        //    }
        //}
    }
    private IEnumerator DisableHoverAfterReset()
    {
        // Force le retour à l’état initial
        hoverHighlight.ResetToInitial();

        // Attend que le scale soit presque revenu
        while (Vector3.Distance(transform.localScale, Vector3.one) > 0.01f)
            yield return null;

        hoverHighlight.enabled = false;
        //disableRoutine = null;
    }


}
