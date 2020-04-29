using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ArmorType
{
    Head,Shoulders,Chest,Hands,Legs,Feet,MainHand,OffHand,TwoHand
}

[CreateAssetMenu(fileName = "装备", menuName = "Items/装备", order = 2)]
public class Armor :Item
{
    [SerializeField]
    [Header("装备类型")]
    private ArmorType armorType;

    [SerializeField]
    [Header("智力")]
    private int intellect;

    [SerializeField]
    [Header("力量")]
    private int strength;

    [SerializeField]
    [Header("体力")]
    private int stamina;

    internal ArmorType MyArmorType
    {
        get => armorType;
    }

    public override string GetDescription()
    {
        string stats = string.Empty;
        if (intellect > 0)
        {
            stats += string.Format("\n + {0} intellect", intellect);
        }
        if (strength > 0)
        {
            stats += string.Format("\n + {0} strength", strength);
        }
        if (stamina > 0)
        {
            stats += string.Format("\n + {0} stamina", stamina);
        }
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}
