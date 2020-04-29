using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 技能书上的魔法按键
/// </summary>
public class SpellButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string spellName;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandScript.MyInstance.TakeMoveable(SpellBook.MyInstance.GetSpell(spellName));
        }
    }
}
