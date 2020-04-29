﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "背包",menuName ="Items/背包",order =1)]
public class Bag : Item,IUseable
{
    [SerializeField]
    [Header("背包格子数量")]
    private int slots;

    [SerializeField]
    [Header("背包预制体")]
    private GameObject bagPrefab;

    public BagScript MyBagScript
    {
        get;set;
    }

    public BagButton MyBagButton
    {
        get;set;
    }

    public int MySlotCount
    {
        get => slots;
    }

    public void Initialize(int slots)
    {
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScript.MyInstance.CanAddBag)
        {
            Remove();
            MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
            MyBagScript.AddSlots(slots);
            if (MyBagButton == null)
            {
                InventoryScript.MyInstance.AddBag(this);
            }
            else
            {
                InventoryScript.MyInstance.AddBag(this,MyBagButton);
            }
            MyBagScript.MyBagIndex = MyBagButton.MyBagIndex;
        }
    }

    public void SetupScript()
    {
        MyBagScript = Instantiate(bagPrefab, InventoryScript.MyInstance.transform).GetComponent<BagScript>();
        MyBagScript.AddSlots(slots);
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\n {0} slot bag",slots);
    }
}
