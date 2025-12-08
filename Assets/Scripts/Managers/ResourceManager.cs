using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    [Header("Mana")]
    public int mana = 0;
    [SerializeField] TMP_Text manaText;

    [Header("scraps")]
    public ItemSO[] scraps;

    [Header("References")]
    [SerializeField] private UI_Manager ui_Manager;
    

    public ItemSlot[] craftingSlots;
    public event Action<int> OnManaChanged;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        foreach (ItemSO resource in scraps)
        {
            resource.amount = 0;
            ui_Manager.UpdateResourceText(resource.amount,resource.index);
        }
        UpdateManaText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (ItemSO resource in scraps)
            {
                AddResource(resource, 1);
            }
            AddMana(100);
        }

    }
    public void AddResource(ItemSO resource, int amount)
    {
        resource.amount += amount;
        ui_Manager.SpawnResource(resource.index);
        ui_Manager.UpdateResourceText(resource.amount, resource.index);

        // Met à jour les couleurs des lignes de coût
        if (Clicker.Instance.currentSelectedTower != null)
        {
            Clicker.Instance.currentSelectedTower.towerUpgradeManager.UpdateCostLineColors();
        }
    }

    public void UseResource(ItemSO resource, int amount)
    {
        resource.amount -= amount;
        if (resource.amount <= 0)
        {
            resource.amount = 0;
        }
        ui_Manager.UpdateResourceText(resource.amount, resource.index);

        // Met à jour les couleurs des lignes de coût
        if (Clicker.Instance.currentSelectedTower != null)
        {
            Clicker.Instance.currentSelectedTower.towerUpgradeManager.UpdateCostLineColors();
        }
    }

    public void AddMana(int amount) 
    {
        mana += amount;
        UpdateManaText();
        DispatchManaChanged();
        if (Clicker.Instance.currentSelectedTower != null)
        {
            Clicker.Instance.currentSelectedTower.towerUpgradeManager.UpdateCostLineColors();
            
        }
    }

    public void SpendMana(int amount) 
    {
        mana-= amount;
        if (mana <= 0) { mana = 0; }
        UpdateManaText();
        DispatchManaChanged();
        if (Clicker.Instance.currentSelectedTower != null)
        {
            Clicker.Instance.currentSelectedTower.towerUpgradeManager.UpdateCostLineColors();
        }
    }
    private void DispatchManaChanged()
    {
        OnManaChanged?.Invoke(mana);
    }
    private void UpdateManaText()
    {
        manaText.text = $"x {mana}";
    }

    public void ClearSlots() 
    {
        foreach(ItemSlot itemSlot in craftingSlots) 
        {
            itemSlot.currItem = null;
            itemSlot.UpdateSlotData();
        }
    }
    public void CancelCraft() 
    {
        foreach(ItemSlot itemSlot in craftingSlots) 
        {
            if(itemSlot.currItem != null) 
            {
                AddResource(itemSlot.currItem, 1);
            }
        }
        ClearSlots();
    }
}
