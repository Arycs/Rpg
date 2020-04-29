using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 掉落物品表
/// </summary>
public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] loot;

    private List<Item> droppedItems = new List<Item>();

    /// <summary>
    /// 掉落物品UI是否显示
    /// </summary>
    private bool rooled = false;

    public void ShowLoot()
    {
        if (!rooled)
        {
            RollLoot();
        }

        LootWindow.MyInstance.CreatePages(droppedItems);
    }

    /// <summary>
    /// 随机出掉落物品
    /// </summary>
    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);
            if (roll <= item.MyDropChance)
            {
                droppedItems.Add(item.MyItem);
            }
        }
        rooled = true;
    }
}
