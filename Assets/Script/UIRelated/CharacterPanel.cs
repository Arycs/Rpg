using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel instance;
    public static CharacterPanel MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterPanel>();
            }
            return instance;
        }
    }

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private CharButton head, shoulders, chest, hands, legs, feet, main, off;

    public CharButton MySelectedButton
    {
        get;set;
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha <= 0)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.MyArmorType)
        {
            case ArmorType.Head:
                head.EquipArmor(armor);
                break;
            case ArmorType.Shoulders:
                shoulders.EquipArmor(armor);
                break;
            case ArmorType.Chest:
                chest.EquipArmor(armor);
                break;
            case ArmorType.Hands:
                hands.EquipArmor(armor);
                break;
            case ArmorType.Legs:
                legs.EquipArmor(armor);
                break;
            case ArmorType.Feet:
                feet.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                main.EquipArmor(armor);
                break;
            case ArmorType.OffHand:
                off.EquipArmor(armor);
                break;
            case ArmorType.TwoHand:
                break;
            default:
                break;
        }
    }
}
