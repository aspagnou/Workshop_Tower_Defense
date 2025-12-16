using System;
using UnityEngine;

[Serializable]
public class UpgradeLevel
{
    public ItemSO[] scraps;
    public int[] costs;
    public int manaCost;

    [Header("Base Stats")]
    public float baseAttackDamage;
    public float baseRange;
    public float baseAttackSpeed;
    public float baseCriticalChance;
}
