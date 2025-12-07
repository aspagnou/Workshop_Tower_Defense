using System;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public int mana = 0;
    [SerializeField] TMP_Text manaText;
    public ItemSO[] resources;
    
    [SerializeField] private UI_Manager ui_Manager;
    public ItemSlot[] craftingSlots;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        foreach (ItemSO resource in resources)
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
            foreach (ItemSO resource in resources)
            {
                AddResource(resource, 1);
            }
            AddMana(100);
        }
        //if (Input.GetKeyDown(KeyCode.A)) 
        //{
        //    ui_Manager.ShowGearMenu();
        //}

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
        if (Clicker.Instance.currentSelectedTower != null)
        {
            Clicker.Instance.currentSelectedTower.towerUpgradeManager.UpdateCostLineColors();
        }
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
