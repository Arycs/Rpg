using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup canvasGroup;

    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots { get; } = new List<SlotScript>();

    public int MyBagIndex
    {
        get; set;
    }

    /// <summary>
    /// 当前背包空格数量
    /// </summary>
    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }   
            }
            return count;
        }
    }

    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 获取当前背包中所有的物品
    /// </summary>
    /// <returns>物品的列表</returns>
    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (SlotScript slot in MySlots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }
        return items;
    }


    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
           SlotScript slot =  Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyIndex = i;
            slot.MyBag = this;
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="item">物品</param>
    /// <returns>是否添加成功</returns>
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }

    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void Clear()
    {
        foreach (SlotScript slot in MySlots)
        {
            slot.Clear();
        }
    }

}
