using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject,IMoveable,IDescribable
{
    [SerializeField]
    [Header("物品图标")]
    private Sprite icon;

    [SerializeField]
    [Header("物品叠加上限")]
    private int stackSize;

    [SerializeField]
    [Header("物品名字")]
    private string title;

    [SerializeField]
    [Header("物品品质")]
    private Quality quality;

    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    [Header("物品价格")]
    private int price;

    public CharButton MyCharButton
    {
        get => charButton;
        set
        {
            MySlot = null;
            charButton = value;
        }
    }

    public Sprite MyIcon
    {
        get => icon;
    }
    public int MyStackSize
    {
        get => stackSize;
    }


    public SlotScript MySlot
    {
        get => slot;
        set => slot = value;
    }
    public Quality MyQuality
    {
        get => quality;
    }
    public string MyTitle
    {
        get => title;
    }
    public int MyPrice
    {
        get => price;
    }

    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>",QualityColor.MyColors[MyQuality],MyTitle);
    }

    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
