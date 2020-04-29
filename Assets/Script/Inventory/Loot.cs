using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 掉落物品
/// </summary>
[System.Serializable]
public class Loot
{
    [Header("掉落物品")]
    [SerializeField]
    private Item item;

    [Header("掉落几率")]
    [SerializeField]
    private float dropChance;

    public Item MyItem
    {
        get => item;
    }
    public float MyDropChance
    {
        get => dropChance;
    }
}
