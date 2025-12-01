using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Net.Http.Headers;

public class ToolTipManager : MonoBehaviour
{
    public Canvas parentCanvas;
    public Transform toolTipTransform;
    public static ToolTipManager Instance;
    public TMP_Text Title, Details;
    public CanvasGroup toolTipCanvasGroup;
    [SerializeField] private Transform lineSpawnTransform;




    bool isShowing;

    // Icônes pour chaque stat
    public Sprite attackDamageIcon;
    public Sprite rangeIcon;
    public Sprite attackSpeedIcon;
    public Sprite criticalChanceIcon;

    [SerializeField] GameObject statLinePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        isShowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;
        if (isShowing) 
        {
            if (toolTipCanvasGroup.alpha < 1) 
            {
                toolTipCanvasGroup.alpha += Time.deltaTime*3;
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out movePos);
            toolTipTransform.position = parentCanvas.transform.TransformPoint(movePos);

        }
    }


    public void Show(GearSO gear)
    {
        if (gear == null) return;
        toolTipCanvasGroup.alpha = 0;
        Title.text = gear.gearName;

        // Supprime les anciennes lignes (si elles existent)
        foreach (Transform child in lineSpawnTransform)
        {
            if (child.gameObject != Title.gameObject) // Garde le titre
            {
                Destroy(child.gameObject);
            }
        }

        // Ajoute les stats flat
        AddStatLine(attackDamageIcon, gear.flatAttackDamage, false);
        AddStatLine(rangeIcon, gear.flatRange, false);
        AddStatLine(attackSpeedIcon, gear.flatAttackSpeed, false);
        AddStatLine(criticalChanceIcon, gear.flatCriticalChance, false);

        // Ajoute les stats en pourcentage
        AddStatLine(attackDamageIcon, gear.percentAttackDamage, true);
        AddStatLine(rangeIcon, gear.percentRange, true);
        AddStatLine(attackSpeedIcon, gear.percentAttackSpeed, true);

        toolTipTransform.gameObject.SetActive(true);
        isShowing = true;
    }

    // Méthode pour ajouter une ligne de stat
    private void AddStatLine(Sprite icon, float value, bool isPercent)
    {
        if (value == 0) return; // Ne pas afficher si la valeur est 0

        // Instancie une nouvelle ligne
        GameObject statLine = Instantiate(statLinePrefab, lineSpawnTransform);

        // Récupère les composants de la ligne
        Image iconImage = statLine.transform.GetChild(0).GetComponent<Image>();
        TMP_Text statText = statLine.transform.GetChild(1).GetComponent<TMP_Text>();

        // Configure l'icône
        iconImage.sprite = icon;
        iconImage.gameObject.SetActive(icon != null);

        // Configure le texte
        if (isPercent)
        {
            statText.text = $"+{value}%";
        }
        else
        {
            statText.text = $"+{value}";
        }
    }

    public void Hide()
    {
        toolTipTransform.gameObject.SetActive(false);
        isShowing = false;
    }
}
