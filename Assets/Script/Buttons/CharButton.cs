using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{

    [SerializeField]
    private ArmorType armorType;

    private Armor equippedArmor;

    [SerializeField]
    private Image icon;

    public Armor MyEquippedArmor
    {
        get => equippedArmor;
        set => equippedArmor = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMoveable is Armor)
            {
                Armor tmp = (Armor)HandScript.MyInstance.MyMoveable;
                if (tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }
                UIManager.MyInstance.RefreshToolTip(tmp);
            }
            else if (HandScript.MyInstance.MyMoveable == null && MyEquippedArmor != null)
            {
                HandScript.MyInstance.TakeMoveable(MyEquippedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.grey;
            }
        }
    }   
    /// <summary>
    /// 穿戴装备
    /// </summary>
    /// <param name="armor"></param>
    public void EquipArmor(Armor armor)
    {
        armor.Remove();

        if (MyEquippedArmor != null)
        {
            if (MyEquippedArmor != armor)
            {
                Debug.Log(MyEquippedArmor);
                armor.MySlot.AddItem(MyEquippedArmor);
            }
            UIManager.MyInstance.RefreshToolTip(MyEquippedArmor);
        }
        else
        {
            UIManager.MyInstance.HideToolTip();
        }


        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        this.MyEquippedArmor = armor;
        this.MyEquippedArmor.MyCharButton = this;
        if (HandScript.MyInstance.MyMoveable == (armor as IMoveable))
        {
            HandScript.MyInstance.Drop();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyEquippedArmor != null)
        {
           
            UIManager.MyInstance.ShowToolTip(new Vector2(0,1),transform.position, MyEquippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideToolTip();
    }

    public void DequipArmor()
    {
        icon.color = Color.white;
        icon.enabled = false;
        MyEquippedArmor.MyCharButton = null;
        MyEquippedArmor = null;
    }
}
