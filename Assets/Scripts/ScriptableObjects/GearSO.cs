using UnityEngine;

[CreateAssetMenu(fileName = "GearSO", menuName = "Gear /New Gear")]
public class GearSO : ScriptableObject
{
    public string gearName;
    public Sprite gearIcon;

    public ItemData gearInventoryData;

    [Header("Flat Modifiers")]
    public float flatAttackDamage;
    public float flatRange;
    public float flatAttackSpeed;
    public float flatCriticalChance;

    [Header("Percentage Modifiers (in %°")]
    public float percentAttackDamage;
    public float percentRange;
    public float percentAttackSpeed;
    


}
